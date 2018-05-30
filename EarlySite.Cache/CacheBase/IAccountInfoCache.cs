namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;
    /// <summary>
    /// 账户缓存接口
    /// </summary>
    public interface IAccountInfoCache : ICache<AccountInfo>
    {
        /// <summary>
        /// 检验手机号是否存在
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool CheckPhoneExists(string phone);

        /// <summary>
        /// 检验邮箱是否存在
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        bool CheckMailExists(string mail);

    }
}
