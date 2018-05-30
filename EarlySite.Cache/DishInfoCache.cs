﻿namespace EarlySite.Cache
{
    using EarlySite.Model.Database;
    using EarlySite.Cache.CacheBase;
    using System;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;


    /// <summary>
    /// 单品信息缓存
    /// <!--Redis Key格式-->
    /// DB_DI_编号_类型_用餐时间_商店编号
    /// </summary>
    public partial class DishInfoCache : IDishCache
    {
        /// <summary>
        /// 获取单个单品信息
        /// </summary>
        /// <param name="dishId"></param>
        /// <returns></returns>
        DishInfo IDishCache.GetDishInfoById(int dishId)
        {
            DishInfo result = null;
            string key = string.Format("DB_DI_{0}_*", dishId);
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<DishInfo>(keys[0]);
            }
            else
            {
                //从数据库拿取
                IList<DishInfo> dishlist = DBConnectionManager.Instance.Reader.Select<DishInfo>(new DishSelectSpefication(dishId.ToString(), 0).Satifasy());
                if(dishlist != null && dishlist.Count > 0)
                {
                    //同步到缓存
                    result = dishlist[0];
                    Session.Current.Set(result.GetKeyName(), result);
                    Session.Current.Expire(result.GetKeyName(), ExpireTime);
                }
            }
            return result;
        }
    }


    /// <summary>
    /// 单品信息缓存
    /// <!--Redis Key格式-->
    /// DB_DI_编号_类型_用餐时间_商店编号
    /// </summary>
    public partial class DishInfoCache :  IDishCache
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
        void ICache<DishInfo>.LoadCache()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通过键值移除单品信息缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ICache<DishInfo>.RemoveInfo(string key)
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

        /// <summary>
        /// 移除单品信息缓存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool ICache<DishInfo>.RemoveInfo(DishInfo param)
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

        /// <summary>
        /// 保存单品信息到缓存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool ICache<DishInfo>.SaveInfo(DishInfo param)
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

        /// <summary>
        /// 根据键值搜索单品缓存信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        DishInfo ICache<DishInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key can not be null");
            }
            DishInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<DishInfo>(keys[0]);
            }
            return result;
        }
        
    }
}
