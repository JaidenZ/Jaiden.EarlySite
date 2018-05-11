namespace EarlySite.Business.Filter
{
    using System;
    using System.Reflection;
    using EarlySite.Core.DDD.Service;

    public class RecipesServiceFilter : ServiceFilter
    {
        public RecipesServiceFilter(IServiceBase service) : base(service)
        {

        }

        protected override object InvokeMember(IServiceBase service, MethodBase method, params object[] args)
        {
            try
            {
                return method.Invoke(null, args);
            }
            catch(Exception ex)
            {

            }
            return null;
            
        }
    }
}
