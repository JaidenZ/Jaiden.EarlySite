namespace EarlySite.Cache.CacheBase
{
    using EarlySite.Model.Database;

    /// <summary>
    /// 分享信息关系缓存接口
    /// </summary>
    public interface IRelationShareInfoCache: ICache<RelationShareInfo>
    {

        /// <summary>
        /// 解除用户的食谱关系
        /// </summary>
        /// <param name="recipesId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool RemoveRelationShareByRecipes(int recipesId, long phone);

        /// <summary>
        /// 解除用户的食谱关系
        /// </summary>
        /// <param name="recipesId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool RemoveRelationShareByPhone(long phone);
    }
}
