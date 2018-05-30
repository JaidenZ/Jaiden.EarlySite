namespace EarlySite.Cache.CacheBase
{
    using System.Collections.Generic;
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

        /// <summary>
        /// 根据手机号获食谱集
        /// </summary>
        /// <param name="recipesId"></param>
        /// <returns></returns>
        IList<RecipesInfo> GetRecipesInfoByPhone(long phone);

        /// <summary>
        /// 根据手机号获取喜爱的食谱集
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        IList<RecipesInfo> GetFavoriteRecipesByPhone(long phone);

    }
}
