namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;
    /// <summary>
    /// 单品信息缓存接口
    /// </summary>
    public interface IDishCache : ICache<DishInfo>
    {


        void Test();


    }
}
