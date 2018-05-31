namespace EarlySite.Cache
{
    using CacheBase;
    using EarlySite.Model.Database;

    /// <summary>
    /// 分享关系信息缓存
    /// <!--Redis Key格式-->
    /// DB_RS_食谱编号_食物编号_手机号
    /// </summary>
    public partial class RelationShareInfoCache : IRelationShareInfoCache
    {

    }


    /// <summary>
    /// 分享关系信息缓存
    /// <!--Redis Key格式-->
    /// DB_RS_食谱编号_食物编号_手机号
    /// </summary>
    public partial class RelationShareInfoCache : IRelationShareInfoCache
    {
        void ICache<RelationShareInfo>.LoadCache()
        {
            throw new System.NotImplementedException();
        }

        bool ICache<RelationShareInfo>.RemoveInfo(string key)
        {
            throw new System.NotImplementedException();
        }

        bool ICache<RelationShareInfo>.RemoveInfo(RelationShareInfo param)
        {
            throw new System.NotImplementedException();
        }

        bool ICache<RelationShareInfo>.SaveInfo(RelationShareInfo param)
        {
            throw new System.NotImplementedException();
        }

        RelationShareInfo ICache<RelationShareInfo>.SearchInfoByKey(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
