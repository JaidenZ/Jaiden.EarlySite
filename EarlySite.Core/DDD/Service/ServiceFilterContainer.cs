namespace EarlySite.Core.DDD.Service
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    sealed class ServiceFilterContainer : IoCContainerBase<ServiceFilter>
    {
        private static ServiceFilterContainer SERVICE_CONTAINER = null;

        public static ServiceFilterContainer Current
        {
            get
            {
                if (SERVICE_CONTAINER == null)
                {
                    SERVICE_CONTAINER = new ServiceFilterContainer();
                }
                return SERVICE_CONTAINER;
            }
        }

        private ServiceBaseContainer service = null;

        private ServiceFilterContainer()
        {
            service = ServiceBaseContainer.Current;
        }

        protected override object Resolve(Type clazz, bool alloc, params Type[] exceptive)
        {
            exceptive = new Type[] { typeof(IServiceBase) };
            return base.Resolve(clazz, alloc, exceptive);
        }

        protected override object Resolve(ConstructorInfo ctor, bool alloc, IDictionary<Type, object> exceptive)
        {
            exceptive = new Dictionary<Type, object>();
            foreach (var pi in ctor.GetParameters())
            {
                Type clazz = pi.ParameterType;
                object obj = service.Get(clazz);
                if (obj != null)
                {
                    exceptive.Add(clazz, obj);
                }
            }
            return base.Resolve(ctor, alloc, exceptive);
        }
    }
}
