namespace EarlySite.Business.Constract
{
    using System;
    using Model.Show;
    using Model.Common;
    using Model.Database;
    using Core.Utils;
    using Cache;
    using IService;

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
        public Result<Account> RegistInfo(Account account)
        {
            //Todo:加入数据库信息

            throw new System.NotImplementedException();
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
            account.CreatTime = DateTime.Now;
            result.Status = true;
            //保存到缓存
            AccountInfoCache.Instance.CurrentAccount = account;

            result.Data = account.Copy<Account>();
            return result;
        }
        /// <summary>
        /// 发送注册邮件
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Result SendRegistEmail(Account account)
        {
            //生成code码加入缓存 设置时效日期

            //Todo 发送code码注册邮件

            throw new NotImplementedException();
        }
        /// <summary>
        /// 认证账户
        /// </summary>
        /// <param name="requireCode"></param>
        /// <returns></returns>
        public Result RequireAccount(string requireCode)
        {
            //拿到code对应的账户信息 修改数据库账户状态 登录

            return null;
        }
    }
}
