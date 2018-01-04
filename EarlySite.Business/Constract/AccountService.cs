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


        public Result<Account> LogOut(int phone)
        {
            throw new System.NotImplementedException();
        }

        public Result<Account> RegistInfo(Account account)
        {
            throw new System.NotImplementedException();
        }

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
    }
}
