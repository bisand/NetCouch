using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Biseth.Net.Settee.Linq.Old
{
    internal static class TypeSystem
    {
        private static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof (string))
                return null;
            if (seqType.IsArray)
                return typeof (IEnumerable<>).MakeGenericType(seqType.GetElementType());
            if (seqType.IsGenericType)
            {
                foreach (var arg in seqType.GetGenericArguments())
                {
                    var ienum = typeof (IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }
            var ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (var iface in ifaces)
                {
                    var ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof (object))
            {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }

        internal static Type GetSequenceType(Type elementType)
        {
            return typeof (IEnumerable<>).MakeGenericType(elementType);
        }

        internal static Type GetElementType(Type seqType)
        {
            var ienum = FindIEnumerable(seqType);
            return ienum == null ? seqType : ienum.GetGenericArguments().FirstOrDefault();
        }

        internal static bool IsNullableType(Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        internal static bool IsNullAssignable(Type type)
        {
            return !type.IsValueType || IsNullableType(type);
        }

        internal static Type GetNonNullableType(Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        internal static Type GetMemberType(MemberInfo mi)
        {
            var fi = mi as FieldInfo;
            if (fi != null) return fi.FieldType;
            var pi = mi as PropertyInfo;
            if (pi != null) return pi.PropertyType;
            var ei = mi as EventInfo;
            if (ei != null) return ei.EventHandlerType;
            return null;
        }
    }
}