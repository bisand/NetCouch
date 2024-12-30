using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NetCouch.Linq;

public class CouchDbViewQueryBuilder
{
    private readonly StringBuilder _query;
    private readonly CouchDbTranslation _translation;
    private readonly StringBuilder _view;

    private const string DocTypeCheck = "function(doc) { if (doc.doc__type && doc.doc__type == '";

    public CouchDbViewQueryBuilder(CouchDbTranslation translation)
    {
        _translation = translation ?? throw new ArgumentNullException(nameof(translation));
        _query = new StringBuilder();
        _view = new StringBuilder();
        _view.Append(DocTypeCheck);
    }

    public ViewAndQuery Build()
    {
        var notEqualStatements = _translation.Statements.Where(x => x.NodeType == ExpressionType.NotEqual).ToList();
        var equalStatements = _translation.Statements.Where(x => x.NodeType == ExpressionType.Equal).ToList();
        var equalGroups = equalStatements.Select(x => x.Level).GroupBy(g => g).ToList();

        _view.Append(_translation.DesignDocName);
        _view.Append("') { ");
        // emit operations go here

        AppendNotEqualStatements(notEqualStatements);
        AppendEqualStatements(equalStatements);

        _view.Append(" } } }");

        return new ViewAndQuery { View = _view.ToString(), Query = _query.ToString() };
    }

    private void AppendNotEqualStatements(List<Statement> notEqualStatements)
    {
        if (notEqualStatements.Count > 0)
        {
            _view.Append("if (");
            for (int i = 0; i < notEqualStatements.Count; i++)
            {
                var statement = notEqualStatements[i];
                if (i > 0)
                {
                    _view.Append(statement.LastExprType == ExpressionType.And ? " && " : " || ");
                }

                AppendStatement(statement);
            }
            _view.Append(") { ");
        }
    }

    private void AppendEqualStatements(List<Statement> equalStatements)
    {
        if (equalStatements.Count > 0)
        {
            _query.Append("keys=[");
            Statement? prevExpr = null;
            foreach (var eq in equalStatements)
            {
                if (prevExpr == null || eq.LastExprType == ExpressionType.Or)
                {
                    _query.Append('[');
                    _view.Append("emit([");
                }
                if (prevExpr != null && eq.LastExprType == ExpressionType.Or)
                {
                    _view.Remove(_view.Length - 1, 1);
                    _view.Append("],null);");
                    _query.Remove(_query.Length - 1, 1);
                    _query.Append("],");
                }

                AppendStatement(eq);
                prevExpr = eq;
            }
            _view.Remove(_view.Length - 1, 1);
            _view.Append("],null);");
            _query.Remove(_query.Length - 1, 1);
            _query.Append("]]");
        }
        else
        {
            _view.Append("emit(null, null);");
        }
    }


    private void AppendStatement(Statement statement)
    {
        if (IsMemberExpression(statement.Left, out string? memberName) && ContainsValue(statement.Right, out object? expressionValue))
        {
            AppendFormattedStatement(memberName, expressionValue);
        }
        else if (ContainsValue(statement.Left, out expressionValue) && IsMemberExpression(statement.Right, out memberName))
        {
            AppendFormattedStatement(memberName, expressionValue);
        }
    }

    private static bool IsNumeric(object? expressionValue) => expressionValue is int or long or double or float or decimal or short or byte or sbyte or ushort or uint or ulong or char or Enum;

    private void AppendFormattedStatement(string? memberName, object? expressionValue)
    {
        if (expressionValue is string)
        {
            _view.AppendFormat("doc.{0} == '{1}',", memberName, expressionValue);
            _query.AppendFormat("'{0}',", expressionValue);
        }
        else if (expressionValue is bool)
        {
            _view.AppendFormat("doc.{0} == {1},", memberName, expressionValue?.ToString()?.ToLower());
            _query.AppendFormat("{0},", expressionValue?.ToString()?.ToLower());
        }
        else if (IsNumeric(expressionValue))
        {
            _view.AppendFormat("doc.{0} == {1},", memberName, expressionValue);
            _query.AppendFormat("{0},", expressionValue);
        }
    }

    private static bool IsMemberExpression(Expression expression, out string? memberName)
    {
        memberName = null;
        if (expression is MemberExpression memberExpr)
        {
            memberName = memberExpr.Member.Name;
            return true;
        }
        return false;
    }

    private static bool ContainsValue(Expression? expression, out object? expressionValue)
    {
        expressionValue = null;

        if (expression is ConstantExpression constExpr)
        {
            expressionValue = new CouchDbVisitor<object>(null).Visit(constExpr);
            return true;
        }
        if (expression is MemberExpression memberExp)
        {
            expressionValue = new CouchDbVisitor<object>(null).Visit(memberExp);
            if (ContainsValue(memberExp?.Expression, out var value))
            {
                expressionValue = value;
                return true;
            }
        }

        return false;
    }
}