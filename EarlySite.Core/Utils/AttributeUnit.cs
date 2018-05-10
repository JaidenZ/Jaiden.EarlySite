namespace EarlySite.Core.Utils
{
    using System;
    using System.Reflection;

    /// <summary>
    /// 特性工具单元
    /// </summary>
    public static class AttributeUnit
    {
        /// <summary>
        /// 获取特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="member">被检索的成员</param>
        /// <returns></returns>
        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            Attribute attr = AttributeUnit.GetAttribute(member, typeof(T));
            if (attr == null)
            {
                return null;
            }
            return (T)attr;
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="member">被检索的成员</param>
        /// <param name="attribute">特性类型</param>
        /// <returns></returns>
        public static Attribute GetAttribute(MemberInfo member, Type attribute)
        {
            if (attribute == null || member == null)
            {
                throw new ArgumentNullException();
            }
            if (!typeof(Attribute).IsAssignableFrom(attribute))
            {
                return null;
            }
            object[] attris = member.GetCustomAttributes(attribute, false);
            if (ListUtils.IsNullOrEmpty<object>(attris))
            {
                return null;
            }
            return (Attribute)attris[0];
        }
        
    }
}
