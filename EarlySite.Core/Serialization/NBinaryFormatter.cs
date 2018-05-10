#pragma warning disable 0675

namespace EarlySite.Core.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public unsafe static partial class NBinaryFormatter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IList<Type> ValueType = new Type[] { typeof(double), typeof(float), typeof(long), typeof(ulong), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(char), typeof(int), typeof(uint), typeof(bool), typeof(DateTime) };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IList<byte> ValueSize = new byte[] { 8, 4, 8, 8, 1, 1, 2, 2, 2, 4, 4, 1, 8 };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Encoding Encoding = Encoding.Default;
    }
    /// <summary>
    /// 终端专用
    /// </summary>
    public unsafe static partial class NBinaryFormatter
    {
        private static byte[] GetBytes(short value)
        {
            return NBinaryFormatter.GetBytes(unchecked((ushort)value));
        }

        private static byte[] GetBytes(ushort value)
        {
            byte[] buffer = new byte[sizeof(ushort)];
            buffer[0] = (byte)(value >> 8);
            buffer[1] = (byte)value;
            return buffer;
        }

        public static int GetSize(Type type)
        {
            int i = ValueType.IndexOf(type);
            if (i < 0)
            {
                return 0;
            }
            return ValueSize[i];
        }

        private static byte[] GetBytes(Type type, long value, int size = -1)
        {
            if (size < 0)
            {
                size = NBinaryFormatter.GetSize(type);
            }
            byte[] buffer = new byte[size];
            unchecked
            {
                for (int i = 0; i < size; i++)
                {
                    buffer[i] = (byte)((value >> i * 8) & 0xFF);
                }
            }
            Array.Reverse(buffer);
            return buffer;
        }

        private static byte[] GetBytes(Type type, string value, bool single)
        {
            if (type == typeof(string))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter sw = new BinaryWriter(ms))
                    {
                        if (value == null)
                        {
                            if (single)
                            {
                                sw.Write((sbyte)-1);
                            }
                            else
                            {
                                sw.Write(NBinaryFormatter.GetBytes(-1));
                            }
                        }
                        else
                        {
                            byte[] buffer = NBinaryFormatter.Encoding.GetBytes(value);
                            if (single)
                            {
                                sw.Write((byte)buffer.Length);
                            }
                            else
                            {
                                sw.Write(NBinaryFormatter.GetBytes((short)buffer.Length));
                            }
                            sw.Write(buffer);
                        }
                        return ms.ToArray();
                    }
                }
            }
            return new byte[0];
        }

        private static byte[] GetBytes(Type type, IList value, bool single)
        {
            Type clazz = GetArrayElement(type);
            if (clazz != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter sw = new BinaryWriter(ms))
                    {
                        if (value == null)
                        {
                            if (single)
                            {
                                sw.Write((sbyte)-1);
                            }
                            else
                            {
                                sw.Write(NBinaryFormatter.GetBytes(-1));
                            }
                        }
                        else
                        {
                            if (single)
                            {
                                sw.Write((byte)value.Count);
                            }
                            else
                            {
                                sw.Write(NBinaryFormatter.GetBytes((short)value.Count));
                            }
                            if (type == typeof(byte[]))
                            {
                                sw.Write(value as byte[]);
                            }
                            else
                            {
                                for (int i = 0; i < value.Count; i++)
                                {
                                    object key = value[i];
                                    //
                                    byte[] buffer = NBinaryFormatter.ToBytes(clazz, key, single);
                                    sw.Write(buffer);
                                    //
                                    buffer = NBinaryFormatter.GetBytes(clazz, key, single);
                                    sw.Write(buffer);
                                }
                            }
                        }
                        return ms.ToArray();
                    }
                }
            }
            return new byte[0];
        }

        private static byte[] GetBytes(Type type, object value, bool single)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter sw = new BinaryWriter(ms))
                {
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        object obj = prop.GetValue(value, null);
                        //
                        sw.Write(ToBytes(prop.PropertyType, obj, single));
                    }
                }
                return ms.ToArray();
            }
        }

        private static byte[] GetBytes(Type type, object value, int size)
        {
            if (type == typeof(float))
            {
                byte[] buffer = BitConverter.GetBytes(Convert.ToSingle(value));
                Array.Reverse(buffer);
                return buffer;
            }
            if (type == typeof(double))
            {
                byte[] buffer = BitConverter.GetBytes(Convert.ToDouble(value));
                Array.Reverse(buffer);
                return buffer;
            }
            return new byte[0];
        }

        private static byte[] ToBytes(Type clazz, object value, bool single)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter sw = new BinaryWriter(ms))
                {
                    int size = NBinaryFormatter.GetSize(clazz);
                    //
                    if (size > 0)
                    {
                        if (clazz == typeof(double) || clazz == typeof(float))
                        {
                            sw.Write(NBinaryFormatter.GetBytes(clazz, value, size));
                        }
                        else
                        {
                            if (clazz == typeof(DateTime))
                            {
                                value = Convert.ToDateTime(value).Ticks;
                            }
                            if (clazz == typeof(ulong))
                            {
                                byte[] buffer = BitConverter.GetBytes(Convert.ToUInt64(value));
                                Array.Reverse(buffer);
                                //
                                sw.Write(buffer);
                            }
                            else
                            {
                                sw.Write(NBinaryFormatter.GetBytes(clazz, Convert.ToInt64(value), size));
                            }
                        }
                    }
                    else
                    {
                        sw.Write(NBinaryFormatter.GetBytes(clazz, value != null ? Convert.ToString(value) : null, single));
                        sw.Write(NBinaryFormatter.GetBytes(clazz, value as IList, single));
                    }
                }
                return ms.ToArray();
            }
        }
    }

    public static partial class NBinaryFormatter
    {
        private static Type GetArrayElement(Type array)
        {
            if (array.IsArray)
            {
                return array.GetElementType();
            }
            if (array.IsGenericType && (typeof(IList<>).GUID == array.GUID || typeof(List<>).GUID == array.GUID))
            {
                Type[] args = array.GetGenericArguments();
                return args[0];
            }
            return null;
        }

        private static IList CreateArrayCollection(Type array, Type element, int len)
        {
            if (array.IsArray)
            {
                return Array.CreateInstance(element, len);
            }
            if (array.IsGenericType && (typeof(IList<>).GUID == array.GUID || typeof(List<>).GUID == array.GUID))
            {
                Type clazz = typeof(List<>).MakeGenericType(element);
                ConstructorInfo ctor = clazz.GetConstructor(new Type[] { typeof(int) });
                //
                return (IList)ctor.Invoke(new object[] { len });
            }
            return null;
        }
    }

    public static partial class NBinaryFormatter
    {
        private partial class NBinaryReader : BinaryReader
        {
            public NBinaryReader(Stream s) : base(s) { }

            public override short ReadInt16()
            {
                return (short)(base.ReadByte() << 8 | base.ReadByte());
            }

            public override ushort ReadUInt16()
            {
                return unchecked((ushort)this.ReadInt16());
            }
        }

        private static object GetValue(Type type, long value)
        {
            unchecked
            {
                if (type == typeof(int))
                {
                    return (int)value;
                }
                if (type == typeof(uint))
                {
                    return Convert.ToUInt32(value);
                }
                if (type == typeof(long))
                {
                    return value;
                }
                if (type == typeof(ulong))
                {
                    return (ulong)value;
                }
                if (type == typeof(bool))
                {
                    return value > 0;
                }
                if (type == typeof(byte))
                {
                    return Convert.ToByte(value);
                }
                if (type == typeof(sbyte))
                {
                    return Convert.ToSByte(value);
                }
                if (type == typeof(short))
                {
                    return Convert.ToInt16(value);
                }
                if (type == typeof(ushort))
                {
                    return Convert.ToUInt16(value);
                }
                return null;
            }
        }

        private static string GetString(BinaryReader sr, bool single)
        {
            int len = single ? sr.ReadByte() : sr.ReadInt16();
            if (single && (sbyte)len == -1)
            {
                return null;
            }
            if (len == -1)
            {
                return null;
            }
            if (len <= 0)
            {
                return string.Empty;
            }
            return NBinaryFormatter.Encoding.GetString(sr.ReadBytes(len));
        }

        private static object GetList(Type array, BinaryReader sr, bool single)
        {
            Type element = NBinaryFormatter.GetArrayElement(array);
            if (element == null)
            {
                return null;
            }
            int len = single ? sr.ReadByte() : sr.ReadInt16();
            //
            if (single && (sbyte)len == -1)
            {
                return null;
            }
            if (len == -1)
            {
                return null;
            }
            if (len <= 0)
            {
                return NBinaryFormatter.CreateArrayCollection(array, element, 0);
            }
            if (array == typeof(byte[]))
            {
                return sr.ReadBytes(len);
            }
            IList buffer = NBinaryFormatter.CreateArrayCollection(array, element, len);
            for (int i = 0; i < len; i++)
            {
                object value = null;
                if (element.IsValueType || element == typeof(string))
                {
                    value = NBinaryFormatter.ToObject(element, sr, single);
                }
                else
                {
                    value = NBinaryFormatter.GetObject(element, sr, single);
                }
                if (buffer.IsFixedSize)
                {
                    buffer[i] = value;
                }
                else
                {
                    buffer.Add(value);
                }
            }
            return buffer;
        }

        private static object GetObject(Type type, byte[] buffer, int size = -1)
        {
            if (size < 0)
            {
                size = NBinaryFormatter.GetSize(type);
            }
            long value = 0;
            unchecked
            {
                Array.Reverse(buffer);
                for (int i = 0; i < size; i++)
                {
                    value |= ((long)buffer[i] & 0xFF) << (i * 8);
                }
            }
            return NBinaryFormatter.GetValue(type, value);
        }

        private static object GetObject(Type type, BinaryReader sr, bool single)
        {
            object key = Activator.CreateInstance(type);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                object value = NBinaryFormatter.ToObject(prop.PropertyType, sr, single);
                //
                prop.SetValue(key, value, null);
            }
            return key;
        }

        private static object GetValue(Type type, byte[] buffer)
        {
            if (type == typeof(double))
            {
                Array.Reverse(buffer);
                return BitConverter.ToDouble(buffer, 0);
            }
            if (type == typeof(float))
            {
                Array.Reverse(buffer);
                return BitConverter.ToSingle(buffer, 0);
            }
            return null;
        }

        private static object ToObject(Type clazz, BinaryReader sr, bool single)
        {
            int size = NBinaryFormatter.GetSize(clazz);
            //
            if (size > 0)
            {
                byte[] buffer = sr.ReadBytes(size);
                if (clazz == typeof(double) || clazz == typeof(float))
                {
                    return NBinaryFormatter.GetValue(clazz, buffer);
                }
                else
                {
                    if (clazz == typeof(DateTime))
                    {
                        object ticks = GetObject(typeof(long), buffer, size);
                        return new DateTime(Convert.ToInt64(ticks));
                    }
                    return NBinaryFormatter.GetObject(clazz, buffer, size);
                }
            }
            else
            {
                if (clazz == typeof(string))
                {
                    return NBinaryFormatter.GetString(sr, single);
                }
                else
                {
                    return NBinaryFormatter.GetList(clazz, sr, single);
                }
            }
        }
    }

    public static partial class NBinaryFormatter
    {
        public static byte[] Serialize(object graph, Type type, bool single)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return NBinaryFormatter.GetBytes(type, graph, single);
        }

        public static object Deserialize(byte[] buffer, Type type, bool single)
        {
            if (buffer == null || type == null)
            {
                throw new ArgumentNullException("stream or type");
            }
            using (Stream ms = new MemoryStream(buffer))
            {
                using (BinaryReader sr = new NBinaryReader(ms))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    //
                    return NBinaryFormatter.GetObject(type, sr, single);
                }
            }
        }

        public static T Deserialize<T>(byte[] buffer, bool single)
        {
            object value = NBinaryFormatter.Deserialize(buffer, typeof(T), single);
            if (value == null)
            {
                return default(T);
            }
            return (T)value;
        }

        public static byte[] Serialize(object graph, bool single)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            return NBinaryFormatter.Serialize(graph, graph.GetType(), single);
        }

        public static int SizeOf(object graph, bool single)
        {
            if (graph == null)
            {
                return 0;
            }
            return NBinaryFormatter.Serialize(graph, single).Length;
        }
    }

    public static partial class NBinaryFormatter
    {
        /// <summary>
        /// 解析附加信息集合 格式 id length msg
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="buffer">字节数组</param>
        /// <returns></returns>
        public static IList<T> Deserialize<T>(byte[] buffer)
        {
            Type type = typeof(T);
            if (buffer == null || type == null)
            {
                throw new ArgumentNullException("type");
            }

            PropertyInfo[] propertyinfoarry = type.GetProperties();
            if (propertyinfoarry[0].PropertyType != typeof(byte) || propertyinfoarry[1].PropertyType != typeof(byte) || propertyinfoarry[2].PropertyType != typeof(byte[]))
            {
                throw new ArgumentNullException("type error");
            }

            IList<T> listobj = new List<T>();
            int start = 0;
            for (int i = start; i < buffer.Length; i = start)
            {
                T key = (T)Activator.CreateInstance(type);
                ////id
                propertyinfoarry[0].SetValue(key, buffer[i], null);

                ////length
                start = buffer[i + 1] + i + 2;
                propertyinfoarry[1].SetValue(key, buffer[i + 1], null);

                ////msg
                int datastart = i + 2;
                byte[] tempbyte = new byte[buffer[i + 1]];
                for (int j = 0; j < tempbyte.Length; j++)
                {
                    tempbyte[j] = buffer[datastart + j];
                }

                Array.Reverse(tempbyte);
                //string tempmsg = string.Empty;
                //if (tempbyte.Length >= 4)
                //{
                //    tempmsg = BitConverter.ToInt32(tempbyte, 0).ToString();
                //}
                //else if (tempbyte.Length == 1)
                //{
                //    tempmsg = ((int)tempbyte[0]).ToString();
                //}
                //else
                //{
                //    tempmsg = BitConverter.ToInt16(tempbyte, 0).ToString();
                //}

                propertyinfoarry[2].SetValue(key, tempbyte, null);
                listobj.Add(key);
            }

            return listobj;
        }
        /// <summary>
        /// 解析终端参数应答
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static IList<T> DeserializeParam<T>(byte[] buffer)
        {
            Type type = typeof(T);
            if (buffer == null || type == null)
            {
                throw new ArgumentNullException("type");
            }

            PropertyInfo[] propertyinfoarry = type.GetProperties();
            if (propertyinfoarry[0].PropertyType != typeof(uint) || propertyinfoarry[1].PropertyType != typeof(byte) || propertyinfoarry[2].PropertyType != typeof(byte[]))
            {
                throw new ArgumentNullException("type error");
            }

            IList<T> listobj = new List<T>();
            int start = 0;
            for (int i = start; i < buffer.Length; i = start)
            {
                T key = (T)Activator.CreateInstance(type);
                ////id
                byte[] idBuffer = new byte[4];
                for (int id = 0; id < idBuffer.Length; id++)
                {
                    idBuffer[id] = buffer[i];
                    i++;
                }
                Array.Reverse(idBuffer);
                uint TerminalId = BitConverter.ToUInt32(idBuffer, 0);
                if (TerminalId == 0x90)
                {
                }
                if (TerminalId == 0x75)
                {
                }
                propertyinfoarry[0].SetValue(key, TerminalId, null);

                ////length
                start = buffer[i] + i + 1;
                propertyinfoarry[1].SetValue(key, buffer[i], null);

                ////msg
                int datastart = i + 1;
                byte[] tempbyte = new byte[buffer[i]];
                for (int j = 0; j < tempbyte.Length; j++)
                {
                    tempbyte[j] = buffer[datastart + j];
                }
                propertyinfoarry[2].SetValue(key, tempbyte, null);
                listobj.Add(key);
            }

            return listobj;
        }
    }
}