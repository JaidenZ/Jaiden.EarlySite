namespace EarlySite.Business.IService
{
    using Model.Show;
    using Model.Common;

    public interface IAccount : IService
    {
        /// <summary>
        /// 账户登录
        /// </summary>
        /// <param name="signInCode">登录码:手机或者邮箱</param>
        /// <param name="securityCode">安全码</param>
        /// <returns></returns>
        Result<Account> SignIn(string signInCode,string securityCode);

        /// <summary>
        /// 账户注销
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result<Account> SignOut(long phone);

        /// <summary>
        /// 账户注册
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result<Account> RegistInfo(Account account);

        /// <summary>
        /// 发送注册邮件
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result SendRegistEmail(Account account);

    }
}
