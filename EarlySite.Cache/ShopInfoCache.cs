namespace EarlySite.Cache
{

    using System;
    using Model.Database;
    using EarlySite.Cache.CacheBase;
    using System.Collections.Generic;

    /// <summary>
    /// 门店信息缓存
    /// <!--Redis Key格式-->
    /// DB_SI_门店编号_门店名称
    /// </summary>
    public partial class ShopInfoCache : IShopCache
    {
    }

    /// <summary>
    /// 门店信息缓存
    /// <!--Redis Key格式-->
    /// DB_SI_门店编号_门店名称
    /// </summary>
    public partial class ShopInfoCache : IShopCache
    {
        /// <summary>
        /// 有效时间
        /// </summary>
        public const int EffectiveTime = 15;


        /// <summary>
        /// 失效时间
        /// </summary>
        public static DateTime ExpireTime { get { return DateTime.Now.AddDays(EffectiveTime); } }

        void ICache<ShopInfo>.LoadCache()
        {
            throw new NotImplementedException();
        }

        bool ICache<ShopInfo>.RemoveInfo(string key)
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

        bool ICache<ShopInfo>.RemoveInfo(ShopInfo param)
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

        bool ICache<ShopInfo>.SaveInfo(ShopInfo param)
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

        ShopInfo ICache<ShopInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("search key can not be null");
            }

            ShopInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<ShopInfo>(keys[0]);
            }
            return result;
        }
    }


}
