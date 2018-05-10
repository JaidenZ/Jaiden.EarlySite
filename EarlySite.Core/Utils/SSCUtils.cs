
namespace EarlySite.Core.Utils
{
    using System;
    using ssc;
    using System.Reflection;

    public static class SSCUtils
    {
        public static object Deserialize(this byte[] buffer, Type clazz)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static;
            Binder binder = Type.DefaultBinder;
            Type[] args = { typeof(byte[]) };
            MethodInfo met = clazz.GetMethod("Deserialize", flags, binder, args, null);
            if (met != null)
                return met.Invoke(null, new object[] { buffer });
            return null;
        }

        public static T Deserialize<T>(this byte[] buffer) where T : class, new()
        {
            ISerializable s = (new T()) as ISerializable;
            if (s != null)
                return (T)s.Deserialize(buffer);
            return null;
        }
    }
}
