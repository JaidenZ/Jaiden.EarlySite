namespace EarlySite.Web.Controllers
{
    using Core.Utils;
    using EarlySite.Cache;
    using EarlySite.Cache.CacheBase;
    using EarlySite.Core.DDD.Service;
    using EarlySite.Model.Common;
    using EarlySite.Model.Database;
    using EarlySite.Model.Show;
    using System;
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.Title = "哪儿吃";
        }
        
        /// <summary>
        /// 当前登录账户
        /// </summary>
        protected Account CurrentAccount
        {
            get
            {

                Account account = null;

                if(HttpContext.Session["CurrentAccount"] != null)
                {
                    string phone = (string)HttpContext.Session["CurrentAccount"];
                    ICache<OnlineAccountInfo> service = ServiceObjectContainer.Get<ICache<OnlineAccountInfo>>();
                    OnlineAccountInfo accountinfo = service.SearchInfoByKey(string.Format("OnlineAI_{0}",phone));
                    if(accountinfo != null)
                    {
                        account = accountinfo.Copy<Account>();
                    }
                }

                return account;

                return new Account()
                {
                    NickName = "测试用户",
                    Phone = 18502850589,
                    Sex = Model.Enum.AccountSex.Male,
                    Email = "272665534@qq.com",
                    Birthday = DateTime.Now,
                    CreatTime = Convert.ToDateTime("1900-01-01"),
                    Avator = ConstInfo.DefaultHeadBase64,
                    BackCorver = ConstInfo.DefaultBackCover
                };
            }
        }
        

        /// <summary>
        /// 重写调用方法前操作
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //验证登录信息或其他
            if(CurrentAccount == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = Json(new
                    {
                        IsLogout = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    filterContext.Result = Redirect(Url.Action("Login", "Account"));
                }
            }



            base.OnActionExecuting(filterContext);
        }


        protected override void HandleUnknownAction(string actionName)
        {
            //Response.Redirect("/", true);
            this.View("Error").ExecuteResult(this.ControllerContext);
        }
        

        /// <summary>
        /// 重写异常
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;

            //日志收集
            LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(exception, "At Controller OnException"), LogType.ErrorLog);

            Result<Exception> exceptionmodel = new Result<Exception>()
            {
                Data = exception,
                Message = exception.Message,
                Status = false
            };
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = Json(exceptionmodel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                filterContext.Result = View("Error", exceptionmodel);
            }

            filterContext.ExceptionHandled = true;
            //base.OnException(filterContext);
        }
        
    }
}