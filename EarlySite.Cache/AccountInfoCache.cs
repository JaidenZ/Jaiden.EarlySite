namespace EarlySite.Cache
{
    using System;
    using Core.Collection;
    using Model.Show;
    using Model.Database;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;
    using EarlySite.Cache.CacheBase;

    /// <summary>
    /// 账户信息缓存
    /// <!--Redis Key格式-->
    /// DB_AI_手机号_邮箱号_昵称_性别
    /// </summary>
    public partial class AccountInfoCache : IAccountInfoCache
    {
        bool IAccountInfoCache.Test()
        {
            throw new NotImplementedException();
        }
    }






    /// <summary>
    /// 账户信息缓存
    /// <!--Redis Key格式-->
    /// DB_AI_手机号_邮箱号_昵称_性别
    /// </summary>
    public partial class AccountInfoCache :  IAccountInfoCache
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
        void ICache<AccountInfo>.LoadCache()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据键值搜索账户缓存信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        AccountInfo ICache<AccountInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key can not be null");
            }
            AccountInfo result = null;
            result = Session.Current.Get<AccountInfo>(key);
            return result;
        }

        /// <summary>
        /// 根据键值移除账户缓存信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ICache<AccountInfo>.RemoveInfo(string key)
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
        /// 移除账户缓存信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool ICache<AccountInfo>.RemoveInfo(AccountInfo param)
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
        /// 保存账户信息到缓存
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool ICache<AccountInfo>.SaveInfo(AccountInfo param)
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
        
    }
}
