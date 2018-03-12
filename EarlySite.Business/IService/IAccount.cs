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
        Result<Account> RegistInfo(RegistRequest request);

        /// <summary>
        /// 发送注册邮件
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result SendRegistEmail(Account account);

        /// <summary>
        /// 认证账户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Result RequireAccount(long phone);

        /// <summary>
        /// 验证邮箱是否注册
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        Result CheckMailRegisted(string mail);
        
        /// <summary>
        /// 验证手机是否注册
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Result CheckPhoneRegisted(string phone);

        /// <summary>
        /// 发送忘记密码验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        Result SendForgetVerificationCode(string mail);

    }
}
