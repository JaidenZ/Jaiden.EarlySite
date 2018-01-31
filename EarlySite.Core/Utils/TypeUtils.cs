namespace EarlySite.Core.Utils
{
    using System;
    using System.Collections.Generic;
    public class TypeUtils
    {
        /// <summary>
        /// 获取数组的成员类型
        /// </summary>
        /// <param name="array">数组类型</param>
        /// <returns></returns>
        public static Type GetArrayElement(Type array)
        {
            if (array.IsArray)
            {
                return array.GetElementType();
            }
            if (TypeUtils.IsList(array))
            {
                Type[] args = array.GetGenericArguments();
                return args[0];
            }
            return null;
        }

        /// <summary>
        /// 是否缓冲区
        /// </summary>
        /// <returns></returns>
        public static bool IsBuffer(Type clazz)
        {
            return clazz == typeof(byte[]);
        }

        /// <summary>
        /// 是否字符串
        /// </summary>
        /// <returns></returns>
        public static bool IsString(Type clazz)
        {
            return clazz == typeof(string);
        }

        /// <summary>
        /// 是否列表
        /// </summary>
        /// <returns></returns>
        public static bool IsList(Type clazz)
        {
            if (clazz == null)
                return false;
            if (clazz.IsArray)
                return true;
            return clazz.IsGenericType && (typeof(IList<>).GUID == clazz.GUID || typeof(List<>).GUID == clazz.GUID);
        }
    }
}
