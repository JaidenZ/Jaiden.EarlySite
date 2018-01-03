using EarlySite.Core.Utils;
using EarlySite.SModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EarlySite.Web.Controllers
{
    public class AccountController : BaseController
    {

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Forget()
        {
            return View();
        }


        [HttpPost]
        public JsonResult LoginRequest(LoginRequest account)
        {
            Result loginresult = new Result()
            {
                Status = true,
                Message = "",
                StatusCode = ""
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

            if (loginresult.Status)
            {
                loginresult = VerificationAccount(account.LoginUsername, account.LoginSecurity);
            }

            if (loginresult.Status)
            {
                

                base.CurrentAccount = new Account() { NickName = "hello" };
            }


            return Json(loginresult);
        }

        /// <summary>
        /// 验证账户有效性
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Result VerificationAccount(string username, string password)
        {
            Result loginresult = new Result()
            {
                Status = true,
                Message = "登录成功",
                StatusCode = "100"
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