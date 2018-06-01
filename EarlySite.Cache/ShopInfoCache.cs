namespace EarlySite.Cache
{

    using System;
    using Model.Database;
    using EarlySite.Cache.CacheBase;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;

    /// <summary>
    /// 门店信息缓存
    /// <!--Redis Key格式-->
    /// DB_SI_门店编号_门店名称
    /// </summary>
    public partial class ShopInfoCache : IShopCache
    {
        /// <summary>
        /// 根据门店编号获取门店信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        ShopInfo IShopCache.GetshopInfoByShopId(int shopId)
        {
            ShopInfo result = null;
            if(shopId == 0)
            {
                throw new ArgumentNullException("Shop Id can not be zero");
            }

            string key = string.Format("DB_SI_{0}_*",shopId);
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<ShopInfo>(keys[0]);
            }
            else
            {
                //从数据库获取数据
                IList<ShopInfo> shopinfolist = DBConnectionManager.Instance.Reader.Select<ShopInfo>(new ShopSelectSpefication(shopId.ToString(), 0).Satifasy());

                if (shopinfolist != null && shopinfolist.Count > 0)
                {
                    result = shopinfolist[0];
                    //同步缓存
                    Session.Current.Set(result.GetKeyName(), result);
                }
            }
            return result;
        }

        /// <summary>
        /// 设置门店禁用
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        bool IShopCache.SetShopInfoEnable(int shopId, bool enable)
        {
            if (shopId == 0)
            {
                throw new ArgumentNullException("shopId can not be zero");
            }
            bool result = false;

            string key = string.Format("DB_SI_{0}_*", shopId);

            ShopInfo updateinfo = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                updateinfo = Session.Current.Get<ShopInfo>(keys[0]);
            }
            if (updateinfo != null)
            {
                updateinfo.Enable = enable;
                result = Session.Current.Set(updateinfo.GetKeyName(), updateinfo);
            }
            return result;
        }
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
