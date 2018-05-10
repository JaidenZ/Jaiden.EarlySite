namespace EarlySite.Core.Serialization
{
    using EarlySite.Core.Utils;
    using System;
    using System.Reflection;
using System.Text;
    
    /// <summary>
    /// 高级序列化变换控制特性
    /// </summary>
    public sealed class AdvancedObjectMarshalAsAttribute : Attribute
    {
        /// <summary>
        /// 高级序列化变换控制特性
        /// </summary>
        public AdvancedObjectMarshalAsAttribute()
        {
            this.NotSerialized = false;
            this.ArrayDoubleLength = true;
            this.SizeConst = 0;
            this.BigEndianModel = false;
            this.TextEncoding = "gbk";
            this.NotNullFlags = false;
        }
        /// <summary>
        /// 序列化时采用大端模式
        /// </summary>
        public bool BigEndianModel
        {
            get;
            set;
        }
        /// <summary>
        /// 固定长度
        /// </summary>
        public int SizeConst
        {
            get;
            set;
        }
        /// <summary>
        /// 双字节数组长度
        /// </summary>
        public bool ArrayDoubleLength
        {
            get;
            set;
        }
        /// <summary>
        /// 文本字符串编码
        /// </summary>
        public string TextEncoding
        {
            get;
            set;
        }
        /// <summary>
        /// 不可序列化
        /// </summary>
        public bool NotSerialized
        {
            get;
            set;
        }
        /// <summary>
        /// 不含空标志
        /// </summary>
        public bool NotNullFlags
        {
            get;
            set;
        }
        /// <summary>
        /// 获取一个特性
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static AdvancedObjectMarshalAsAttribute GetAttribute(PropertyInfo property)
        {
            return AttributeUnit.GetAttribute<AdvancedObjectMarshalAsAttribute>(property);
        }
    }
}
