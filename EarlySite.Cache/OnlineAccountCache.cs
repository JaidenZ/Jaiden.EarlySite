namespace EarlySite.Cache
{
    using System;
    using Model.Database;
    using EarlySite.Cache.CacheBase;
    using System.Collections.Generic;

    /// <summary>
    /// 在线账户缓存
    /// <!--Redis Key格式-->
    /// OnlineAI_手机号_邮箱号
    /// </summary>
    public partial class OnlineAccountCache : IOnlineAccountCache
    {
        
        /// <summary>
        /// 更新在线账户信息缓存
        /// </summary>
        /// <param name="online"></param>
        void IOnlineAccountCache.UpdateOnlineAccount(OnlineAccountInfo online)
        {
            if (online == null)
            {
                throw new ArgumentNullException("onlinecache info can not be null");
            }
            //从缓存中拿数据
            string key = string.Format("OnlineAI_{0}_*", online.Phone);
            IList<string> list = Session.Current.ScanAllKeys(key);
            if (list != null && list.Count > 0)
            {
                OnlineAccountInfo infocache = Session.Current.Get<OnlineAccountInfo>(list[0]);

                //修改数据
                infocache.NickName = online.NickName;
                infocache.Sex = online.Sex;
                infocache.Description = online.Description;
                infocache.BirthdayDate = online.BirthdayDate;

                //保存
                Session.Current.Set(infocache.GetKeyName(), infocache);
                Session.Current.Expire(infocache.GetKeyName(), ExpireTime);
            }
        }
    }

    /// <summary>
    /// 在线账户缓存
    /// <!--Redis Key格式-->
    /// OnlineAI_手机号_邮箱号
    /// </summary>
    public partial class OnlineAccountCache: IOnlineAccountCache
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
        void ICache<OnlineAccountInfo>.LoadCache()
        {
            //在线账户无加载
        }

        /// <summary>
        /// 根据键值搜索账户信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        OnlineAccountInfo ICache<OnlineAccountInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("search key can not be null");
            }

            OnlineAccountInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if(keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<OnlineAccountInfo>(keys[0]);
            }
            return result;

        }

        /// <summary>
        /// 根据键值移除缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ICache<OnlineAccountInfo>.RemoveInfo(string key)
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
        /// 移除在线账户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool ICache<OnlineAccountInfo>.RemoveInfo(OnlineAccountInfo param)
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
        /// 保存在线账户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        bool ICache<OnlineAccountInfo>.SaveInfo(OnlineAccountInfo param)
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
