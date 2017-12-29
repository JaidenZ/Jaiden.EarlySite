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
            throw new System.NotImplementedException();
        }
    }
}
