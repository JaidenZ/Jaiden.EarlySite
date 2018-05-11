namespace EarlySite.Business.Filter
{

    using System;
    using System.Reflection;
    using EarlySite.Core.DDD.Service;

    public class AccountServiceFilter : ServiceFilter
    {
        public AccountServiceFilter(IServiceBase service) : base(service)
        {
        }

        protected override object InvokeMember(IServiceBase service, MethodBase method, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
