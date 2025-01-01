using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace NetCouch.Linq;

public class CouchDbViewQueryBuilder<T>
{
    private readonly StringBuilder _query;
    private readonly CouchDbTranslation _translation;
    private readonly StringBuilder _view;
    private readonly Type _type;

    private const string DocTypeCheck = "function(doc) { if (doc.doc_type_ && doc.doc_type_ == '";

    public CouchDbViewQueryBuilder(CouchDbTranslation translation)
    {
        _translation = translation ?? throw new ArgumentNullException(nameof(translation));
        _query = new StringBuilder();
        _view = new StringBuilder();
        _view.Append(DocTypeCheck);
        _type = typeof(T);
    }

    public ViewAndQuery Build()
    {
        var notEqualStatements = _translation.Statements.Where(x => x.NodeType == ExpressionType.NotEqual).ToList();
        var equalStatements = _translation.Statements.Where(x => x.NodeType == ExpressionType.Equal).ToList();
        var equalGroups = equalStatements.Select(x => x.Level).GroupBy(g => g).ToList();

        _view.Append(_translation.DesignDocName);
        _view.Append("') { ");

        AppendNotEqualStatements(notEqualStatements);
        AppendEqualStatements(equalStatements);

        if (notEqualStatements.Count > 0)
            _view.Append(" } ");

        _view.Append(" } }");

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
            _view.Append("emit(doc._id, 1);");
        }
    }


    private void AppendStatement(Statement statement)
    {
        if (IsMemberExpression(statement.Left, out string? memberName) && ContainsValue(statement.Right, out Expression? expressionValue))
        {
            AppendFormattedStatement(memberName, expressionValue);
        }
        else if (ContainsValue(statement.Left, out expressionValue) && IsMemberExpression(statement.Right, out memberName))
        {
            AppendFormattedStatement(memberName, expressionValue);
        }
    }

    static bool ImplementsINumber(Type type)
    {
        return type.GetInterfaces()
                   .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>));
    }

    private void AppendFormattedStatement(string? memberName, Expression? expressionValue)
    {
        string jsonPropertyName = GetJsonPropertyName(memberName);

        if (expressionValue?.Type == typeof(string))
        {
            _view.Append($"doc.{jsonPropertyName},");
            _query.Append($"{expressionValue},");
        }
        else if (expressionValue?.Type == typeof(bool))
        {
            string boolValue = expressionValue?.ToString()?.ToLower() ?? "false";
            _view.Append($"doc.{jsonPropertyName},");
            _query.Append($"{boolValue},");
        }
        else if (expressionValue?.Type != null && ImplementsINumber(expressionValue.Type))
        {
            _view.Append($"doc.{jsonPropertyName},");
            _query.Append($"{expressionValue},");
        }
    }

    private string GetJsonPropertyName(string? memberName)
    {
        if (string.IsNullOrEmpty(memberName))
            return string.Empty;

        var property = _type.GetProperty(memberName);
        if (property != null)
        {
            var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonPropertyAttribute != null)
            {
                return jsonPropertyAttribute.Name;
            }
        }

        // Fallback to camel case if no attribute is found
        return ToCamelCase(memberName);
    }

    private string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            return str;

        char[] chars = str.ToCharArray();
        chars[0] = char.ToLower(chars[0]);
        return new string(chars);
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

    private static bool ContainsValue(Expression? expression, out Expression? expressionValue)
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