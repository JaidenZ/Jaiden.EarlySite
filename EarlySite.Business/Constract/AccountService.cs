namespace EarlySite.Business.Constract
{
    using System;
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
            //DBConnectionManager.Instance.Writer.Insert()

            result.Data = account.Copy<Account>();

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
            //Todo:查询数据库


            AccountInfo account = new AccountInfo();
            account.NickName = "PandaTV_0000";
            account.Phone = 18502850589;
            account.CreatDate = DateTime.Now;
            account.Avator = ConstInfo.DefaultHeadBase64;
            account.BackCorver = ConstInfo.DefaultBackCover;
            account.Sex = Model.Enum.AccountSex.Male;
            account.BirthdayDate = DateTime.Parse("2000-01-01");
            account.Description = "描述为空";
            account.RequiredStatus = Model.Enum.AccountRequiredStatus.Required;
            account.Email = "haojun.zhao@icloud.com";
            result.Status = true;
            Account returnaccount = account.Copy<Account>();

            //保存到缓存
            AccountInfoCache.Instance.CurrentAccount = returnaccount;

            result.Data = returnaccount;
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

            //生成code码加入缓存 设置时效日期
            if (account != null)
            {
                byte[] phonebyte = Encoding.UTF8.GetBytes(account.Phone.ToString());
                string code = Base64Engine.ToBase64String(phonebyte);

                CookieUtils.SetCookie(string.Format("code{0}", account.Phone), code, DateTime.Now.AddHours(1));


                SendMailInfo sendinfo = new SendMailInfo();
                sendinfo.Content = "hello haojun.zhao";
                sendinfo.Title = "验证账户";


                VerifiedMail.Sender.AddSend(sendinfo, new List<string>() { "272665534@qq.com" });

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
                Message = "验证账户",
                StatusCode = "RA000"
            };

            //修改数据库账户状态 登录
            //保存到缓存
            AccountInfo accountinfo = new AccountInfo();
            accountinfo.NickName = "PandaTV_0000";
            accountinfo.Phone = 18502850589;
            accountinfo.CreatDate = DateTime.Now;
            accountinfo.Avator = ConstInfo.DefaultHeadBase64;
            accountinfo.BackCorver = ConstInfo.DefaultBackCover;
            accountinfo.Sex = Model.Enum.AccountSex.Male;
            accountinfo.BirthdayDate = DateTime.Parse("2000-01-01");

            result.Status = true;

            Account returnaccount = accountinfo.Copy<Account>();
            //保存到缓存
            AccountInfoCache.Instance.CurrentAccount = returnaccount;


            return result;
        }
    }
}
