namespace Jaiden.Redis
{
    using EarlySite.Core.Utils;
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RedisKeyNameAttribute : Attribute
    {
        public string KeyName
        {
            get;
            set;
        }

        public string Format(params string[] args)
        {
            if (ListUtils.IsNullOrEmpty<string>(args))
                return null;
            string value = this.KeyName;
            for (int i = 0; i < args.Length; i++)
                value += string.Format("-{0}", args[i]);
            return value;
        }

        public static string Format(RedisKeyNameAttribute attr, params string[] args)
        {
            if (attr == null)
                return null;
            return attr.Format(args);
        }

        public static RedisKeyNameAttribute GetAttribute<T>()
        {
            return AttributeUnit.GetAttribute<RedisKeyNameAttribute>(typeof(T));
        }
    }
}
