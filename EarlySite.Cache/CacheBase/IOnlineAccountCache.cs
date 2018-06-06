namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;

    /// <summary>
    /// 在线账户缓存接口
    /// </summary>
    public interface IOnlineAccountCache : ICache<OnlineAccountInfo>
    {
        /// <summary>
        /// 更新在线用户缓存
        /// </summary>
        /// <param name="online"></param>
        void UpdateOnlineAccount(OnlineAccountInfo online);


    }
}
