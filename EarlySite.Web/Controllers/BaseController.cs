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
    using System.Configuration;
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

//#if DEBUG
//                return new Account()
//                {
//                    NickName = "不爱我就拉倒",
//                    Phone = 18502850589,
//                    Sex = Model.Enum.AccountSex.Male.GetHashCode(),
//                    Email = "272665534@qq.com",
//                    BirthdayDate = DateTime.Now,
//                    CreatDate = Convert.ToDateTime("2018-03-08 15:47:05"),
//                    Avator = ConstInfo.DefaultHeadBase64,
//                    BackCorver = ConstInfo.DefaultBackCover
//                };
//#endif


                Account account = null;
                if (HttpContext == null)
                    return null;
                if (HttpContext.Session["CurrentAccount"] != null)
                {
                    string phone = HttpContext.Session["CurrentAccount"].ToString();
                    IOnlineAccountCache service = ServiceObjectContainer.Get<IOnlineAccountCache>();
                    OnlineAccountInfo accountinfo = service.SearchInfoByKey(string.Format("OnlineAI_{0}_*", phone));
                    if (accountinfo != null)
                    {
                        account = accountinfo.Copy<Account>();
                    }
                }

                return account;
                
            }
        }
        
        /// <summary>
        /// 当前用户定位坐标
        /// </summary>
        protected Position CurrentPosition
        {
            get
            {
                Position current = new Position();
                if (HttpContext == null)
                    return null;
                if (HttpContext.Session["Position"] != null)
                {
                    string positon = HttpContext.Session["Position"].ToString();
                    
                    current.Longitude =Convert.ToDouble(positon.Split(',')[0]);
                    current.Latitude = Convert.ToDouble(positon.Split(',')[1]);
                }
                return current;
            }
        }

        /// <summary>
        /// 搜索距离 单位M
        /// </summary>
        protected double SearchDistance
        {
            get
            {
                double dis = 0;
                string distance = ConfigurationManager.AppSettings["SearchDistance"];
                if (!string.IsNullOrEmpty(distance))
                {
                    dis = Convert.ToDouble(distance);
                }
                return dis;
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