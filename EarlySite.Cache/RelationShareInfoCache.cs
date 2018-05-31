namespace EarlySite.Cache
{
    using CacheBase;
    using EarlySite.Model.Database;
    using System;
    using System.Collections.Generic;

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
        /// <summary>
        /// 有效时间
        /// </summary>
        public const int EffectiveTime = 15;


        /// <summary>
        /// 失效时间
        /// </summary>
        public static DateTime ExpireTime { get { return DateTime.Now.AddDays(EffectiveTime); } }

        /// <summary>
        /// 加载缓存
        /// </summary>
        void ICache<RelationShareInfo>.LoadCache()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 通过key移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ICache<RelationShareInfo>.RemoveInfo(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key can not be null");
            }
            bool issuccess = false;
            if (Session.Current.Contains(key))
            {
                issuccess = Session.Current.Remove(key);
            }
            return issuccess;
        }

        bool ICache<RelationShareInfo>.RemoveInfo(RelationShareInfo param)
        {
            if(param == null)
            {
                throw new ArgumentNullException("RelationShare info can not be null");
            }

            string key = param.GetKeyName();
            bool issuccess = Session.Current.Remove(key);
            return issuccess;
        }

        bool ICache<RelationShareInfo>.SaveInfo(RelationShareInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("RelationShare info can not be null");
            }
            string key = param.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Set(key, param);
            Session.Current.Expire(key, ExpireTime);
            return issuccess;
        }

        RelationShareInfo ICache<RelationShareInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("search key can not be null");
            }

            RelationShareInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<RelationShareInfo>(keys[0]);
            }
            return result;
        }
    }
}
