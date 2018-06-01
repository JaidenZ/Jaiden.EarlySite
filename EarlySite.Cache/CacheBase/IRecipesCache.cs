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


        /// <summary>
        /// 设置食谱禁用启用状态
        /// </summary>
        /// <param name="recipesId"></param>
        /// <param name="enable">
        /// 启用true
        /// 禁用false
        /// </param>
        /// <returns></returns>
        bool SetRecipesEnable(int recipesId, bool enable);

        /// <summary>
        /// 设置食谱禁用(设置当前用户全部食谱)
        /// </summary>
        /// <param name="phone"></param>
        ///  <param name="enable"></param>
        void SetRecipesEnable(long phone, bool enable);
    }
}
