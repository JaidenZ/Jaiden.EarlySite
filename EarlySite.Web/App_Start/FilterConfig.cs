namespace EarlySite.Web
{
    using System.Web;
    using System.Web.Mvc;
    using EarlySite.Core.Utils.JsonExtension;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new JsonAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
