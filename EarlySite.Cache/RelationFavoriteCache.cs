namespace EarlySite.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using EarlySite.Cache.CacheBase;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;
    using EarlySite.Model.Database;
    using EarlySite.Model.Enum;

    /// <summary>
    /// 收藏关系信息缓存
    /// <!--Redis Key格式-->
    /// DB_FA_手机号_收藏编号_类型
    /// </summary>
    public partial class RelationFavoriteCache : IRelationFavoriteCache
    {
        /// <summary>
        /// 通过手机号获取收藏关系集合
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="favoritetype"></param>
        /// <returns></returns>
        public IList<FavoriteInfo> GetFavoriteByPhone(long phone, FavoriteTypeEnum favoritetype)
        {
            if(phone == 0)
            {
                throw new ArgumentOutOfRangeException("Favorite's phone can not be zero!");
            }

            IList<FavoriteInfo> result = new List<FavoriteInfo>();

            string key = string.Format("DB_FA_{0}_*_{1}", phone, favoritetype == FavoriteTypeEnum.所有类型 ? "*" : favoritetype.GetHashCode().ToString());
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                foreach (string k in keys)
                {
                    result.Add(Session.Current.Get<FavoriteInfo>(k));
                }
            }
            else
            {
                //从数据库拿取
                result = DBConnectionManager.Instance.Reader.Select<FavoriteInfo>(new FavoriteSelectSpefication(phone, favoritetype).Satifasy());
                if (result != null && result.Count > 0)
                {
                    foreach (FavoriteInfo item in result)
                    {
                        //同步到缓存
                        Session.Current.Set(item.GetKeyName(), item);
                        Session.Current.Expire(item.GetKeyName(), ExpireTime);
                    }
                }
            }
            return result;
        }
    }




    public partial class RelationFavoriteCache : IRelationFavoriteCache
    {
        /// <summary>
        /// 有效时间
        /// </summary>
        public const int EffectiveTime = 15;

        /// <summary>
        /// 失效时间
        /// </summary>
        public static DateTime ExpireTime { get { return DateTime.Now.AddDays(EffectiveTime); } }

        void ICache<FavoriteInfo>.LoadCache()
        {
            throw new NotImplementedException();
        }

        bool ICache<FavoriteInfo>.RemoveInfo(string key)
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

        bool ICache<FavoriteInfo>.RemoveInfo(FavoriteInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("Favorite info can not be null");
            }

            string key = param.GetKeyName();
            bool issuccess = Session.Current.Remove(key);
            return issuccess;
        }

        bool ICache<FavoriteInfo>.SaveInfo(FavoriteInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("Favorite info can not be null");
            }
            string key = param.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Set(key, param);
            Session.Current.Expire(key, ExpireTime);
            return issuccess;
        }

        FavoriteInfo ICache<FavoriteInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("search key can not be null");
            }

            FavoriteInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<FavoriteInfo>(keys[0]);
            }
            return result;
        }
    }
}
