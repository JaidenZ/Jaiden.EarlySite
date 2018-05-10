namespace Jaiden.Redis
{
    using EarlySite.Core.Component;
    using System.Reflection;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public abstract class RedisMqBaseContainer
    {
        private IDictionary<Type, object> g_pMq = new Dictionary<Type, object>();

        protected abstract bool IsSubclassOf(Type clazz);

        public void Load(Assembly assembly)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);
            foreach (Type clazz in assembly.GetExportedTypes())
            {
                if (this.IsSubclassOf(clazz))
                {
                    ConstructorInfo ctor = clazz.GetConstructor(Type.EmptyTypes);
                    if (ctor != null)
                        g_pMq.Add(clazz, ctor.Invoke(null));
                }
            }
        }

        public T Get<T>()
        {
            Type key = typeof(T);
            object value = null;
            if (g_pMq.TryGetValue(key, out value))
                return (T)value;
            return default(T);
        }

        protected virtual bool IsSubclassOf(Type children, Type parent)
        {
            do
            {
                if (children.GUID == parent.GUID)
                    return true;
                children = children.BaseType;
            } while (children != null && children != typeof(object));
            return false;
        }
    }
}
