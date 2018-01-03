namespace EarlySite.Business.Constract
{
    using EarlySite.SModel;
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

            Account account = new Account();
            account.NickName = "Jaiden";
            account.Phone = 18502850589;

            result.Status = true;
            result.Data = account;
            return result;
        }
    }
}
