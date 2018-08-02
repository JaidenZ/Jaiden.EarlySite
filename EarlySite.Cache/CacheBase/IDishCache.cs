namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;
    using System.Collections.Generic;

    /// <summary>
    /// 单品信息缓存接口
    /// </summary>
    public interface IDishCache : ICache<DishInfo>
    {

        /// <summary>
        /// 获取单个单品
        /// </summary>
        /// <param name="dishId">单品编号</param>
        /// <returns>单品信息</returns>
        DishInfo GetDishInfoById(int dishId);

        /// <summary>
        /// 获取店铺的单品集合
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        IList<DishInfo> GetDishInfoByShop(int shopId);

        /// <summary>
        /// 更新单品信息中门店名称
        /// </summary>
        /// <param name="shopid">门店编号</param>
        /// <param name="name">更新的门店名称</param>
        void UpdateDishInfoByChangeShopName(int shopid, string name);
    }
}
