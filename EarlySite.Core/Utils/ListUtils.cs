using System;
using System.Collections.Generic;
namespace EarlySite.Core.Utils
{
    using System.Collections;
    using System.Collections.Generic;

    public static class ListUtils
    {
        public static bool IsNullOrEmpty(this IList value)
        {
            return (value == null || value.Count <= 0);
        }

        public static bool IsNullOrEmpty<T>(this IList<T> value)
        {
            return (value == null || value.Count <= 0);
        }

        public static IList<string> Transform(this string value, params char[] separator)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (separator == null || separator.Length <= 0)
            {
                return new string[] { value };
            }
            return value.Split(separator: separator);
        }

        /// <summary>
        /// 字典转泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this IDictionary<string, T> dic) where T : class, new()
        {
            if (dic == null)
            {
                return null;
            }
            IList<T> result = new List<T>();
            foreach (KeyValuePair<string, T> item in dic)
            {
                result.Add(item.Value);
            }
            return result;
        }


    }
}
