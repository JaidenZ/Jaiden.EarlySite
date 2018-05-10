namespace EarlySite.Core.Serialization
{
    using ssc;
    using EarlySite.Core.Utils;
    using System;
    using System.Text;

    public partial class StringFormatter
    {
        private static Encoding encoding = Encoding.GetEncoding("IBM037");

        public unsafe static string Serialize(object value)
        {
            ISerializable s = value as ISerializable;
            byte[] buffer = null;
            if (s != null)
                buffer = s.Serialize();
            else if (value is byte[])
                buffer = (byte[])value;
            else
                buffer = BinaryFormatter.Serialize(value, false);
            return encoding.GetString(buffer);
        }

        public unsafe static object Deserialize(string value, Type type)
        {
            byte[] buffer = encoding.GetBytes(value);
            if (type == typeof(byte[]))
                return buffer;
            object obj = buffer.Deserialize(type);
            if (obj != null)
                return obj;
            return BinaryFormatter.Deserialize(buffer, type, false);
        }

        public unsafe static T Deserialize<T>(string value) where T : class, new()
        {
            byte[] buffer = encoding.GetBytes(value);
            if (typeof(T) == typeof(byte[]))
                return (T)(object)buffer;
            T num = buffer.Deserialize<T>();
            if (num != null)
                return num;
            object obj = BinaryFormatter.Deserialize<T>(buffer, false);
            if (obj == null)
                return default(T);
            return (T)obj;
        }
    }
}
