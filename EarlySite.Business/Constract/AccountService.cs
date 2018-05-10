namespace EarlySite.Business.Constract
{
    using System;
    using System.IO;
    using Model.Show;
    using Model.Common;
    using Model.Database;
    using Core.Utils;
    using Cache;
    using IService;
    using EarlySite.Core.Cryptography;
    using System.Text;
    using EarlySite.Core.MailSender;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;

    public class AccountService : IAccount
    {

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result<Account> SignOut(long phone)
        {
            Result<Account> result = new Result<Account>()
            {
                Status = false,
                Message = "登出失败,登出人员与当前账户不匹配",
                StatusCode = "SO001"
            };

            if (AccountInfoCache.Instance.CurrentAccount != null && AccountInfoCache.Instance.CurrentAccount.Phone == phone)
            {
                result.Status = true;
                //Todo:记录数据库
                

                AccountInfoCache.Instance.CurrentAccount = null;
                result.Status = true;
                result.Message = "登出成功";
                result.StatusCode = "SO101";
            }

            return result;
        }
        /// <summary>
        /// 注册账户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Result<Account> RegistInfo(RegistRequest request)
        {
            Result<Account> result = new Result<Account>()
            {
                Status = true,
                Message = "注册账户成功",
                Data = null,
                StatusCode = "RR000"

            };

            AccountInfo account = new AccountInfo();
            account.Phone = Int64.Parse(request.Phone);
            account.Email = request.Email;
            account.SecurityCode = MD5Engine.ToMD5String(request.SecurityCode);
            account.CreatDate = DateTime.Now;
            account.BirthdayDate = DateTime.Parse("2000-01-01");
            account.Avator = ConstInfo.DefaultHeadBase64;
            account.BackCorver = ConstInfo.DefaultBackCover;
            account.Sex = Model.Enum.AccountSex.Male;
            account.RequiredStatus = Model.Enum.AccountRequiredStatus.UnRequired;
            account.Description = "";
            account.NickName = request.Phone;
            

            //加入数据库
            try
            {
                result.Status = DBConnectionManager.Instance.Writer.Insert(new AccountAddSpefication(account).Satifasy());
                DBConnectionManager.Instance.Writer.Commit();

                result.Data = account.Copy<Account>();
            }
            catch(Exception ex)
            {
                DBConnectionManager.Instance.Writer.Rollback();
                result.Status = false;
                result.Message = ex.Message;
                result.StatusCode = "EX000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:RegistInfo() .AccountService"), LogType.ErrorLog);
            }
            

            return result;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="signInCode"></param>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public Result<Account> SignIn(string signInCode, string securityCode)
        {
            Result<Account> result = new Result<Account>();
            try
            {
                string securityCodeMD5 = MD5Engine.ToMD5String(securityCode);

                IList<AccountInfo> inforesult = DBConnectionManager.Instance.Reader.Select<AccountInfo>(new AccountSelectSpefication(3 ,signInCode, securityCodeMD5).Satifasy());
                if (inforesult != null && inforesult.Count > 0)
                {
                    result.Status = true;
                    result.Data = inforesult[0].Copy<Account>();
                    
                    //保存到缓存
                    AccountInfoCache.Instance.CurrentAccount = result.Data;
                }
                else
                {
                    result.Message = "用户名或密码错误";
                    result.StatusCode = "LG000";
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Data = null;
                result.Message = ex.Message;
                result.StatusCode = "EX000";

                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:SignIn() .AccountService"), LogType.ErrorLog);

            }
            return result;
        }
        /// <summary>
        /// 发送注册邮件
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Result SendRegistEmail(Account account)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "发送邮件成功",
                StatusCode = "SR000"
            };
            
            try
            {
                //生成code码加入缓存 设置时效日期
                if (account != null)
                {
                    byte[] phonebyte = Encoding.UTF8.GetBytes(account.Phone.ToString());
                    string code = Base64Engine.ToBase64String(phonebyte);

                    CookieUtils.SetCookie(string.Format("code{0}", account.Phone), code, DateTime.Now.AddHours(1));


                    SendMailInfo sendinfo = new SendMailInfo();

                    using (StreamReader sr = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "VerificationMail.html"))
                    {
                        sendinfo.Content = sr.ReadToEnd();
                    }
                    sendinfo.Title = "验证账户";
                    if (!string.IsNullOrEmpty(sendinfo.Content))
                    {
                        sendinfo.Content = sendinfo.Content.Replace("(手机)", account.Phone.ToString());
                        sendinfo.Content = sendinfo.Content.Replace("(邮箱)", account.Email);
                        sendinfo.Content = sendinfo.Content.Replace("(验证码)", code);
                    }

                    VerifiedMail.Sender.AddSend(sendinfo, new List<string>() { account.Email });
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = string.Format("邮件验证出错 /r/n{0}", ex.Message);
                result.StatusCode = "EX000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:SendRegistEmail() .AccountService"), LogType.ErrorLog);
            }

            return result;
        }
        /// <summary>
        /// 认证账户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result RequireAccount(long phone)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "验证账户成功",
                StatusCode = "RA000"
            };

            try
            {
                //更改数据库
                if(DBConnectionManager.Instance.Writer.Update(new AccountRequireSpefication(true, phone).Satifasy(), null))
                {
                    IList<AccountInfo> accountlist = DBConnectionManager.Instance.Reader.Select<AccountInfo>(new AccountSelectSpefication(0, phone.ToString()).Satifasy());
                    if(accountlist != null && accountlist.Count > 0)
                    {
                        Account returnaccount = accountlist[0].Copy<Account>();
                        AccountInfoCache.Instance.CurrentAccount = returnaccount;
                    }
                    DBConnectionManager.Instance.Writer.Commit();
                }
                else
                {
                    result.Status = false;
                    result.Message = "验证账户失败";
                    result.StatusCode = "RA001";
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = string.Format("验证账户出错/r/n {0}", ex.Message);
                result.StatusCode = "EX000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:RequireAccount() .AccountService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 检查邮箱是否被注册
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public Result CheckMailRegisted(string mail)
        {
            Result result = new Result()
            {
                Status = true,  
            };
            try
            {
                int resultcount = DBConnectionManager.Instance.Reader.Count(new AccountCheckSpefication(mail, 1).Satifasy());
                if(resultcount > 0)
                {
                    result.Status = false;
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "邮箱验证出错" + ex.Message;
                result.StatusCode = "CMR000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:CheckMailRegisted() .AccountService"), LogType.ErrorLog);
            }
            return result;
        }

        /// <summary>
        /// 检查手机是否被注册
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Result CheckPhoneRegisted(string phone)
        {
            Result result = new Result()
            {
                Status = true,
            };
            try
            {
                int resultcount = DBConnectionManager.Instance.Reader.Count(new AccountCheckSpefication(phone, 0).Satifasy());
                if (resultcount > 0)
                {
                    result.Status = false;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "手机验证出错" + ex.Message;
                result.StatusCode = "CMR000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:CheckPhoneRegisted() .AccountService"), LogType.ErrorLog);
            }
            return result;
        }

        /// <summary>
        /// 发送忘记密码验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public Result SendForgetVerificationCode(string mail)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "发送邮件成功",
                StatusCode = "SR000"
            };

            try
            {
                //生成code码加入缓存 设置时效日期
                if (!string.IsNullOrEmpty(mail))
                {
                    string code = VerificationUtils.GetVefication();

                    CookieUtils.SetCookie(string.Format("forget{0}", mail), code, DateTime.Now.AddMinutes(30));


                    SendMailInfo sendinfo = new SendMailInfo();

                    using (StreamReader sr = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "ForgetVerificationMail.html"))
                    {
                        sendinfo.Content = sr.ReadToEnd();
                    }
                    sendinfo.Title = string.Format("你此次重置密码的验证码是:{0}",code);
                    if (!string.IsNullOrEmpty(sendinfo.Content))
                    {
                        sendinfo.Content = sendinfo.Content.Replace("(手机)", mail);
                        sendinfo.Content = sendinfo.Content.Replace("(验证码)", code);
                    }

                    VerifiedMail.Sender.AddSend(sendinfo, new List<string>() { "272665534@qq.com" });
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = string.Format("忘记密码邮件验证出错 /r/n{0}", ex.Message);
                result.StatusCode = "EX000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:SendForgetVerificationCode() .AccountService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 验证忘记密码提交的验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result VerificationForgetCode(string mail, string code)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "验证码正确",
                StatusCode = "VF001"
            };

            if (!string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(code))
            {
                string vcode = CookieUtils.Get(string.Format("forget{0}", mail));
                if (vcode != code)
                {
                    result.Status = false;
                    result.Message = "验证码错误";
                    result.StatusCode = "VF000";
                }
            }

            return result;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="securityCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Result ResetPassword(string account, string securityCode, int type)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "重置密码成功",
                StatusCode = "RP001"
            };
            try
            {
                //加密
                string code = MD5Engine.ToMD5String(securityCode);
                result.Status = DBConnectionManager.Instance.Writer.Update(new AccountResetPassSpefication(account, code, 0).Satifasy());
                DBConnectionManager.Instance.Writer.Commit();
                if (!result.Status)
                {
                    result.Message = "重置密码失败";
                }
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "修改密码出错" + ex.Message;
                result.StatusCode = "EX000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:ResetPassword() .AccountService"), LogType.ErrorLog);
            }
            return result;
        }

        /// <summary>
        /// 更新账户信息 (不包括背景图像和头像)
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Result UpdateAccountInfo(Account account)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "更新账户信息成功",
                StatusCode = "UA001"
            };

            try
            {
                AccountInfo info = account.Copy<AccountInfo>();
                result.Status = DBConnectionManager.Instance.Writer.Update(new AccountUpdateInfoSpefication(info).Satifasy());
            }
            catch(Exception ex)
            {
                result.Status = false;
                result.Message = "修改账户信息出错" + ex.Message;
                result.StatusCode = "UA000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:UpdateAccountInfo() .AccountService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 更改背景图
        /// </summary>
        /// <param name="backCoverbase64str"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Result UpdateBackCover(string backCoverbase64str, string account)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "更改背景图成功",
                StatusCode = "UB001"
            };
            try
            {
                result.Status = DBConnectionManager.Instance.Writer.Update(new AccountUpdateImageSpefication(backCoverbase64str,account,1).Satifasy());
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "修改背景图出错" + ex.Message;
                result.StatusCode = "UB000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:UpdateBackCover() .AccountService"), LogType.ErrorLog);
            }

            return result;
        }

        /// <summary>
        /// 更改头像
        /// </summary>
        /// <param name="headBase64str"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Result UpdateHead(string headBase64str, string account)
        {
            Result result = new Result()
            {
                Status = true,
                Message = "更改头像成功",
                StatusCode = "UH001"
            };
            try
            {
                result.Status = DBConnectionManager.Instance.Writer.Update(new AccountUpdateImageSpefication(headBase64str, account, 0).Satifasy());
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "修改头像出错" + ex.Message;
                result.StatusCode = "UH000";
                LoggerUtils.LogIn(LoggerUtils.ColectExceptionMessage(ex, "At service:UpdateHead() .AccountService"), LogType.ErrorLog);
            }
            return result;
        }
    }
}
