namespace EarlySite.Cache.CacheBase
{
    using System.Collections.Generic;
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


        /// <summary>
        /// 获取手机用户的分享关系集合
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        IList<RelationShareInfo> GetRelationShareByPhone(long phone);
        

    }
}
