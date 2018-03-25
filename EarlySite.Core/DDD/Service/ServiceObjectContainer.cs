namespace EarlySite.Core.DDD.Service
{
    using EarlySite.Core.AOP.Proxy;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public partial class ServiceObjectContainer
    {
        private static IDictionary<Type, object> services = new Dictionary<Type, object>();

        public static void Load(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException();
            ServiceBaseContainer.Current.Load(assembly);
            ServiceFilterContainer.Current.Load(assembly);
        }

        public static T Get<T>() where T : class
        {
            Type clazz = typeof(T);
            lock (services)
            {
                object value = null;
                if (!services.TryGetValue(clazz, out value))
                {
                    value = New(clazz);
                    if (value == null)
                        throw new ArgumentException();
                    services.Add(clazz, value);
                }
                return (T)value;
            }
        }

        private static object New(Type clazz)
        {
            object[] attrs = clazz.GetCustomAttributes(typeof(ServiceObjectAttribute), false);
            if (attrs.Length <= 0)
                return ServiceBaseContainer.Current.Get(clazz);
            ServiceObjectAttribute attr = (ServiceObjectAttribute)attrs[0];
            if (attr.ServiceFilter == null)
                throw new ArgumentException();
            object filter = ServiceFilterContainer.Current.Get(attr.ServiceFilter);
            if (filter == null)
                throw new ArgumentException();
            return InterfaceProxy.New(clazz, (InvocationHandler)filter);
        }
    }
}
