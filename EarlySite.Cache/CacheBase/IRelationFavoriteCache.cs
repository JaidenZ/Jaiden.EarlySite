namespace EarlySite.Cache.CacheBase
{
    using System.Collections.Generic;
    using EarlySite.Model.Database;
    using EarlySite.Model.Enum;

    /// <summary>
    /// 收藏关系缓存接口
    /// </summary>
    public interface IRelationFavoriteCache : ICache<FavoriteInfo>
    {
        /// <summary>
        /// 根据手机获取收藏的关系集合
        /// </summary>
        /// <param name="phone">收藏关系者的手机号</param>
        /// <param name="favoritetype">收藏关系类型</param>
        /// <returns></returns>
        IList<FavoriteInfo> GetFavoriteByPhone(long phone, FavoriteTypeEnum favoritetype);

        

    }
}
