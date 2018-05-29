namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;
    /// <summary>
    /// 账户缓存接口
    /// </summary>
    public interface IAccountInfoCache : ICache<AccountInfo>
    {
        bool Test();

    }
}
