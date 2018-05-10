namespace EarlySite.Core.Serialization
{
    using EarlySite.Core.Utils;
    using EarlySite.Core.ValueType;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// 高级对象格式序列化算法
    /// </summary>
    public sealed partial class AdvancedObjectFormatter
    {
        /// <summary>
        /// 获取一个有效的的字符串编码
        /// </summary>
        /// <param name="attr">转换的特性类型</param>
        /// <returns></returns>
        private static Encoding GetTextEncoding(AdvancedObjectMarshalAsAttribute attr)
        {
            if (attr == null)
                return Encoding.Default;
            return Encoding.GetEncoding(attr.TextEncoding);
        }

        /// <summary>
        /// 获取具体大小
        /// </summary>
        /// <param name="attr">特性</param>
        /// <param name="clazz">类型</param>
        /// <returns></returns>
        private static int GetSpecificSize(AdvancedObjectMarshalAsAttribute attr, Type clazz)
        {
            if (attr != null && attr.SizeConst > 0)
                return attr.SizeConst;
            return ValueTypeUtils.SizeBy(clazz);
        }

        /// <summary>
        /// 添加字符串到流中
        /// </summary>
        private static bool AddStringToStream(Type clazz, string value, AdvancedObjectMarshalAsAttribute attr, BinaryWriter bw)
        {
            if (TypeUtils.IsString(clazz))
            {
                if (value == null)
                {
                    if (attr == null || attr.ArrayDoubleLength)
                        bw.Write(AdvancedObjectFormatter.As(BitConverter.GetBytes((short)-1), attr));
                    else
                        bw.Write((sbyte)-1);
                }
                else
                {
                    byte[] buffer = AdvancedObjectFormatter.GetTextEncoding(attr).GetBytes(value);
                    int size = buffer.Length;
                    if (attr != null && attr.SizeConst > 0)
                        size = attr.SizeConst;
                    else
                        if (attr == null || attr.ArrayDoubleLength)
                            bw.Write(AdvancedObjectFormatter.As(BitConverter.GetBytes((short)size), attr));
                        else
                            bw.Write((sbyte)size);
                    bw.Write(buffer, 0, size);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 对二进制流进行变换
        /// </summary>
        /// <param name="buffer">可能被变换的二进制流</param>
        /// <param name="attr">变换二进制流的依据</param>
        /// <returns></returns>
        private static byte[] As(byte[] buffer, AdvancedObjectMarshalAsAttribute attr)
        {
            if (buffer == null || attr == null)
                return buffer;
            if (attr.BigEndianModel)
                Array.Reverse(buffer);
            return buffer;
        }

        /// <summary>
        /// 添加值到流中
        /// </summary>
        private static bool AddValueToStream(Type clazz, object value, AdvancedObjectMarshalAsAttribute attr, BinaryWriter bw)
        {
            int size = ValueTypeUtils.SizeBy(clazz);
            if (size > 0)
            {
                if (ValueTypeUtils.IsFloatType(clazz))
                    bw.Write(AdvancedObjectFormatter.As(ValueTypeUtils.BinaryBy(clazz, Convert.ToDouble(value)), attr));
                else
                {
                    if (ValueTypeUtils.IsDateTime(clazz))
                        value = Convert.ToDateTime(value).Ticks;
                    if (ValueTypeUtils.IsULong(clazz))
                        value = unchecked((long)(ulong)value);
                    if (ValueTypeUtils.IsIPAddress(clazz))
                        value = BitConverter.ToInt32(((IPAddress)value).GetAddressBytes(), 0);
                    bw.Write(AdvancedObjectFormatter.As(ValueTypeUtils.BinaryBy(clazz, Convert.ToInt64(value), size), attr));
                }
                return true;
            }
            else
            {
                if (AdvancedObjectFormatter.AddStringToStream(clazz, value as string, attr, bw))
                    return true;
                if (AdvancedObjectFormatter.AddListToStream(clazz, value as IList, attr, bw))
                    return true;
                if (AdvancedObjectFormatter.AddObjectToStream(value, clazz, bw, attr))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 添加列表到流中
        /// </summary>
        private static bool AddListToStream(Type clazz, IList value, AdvancedObjectMarshalAsAttribute attr, BinaryWriter bw)
        {
            Type element = TypeUtils.GetArrayElement(clazz);
            if (element != null)
            {
                if (value == null)
                {
                    if (attr == null || attr.ArrayDoubleLength)
                        bw.Write(AdvancedObjectFormatter.As(BitConverter.GetBytes((short)-1), attr));
                    else
                        bw.Write((sbyte)-1);
                }
                else
                {
                    int size = value.Count;
                    if (attr != null && attr.SizeConst > 0)
                        size = attr.SizeConst;
                    else
                        if (attr == null || attr.ArrayDoubleLength)
                            bw.Write(AdvancedObjectFormatter.As(BitConverter.GetBytes((short)size), attr));
                        else
                            bw.Write((sbyte)size);
                    if (TypeUtils.IsBuffer(clazz))
                        bw.Write((byte[])value);
                    else
                    {
                        bool basic = ValueTypeUtils.IsBasicType(element);
                        for (int i = 0; i < size; i++)
                        {
                            object graph = value[i];
                            if (basic)
                                AdvancedObjectFormatter.AddValueToStream(element, graph, attr, bw);
                            else
                                AdvancedObjectFormatter.AddObjectToStream(graph, element, bw, attr);
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加对象到流
        /// </summary>
        /// <param name="graph">被序列化的对象</param>
        /// <param name="clazz">类型</param>
        /// <param name="writer">二进制流写入器</param>
        private static bool AddObjectToStream(object graph, Type clazz, BinaryWriter bw, AdvancedObjectMarshalAsAttribute attr)
        {
            if (TypeUtils.GetArrayElement(clazz) == null)
            {
                if (attr == null || !attr.NotNullFlags)
                    bw.Write(graph == null);
                if(graph != null)
                    foreach (PropertyInfo pi in clazz.GetProperties())
                    {
                        attr = AdvancedObjectMarshalAsAttribute.GetAttribute(pi);
                        if (attr != null && attr.NotSerialized) // 如果不可序列化则迭代到下一个
                            continue;
                        object value = pi.GetValue(graph, null);
                        AdvancedObjectFormatter.AddValueToStream(pi.PropertyType, value, attr, bw);
                    }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 序列化对象到二进制流
        /// </summary>
        /// <param name="graph">序列化的对象</param>
        /// <returns></returns>
        public static byte[] Serialize(object graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");
            using (MemoryStream ms = new MemoryStream(8096))
            {
                AdvancedObjectFormatter.Serialize(graph, ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 序列化对象到二进制流
        /// </summary>
        /// <param name="graph">序列化的对象</param> 
        /// <param name="s">序列化对象存储的流</param>
        public static void Serialize(object graph, Stream s)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");
            if (s == null)
                throw new ArgumentNullException("s");
            if (!s.CanWrite)
                throw new ArgumentException("s");
            using (BinaryWriter bw = new BinaryWriter(s))
            {
                Type clazz = graph.GetType();
                AdvancedObjectFormatter.AddObjectToStream(graph, clazz, bw, null);
            }
        }
    }

    public sealed partial class AdvancedObjectFormatter
    {
        /// <summary>
        /// 反序列化二进制流到对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream s)
        {
            object obj = AdvancedObjectFormatter.Deserialize(s, typeof(T));
            if (obj == null)
                return default(T);
            return (T)obj;
        }

        /// <summary>
        /// 反序列化二进制流到对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object Deserialize(byte[] buffer, Type type)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return AdvancedObjectFormatter.Deserialize(ms, type);
            }
        }

        /// <summary>
        /// 反序列化二进制流到对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return AdvancedObjectFormatter.Deserialize<T>(ms);
            }
        }

        /// <summary>
        /// 反序列化二进制流到对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object Deserialize(Stream s, Type type)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (!s.CanRead)
                throw new IOException("s");
            if (type == null)
                throw new ArgumentNullException("type");
            if (s.Length <= 0)
                throw new ArgumentException("s");
            using (BinaryReader br = new BinaryReader(s))
                return AdvancedObjectFormatter.GetObjectFromStream(type, br, null);
        }

        /// <summary>
        /// 获取对象从二进制流中
        /// </summary>
        /// <param name="clazz">对象类型</param>
        /// <param name="reader">二进制流读入器</param>
        /// <returns></returns>
        private static object GetObjectFromStream(Type clazz, BinaryReader reader, AdvancedObjectMarshalAsAttribute attr)
        {
            if ((attr == null || !attr.NotNullFlags) && reader.ReadBoolean())
                return null;
            ConstructorInfo ctor = clazz.GetConstructor(Type.EmptyTypes);
            object obj = ctor.Invoke(null);
            foreach (PropertyInfo pi in clazz.GetProperties())
            {
                attr = AdvancedObjectMarshalAsAttribute.GetAttribute(pi);
                if (attr != null && attr.NotSerialized) // 如果不可序列化则迭代到下一个
                    continue;
                object value = AdvancedObjectFormatter.GetValueFromStream(pi.PropertyType, attr, reader);
                if (value != null)
                    pi.SetValue(obj, value, null);
            }
            return obj;
        }

        /// <summary>
        /// 把缓冲区变换成有效的值（如果无效则返回-1，用于表示nil。）
        /// </summary>
        /// <param name="buffer">缓冲区数据</param>
        /// <returns></returns>
        private static int As(AdvancedObjectMarshalAsAttribute attr, byte[] buffer)
        {
            if (ListUtils.IsNullOrEmpty<byte>(buffer))
                return -1;
            int size = buffer.Length;
            if (size <= 1)
            {
                if (size == byte.MaxValue)
                    return -1;
                else
                    return buffer[0];
            }
            if (attr != null && attr.BigEndianModel)
                size = (buffer[1] | buffer[0] << 8);
            else
                size = (buffer[0] | buffer[1] << 8);
            if (size == ushort.MaxValue)
                return -1;
            return (ushort)(size);
        }

        /// <summary>
        /// 获取具体大小
        /// </summary>
        /// <param name="attr">特性</param>
        /// <param name="reader">二进制读入器</param>
        /// <returns></returns>
        private static int GetSpecificSize(AdvancedObjectMarshalAsAttribute attr, BinaryReader reader)
        {
            int size = int.MinValue;
            if (attr != null && attr.SizeConst > 0)
                size = attr.SizeConst;
            else
            {
                if (attr == null || attr.ArrayDoubleLength)
                    size = AdvancedObjectFormatter.As(attr, reader.ReadBytes(sizeof(ushort)));
                else
                    size = AdvancedObjectFormatter.As(attr, reader.ReadBytes(sizeof(byte)));
            }
            return size;
        }

        /// <summary>
        /// 从二进制流中获取字符串
        /// </summary>
        /// <param name="clazz">值的类型</param>
        /// <param name="attr">二进制流转换的控制特性</param>
        /// <param name="reader">二进制流的读入器</param>
        /// <returns></returns>
        private static string GetStringFromStream(AdvancedObjectMarshalAsAttribute attr, BinaryReader reader)
        {
            int size = AdvancedObjectFormatter.GetSpecificSize(attr, reader);
            if (size < 0)
                return null;
            if (size == 0)
                return string.Empty;
            return AdvancedObjectFormatter.GetTextEncoding(attr).GetString(reader.ReadBytes(size));
        }

        /// <summary>
        /// 从二进制流中获取一个有效的值
        /// </summary>
        /// <param name="clazz">值的类型</param>
        /// <param name="attr">二进制流转换的控制特性</param>
        /// <param name="reader">二进制流的读入器</param>
        /// <returns></returns>
        private static object GetValueFromStream(Type clazz, AdvancedObjectMarshalAsAttribute attr, BinaryReader reader)
        {
            int size = ValueTypeUtils.SizeBy(clazz);
            if (size > 0)
                return ValueTypeUtils.LongTo(clazz, ValueTypeUtils.ToLong(clazz, AdvancedObjectFormatter.As(reader.ReadBytes(size), attr), size));
            else
            {
                if (TypeUtils.IsString(clazz))
                    return AdvancedObjectFormatter.GetStringFromStream(attr, reader);
                else if (TypeUtils.GetArrayElement(clazz) != null)
                    return AdvancedObjectFormatter.GetListFromStream(clazz, attr, reader);
                else
                    return AdvancedObjectFormatter.GetObjectFromStream(clazz, reader, attr);
            }
        }

        /// <summary>
        /// 创建成员列表
        /// </summary>
        /// <param name="size">成员数量</param>
        /// <param name="clazz">列表类型</param>
        /// <returns></returns>
        private static IList CreateElementList(Type clazz, int size)
        {
            Type element = TypeUtils.GetArrayElement(clazz);
            if (clazz.IsArray)
                return Array.CreateInstance(element, size);
            if (TypeUtils.IsList(clazz))
            {
                clazz = typeof(List<>).MakeGenericType(element);
                ConstructorInfo ctor = clazz.GetConstructor(new Type[] { typeof(int) });
                return (IList)ctor.Invoke(new object[] { size });
            }
            throw new ArgumentException("Type of array or list is not supported.");
        }

        /// <summary>
        /// 从二进制流中获取一个有效的列表
        /// </summary>
        /// <param name="clazz">列表的类型</param>
        /// <param name="attr">二进制流转换的控制特性</param>
        /// <param name="reader">二进制流的读入器</param>
        /// <returns></returns>
        private static IList GetListFromStream(Type clazz, AdvancedObjectMarshalAsAttribute attr, BinaryReader reader)
        {
            int size = AdvancedObjectFormatter.GetSpecificSize(attr, reader);
            if (size < 0)
                return null;
            IList buffer = AdvancedObjectFormatter.CreateElementList(clazz, size);
            if (size <= 0)
                return buffer;
            if (TypeUtils.IsBuffer(clazz))
                return reader.ReadBytes(size);
            Type element = TypeUtils.GetArrayElement(clazz);
            bool basic = ValueTypeUtils.IsBasicType(element);
            for (int i = 0; i < size; i++)
            {
                object value = null;
                if (basic)
                    value = AdvancedObjectFormatter.GetValueFromStream(element, attr, reader);
                else
                    value = AdvancedObjectFormatter.GetObjectFromStream(element, reader, attr);
                if (!buffer.IsFixedSize)
                    buffer.Add(value);
                else
                    buffer[i] = value;
            }
            return buffer;
        }
    }
}
