namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;

    /// <summary>
    /// 食谱缓存接口
    /// </summary>
    public interface IRecipesCache:ICache<RecipesInfo>
    {
        /// <summary>
        /// 根据食谱编号获取缓存
        /// </summary>
        /// <param name="recipesId"></param>
        /// <returns></returns>
        RecipesInfo GetRecipesInfoById(int recipesId);


    }
}
