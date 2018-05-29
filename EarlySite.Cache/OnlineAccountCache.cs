namespace EarlySite.Cache
{
    using System;
    using Model.Database;
    using EarlySite.Cache.CacheBase;

    /// <summary>
    /// 在线账户缓存
    /// <!--Redis Key格式-->
    /// OnlineAI_手机号_邮箱号
    /// </summary>
    public partial class OnlineAccountCache : IOnlineAccountCache
    {

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
            result = Session.Current.Get<OnlineAccountInfo>(key);
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
