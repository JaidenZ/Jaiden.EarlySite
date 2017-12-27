namespace EarlySite.Core.Utils.JsonExtension
{
    using System.Web.Mvc;

    public class JsonAttribute : ActionFilterAttribute
    {

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string contentType = filterContext.HttpContext.Response.ContentType;
            if (contentType == "application/json") filterContext.HttpContext.Response.ContentType = "text/html";
            base.OnResultExecuted(filterContext);
        }

    }
}
