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

    }
}
