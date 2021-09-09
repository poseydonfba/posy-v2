using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Posy.V2.Infra.CrossCutting.Common.Extensions
{
    public static class Extensions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return Enum.GetName(typeof(T), e); /// could also return string.Empty
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
            }
        }

        /// <summary>
        /// Verifica se o model foi alterado para update
        /// </summary>
        /// <returns></returns>
        public static bool IsModified<T>(this T a1, T a2, params Expression<Func<T, Object>>[] props)
        {
            if (props == null)
                return a1.Equals(a2);

            foreach (Expression<Func<T, object>> memberExpression in props)
            {
                Expression body = memberExpression.Body;
                while (body.NodeType == ExpressionType.Convert || body.NodeType == ExpressionType.ConvertChecked)
                    body = ((UnaryExpression)body).Operand;

                //MemberExpression property = memberExpression.Body as MemberExpression;
                MemberExpression property = body as MemberExpression;

                if (property != null)
                {
                    foreach (PropertyInfo pi in typeof(T).GetProperties())
                    {
                        // exclude all properties we passed in
                        if (pi.Name.Equals(property.Member.Name))
                        {
                            var valueA1 = pi.GetValue(a1, null);
                            var valueA2 = pi.GetValue(a2, null);
                            if (valueA1 != null && valueA2 != null)
                                if (!valueA1.Equals(valueA2))
                                    return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks the equality of two arrays
        /// </summary>
        /// <returns>True if equals otherwise False</returns>
        public static bool ByteEquals(this byte[] array, byte[] other)
        {
            if (array == null && other == null) return true;
            if (array == null || other == null) return false;
            if (array.Length != other.Length) return false;
            return array.SequenceEqual(other);
        }
    }
}
