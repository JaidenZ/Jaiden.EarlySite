namespace EarlySite.Cache
{
    using System;
    using Model.Database;
    using EarlySite.Cache.CacheBase;

    /// <summary>
    /// 食谱信息缓存
    /// <!--Redis Key格式-->
    /// DB_RI_食谱编号_手机号
    /// </summary>
    public partial class RecipesCache : IRecipesCache
    {

    }


    /// <summary>
    /// 食谱信息缓存
    /// <!--Redis Key格式-->
    /// DB_RI_食谱编号_手机号
    /// </summary>
    public partial class RecipesCache : IRecipesCache
    {
        /// <summary>
        /// 有效时间
        /// </summary>
        public const int EffectiveTime = 15;


        /// <summary>
        /// 失效时间
        /// </summary>
        public static DateTime ExpireTime { get { return DateTime.Now.AddDays(EffectiveTime); } }


        void ICache<RecipesInfo>.LoadCache()
        {
            throw new NotImplementedException();
        }

        bool ICache<RecipesInfo>.RemoveInfo(string key)
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

        bool ICache<RecipesInfo>.RemoveInfo(RecipesInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("account info can not be null");
            }
            string key = param.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Remove(key);
            return issuccess;
        }

        bool ICache<RecipesInfo>.SaveInfo(RecipesInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("account info can not be null");
            }
            string key = param.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Set(key, param);
            Session.Current.Expire(key, ExpireTime);
            return issuccess;
        }

        RecipesInfo ICache<RecipesInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("search key can not be null");
            }

            RecipesInfo result = null;
            result = Session.Current.Get<RecipesInfo>(key);
            return result;
        }
    }
}
