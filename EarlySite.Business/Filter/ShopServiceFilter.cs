namespace EarlySite.Business.Filter
{
    using System;
    using System.Reflection;
    using EarlySite.Business.IService;
    using EarlySite.Core.DDD.Service;

    public class ShopServiceFilter : ServiceFilter
    {
        ILoggerService _logger;

        public ShopServiceFilter(IShopService service , ILoggerService logger) : base(service)
        {
            _logger = logger;
        }

        protected override object InvokeMember(IServiceBase service, MethodBase method, params object[] args)
        {
            try
            {
                _logger.AddRunningLog("ShopService", method.Name);
                return method.Invoke(service, args);
            }
            catch (TargetInvocationException e)
            {
                Exception exception = e.InnerException;
                _logger.AddExceptionLog(exception, args);
            }
            return null;
        }
    }
}
