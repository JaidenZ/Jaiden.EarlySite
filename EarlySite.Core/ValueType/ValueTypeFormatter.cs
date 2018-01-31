namespace EarlySite.Core.ValueType
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// 用于对值类型数据的字符串与值类型数据相互格式化
    /// </summary>
    public static class ValueTypeFormatter
    {
        /// <summary>
        /// 字符串到值类型，如 oct“1234”-> dec 668 is type int?
        /// </summary>
        /// <param name="type">值类型</typeparam>
        /// <param name="value">欲被转换的字符串</param>
        /// <param name="style">数值转换使用的方式（如HEX,BIN,OCT,DEC）</param>
        /// <param name="provider">对特定数值转换使用“格式化提供商”</param>
        /// <returns></returns>
        public static object Parse(this string value, Type type, NumberStyles style = NumberStyles.None, IFormatProvider provider = null)
        {
            if (type == null)
            {
                return null;
            }
            if (type == typeof(string))
            {
                return value;
            }
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            //
            MethodInfo met = type.GetMethod("TryParse", new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider), type.MakeByRefType() });
            object[] args = null;
            if (met == null)
            {
                met = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });
                if (met == null)
                {
                    return null;
                }
                args = new object[] { value, null };
            }
            else
            {
                args = new object[] { value, style, provider, null };
            }
            if (args == null)
            {
                return null;
            }
            if ((true).Equals(met.Invoke(null, args)))
            {
                return args[args.Length - 1];
            }
            return null;
        }
        /// <summary>
        /// 字符串到值类型，如 oct“1234”-> dec 668 is type int?
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="value">欲被转换的字符串</param>
        /// <param name="style">数值转换使用的方式（如HEX,BIN,OCT,DEC）</param>
        /// <param name="provider">对特定数值转换使用“格式化提供商”</param>
        /// <returns></returns>
        public static T? Parse<T>(this string value, NumberStyles style = NumberStyles.None, IFormatProvider provider = null) where T : struct
        {
            object o = ValueTypeFormatter.Parse(value, typeof(T), style, provider);
            if (o == null)
            {
                return null;
            }
            return (T)o;
        }
        /// <summary>
        /// 将值类型数据转换成字符串
        /// </summary>
        /// <param name="value">值类型数据</param>
        /// <returns></returns>
        public static string To(this object value)
        {
            return Convert.ToString(value);
        }
        /// <summary>
        /// 将值类型数据转换成字符串
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="value">值类型数据</param>
        /// <returns></returns>
        public static string To<T>(this T value)
        {
            return Convert.ToString(value);
        }
        /// <summary>
        /// 字符串到值类型，如 oct“1234”-> dec 668 is type int
        /// </summary>
        /// <returns></returns>
        public static T TryParse<T>(this string value, NumberStyles style = NumberStyles.None, IFormatProvider provider = null) where T : struct
        {
            T? result = Parse<T>(value);
            if (result == null)
            {
                return default(T);
            }
            return (T)result;
        }


    }
}
