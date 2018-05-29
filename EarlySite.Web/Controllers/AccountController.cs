namespace EarlySite.Web.Controllers
{
    using EarlySite.Core.Utils;
    using EarlySite.Model.Show;
    using EarlySite.Model.Common;
    using EarlySite.Business.IService;
    using EarlySite.Business.Constract;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.IO;
    using EarlySite.Core.Cryptography;
    using EarlySite.Core.DDD.Service;

    public class AccountController : Controller
    {

        public ActionResult Login()
        {

            //string path = AppDomain.CurrentDomain.BaseDirectory + "\\backCover.jpg";
            //FileStream fs = new FileStream(path, FileMode.Open);
            //byte[] array = new byte[fs.Length];
            //using (fs)
            //{
            //    int index = 0;
            //    do
            //    {
            //        index = fs.Read(array, 0, array.Length);
            //    } while (index > 0);
                
            //}
            //string base64 = Base64Engine.ToBase64String(array);


            return View();
        }

        public ActionResult Forget()
        {
            return View();
        }

        public ActionResult Regist()
        {
            return View();
        }

        public ActionResult ResetPassword(string account)
        {
            ViewBag.Account = account;
            return View();
        }

        /// <summary>
        /// 登录ajax请求
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginRequest(LoginRequest account)
        {
            Result<Account> loginresult = new Result<Account>()
            {
                Status = true,
                Message = "",
                StatusCode = "",
                Data = null
            };
            
            string lastdate = CookieUtils.Get("lastSubmit");
            if (string.IsNullOrEmpty(lastdate))
            {
                CookieUtils.SetCookie("lastSubmit",DateTime.Now.ToString());
            }
            else
            {
                DateTime now = DateTime.Now;
                CookieUtils.SetCookie("lastSubmit", now.ToString());
                double seconds = now.Subtract(Convert.ToDateTime(lastdate)).TotalMilliseconds;
                if (seconds < 1000 * 5)
                {
                    loginresult.Status = false;
                    loginresult.Message = "操作过于频繁,请稍后再试";
                    loginresult.StatusCode = "LG000";
                }
            }

            //数据验证
            if (loginresult.Status)
            {
                loginresult = VerificationAccount(account.LoginUsername, account.LoginSecurity);
            }

            //登录操作
            if (loginresult.Status)
            {
                loginresult = ServiceObjectContainer.Get<IAccountService>().SignIn(account.LoginUsername, account.LoginSecurity);
                if (loginresult.Status)
                {
                    HttpContext.Session["CurrentAccount"] = loginresult.Data.Phone;
                }
            }
            return Json(loginresult);
        }

        /// <summary>
        /// 登出ajax请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SignOutRequest(string phone)
        {
            Result result = new Result()
            {
                Status = false
            };
            string lastdate = CookieUtils.Get("lastSubmit");
            if (string.IsNullOrEmpty(lastdate))
            {
                CookieUtils.SetCookie("lastSubmit", DateTime.Now.ToString());
            }
            else
            {
                DateTime now = DateTime.Now;
                CookieUtils.SetCookie("lastSubmit", now.ToString());
                double seconds = now.Subtract(Convert.ToDateTime(lastdate)).TotalMilliseconds;
                if (seconds < 1000 * 5)
                {
                    result.Status = false;
                    result.Message = "操作过于频繁,请稍后再试";
                    result.StatusCode = "SO000";
                }
            }
            long signoutphone = 0;
            if(!long.TryParse(phone, out signoutphone))
            {
                result.Status = false;
                result.Message = "参数错误";
                result.StatusCode = "SO002";
            }

            result = ServiceObjectContainer.Get<IAccountService>().SignOut(signoutphone);

            return Json(result);
        }

        /// <summary>
        /// 注册ajax请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistRequest(RegistRequest regist)
        {
            Result registresult = new Result()
            {
                Status = true,
                Message = "账户注册成功,请到邮箱进行验证.",
                StatusCode = "RR100"
            };

            string lastdate = CookieUtils.Get("lastSubmit");
            if (string.IsNullOrEmpty(lastdate))
            {
                CookieUtils.SetCookie("lastSubmit", DateTime.Now.ToString());
            }
            else
            {
                DateTime now = DateTime.Now;
                CookieUtils.SetCookie("lastSubmit", now.ToString());
                double seconds = now.Subtract(Convert.ToDateTime(lastdate)).TotalMilliseconds;
                if (seconds < 1000 * 5)
                {
                    registresult.Status = false;
                    registresult.Message = "操作过于频繁,请稍后再试";
                    registresult.StatusCode = "RR000";
                }
            }

            if (registresult.Status)
            {
                IAccountService service = ServiceObjectContainer.Get<IAccountService>();
                Result<Account> accountresult = service.RegistInfo(regist);
                if (!accountresult.Status)
                {
                    registresult.Status = false;
                    registresult.Message = "注册账户失败,请稍后再试";
                    registresult.StatusCode = "RR001";
                }
                else
                {
                    service.SendRegistEmail(accountresult.Data);
                }
            }
            return Json(registresult);
        }

        /// <summary>
        /// 检查邮箱是否注册
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckMail(string mail)
        {
            Result registresult = new Result()
            {
                Status = true,
                Message = "邮箱已被注册",
                StatusCode = "CM000"
            };
            if (!string.IsNullOrEmpty(mail))
            {
                registresult.Status = ServiceObjectContainer.Get<IAccountService>().CheckMailRegisted(mail).Status;
            }
            return Json(registresult);
        }

        /// <summary>
        /// 检查手机是否注册
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckPhone(string phone)
        {
            Result registresult = new Result()
            {
                Status = true,
                Message = "手机已被注册",
                StatusCode = "CP000"
            };
            if (!string.IsNullOrEmpty(phone))
            {
                registresult.Status = ServiceObjectContainer.Get<IAccountService>().CheckPhoneRegisted(phone).Status;
            }
            return Json(registresult);
        }

        /// <summary>
        /// 忘记密码提交
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForgetSubmit(string mail)
        {
            Result result = new Result()
            {
                Status = false,
            };

            //生成验证码发送到邮箱 加入缓存
            result.Status = ServiceObjectContainer.Get<IAccountService>().SendForgetVerificationCode(mail).Status;
            return Json(result);
        }

        /// <summary>
        /// 验证忘记密码验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForgetVerivicationCode(string mail,string code)
        {
            Result result = new Result()
            {
                Status = false,
                Message = "验证码不正确",
                StatusCode = "FV000"
            };
            result = ServiceObjectContainer.Get<IAccountService>().VerificationForgetCode(mail, code);

            return Json(result);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ResetPassword(string mail,string code)
        {
            Result result = new Result()
            {
                Status = false,
                Message = "密码修改失败",
                StatusCode = "RP000"
            };
            
            result = ServiceObjectContainer.Get<IAccountService>().ResetPassword(mail, code, 0);
            
            return Json(result);
        }

        /// <summary>
        /// 账户注册认证
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">认证码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RequireRegist(string phone,string code)
        {
            string codesave = CookieUtils.Get(string.Format("code{0}",phone));
            if (!string.IsNullOrEmpty(codesave))
            {
                if(code == codesave)
                {
                    ServiceObjectContainer.Get<IAccountService>().RequireAccount(Int64.Parse(phone));
                }

            }

            return Redirect(Url.Action("Index", "Home"));
        }

        /// <summary>
        /// 验证账户有效性
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Result<Account> VerificationAccount(string username, string password)
        {
            Result<Account> loginresult = new Result<Account>()
            {
                Status = true,
                Message = "登录成功",
                StatusCode = "100",
                Data = null
            };

            if (string.IsNullOrEmpty(username))
            {
                loginresult.Status = false;
                loginresult.StatusCode = "LG101";
                loginresult.Message = "登录信息不能为空";
            }
            else
            {
                Int64 phone = 0;
                if (Int64.TryParse(username, out phone))
                {
                    if (username.Length != 11)
                    {
                        loginresult.Status = false;
                        loginresult.StatusCode = "LG102";
                        loginresult.Message = "请输入正确的手机号码";
                    }
                }
                else
                {
                    if (!username.Contains('@'))
                    {
                        loginresult.Status = false;
                        loginresult.StatusCode = "LG103";
                        loginresult.Message = "请输入正确的登录信息";
                    }
                }

            }


            if (string.IsNullOrEmpty(password))
            {
                loginresult.Status = false;
                loginresult.StatusCode = "LG111";
                loginresult.Message = "密码不能为空";
            }
            return loginresult;
        }


    }
}