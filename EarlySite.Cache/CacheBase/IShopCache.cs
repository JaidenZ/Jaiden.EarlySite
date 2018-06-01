namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;

    /// <summary>
    /// 店铺缓存接口
    /// </summary>
    public interface IShopCache : ICache<ShopInfo>
    {
        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        ShopInfo GetshopInfoByShopId(int shopId);


        /// <summary>
        /// 设置门店是否启用
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        bool SetShopInfoEnable(int shopId, bool enable);

    }
}
