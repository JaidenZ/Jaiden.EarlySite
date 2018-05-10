namespace EarlySite.Business.IService
{
    using Model.Show;
    using Model.Common;
    using Core.DDD.Service;

    public interface IAccount : IServiceBase
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

        /// <summary>
        /// 验证忘记密码验证码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Result VerificationForgetCode(string mail, string code);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="securityCode">重置的密码</param>
        /// <param name="type">
        /// 重置的类型
        /// 0:邮箱
        /// 1:手机号
        /// </param>
        /// <returns></returns>
        Result ResetPassword(string account, string securityCode, int type);

        /// <summary>
        /// 根据手机号获取账户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Result<Account> GetAccountInfo(long phone);

        /// <summary>
        /// 修改账户资料(头像 背景图除外)
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result UpdateAccountInfo(Account account);

        /// <summary>
        /// 更新背景图片
        /// </summary>
        /// <param name="BackCoverbase64str"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        Result UpdateBackCover(string backCoverbase64str, string account);

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="headBase64str"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        Result UpdateHead(string headBase64str, string account);
    }

}
