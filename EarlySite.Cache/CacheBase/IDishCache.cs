namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;
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


    }
}
