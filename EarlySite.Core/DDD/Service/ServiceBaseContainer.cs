namespace EarlySite.Core.DDD.Service
{
    using System;

    sealed class ServiceBaseContainer : IoCContainerBase<IServiceBase>
    {
        private static ServiceBaseContainer SERVICE_CONTAINER = null;

        public static ServiceBaseContainer Current
        {
            get
            {
                if (SERVICE_CONTAINER == null)
                {
                    SERVICE_CONTAINER = new ServiceBaseContainer();
                }
                return SERVICE_CONTAINER;
            }
        }

        private ServiceBaseContainer()
        {
            
        }

        public static bool Invalid(Type clazz)
        {
            if (clazz == null || clazz.IsValueType)
            {
                return false;
            }
            return !typeof(IServiceBase).IsAssignableFrom(clazz);
        }
    }
}
