namespace EarlySite.Cache
{
    using System;
    using Model.Database;
    using EarlySite.Cache.CacheBase;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;

    /// <summary>
    /// 账户信息缓存
    /// <!--Redis Key格式-->
    /// DB_AI_手机号_邮箱号_昵称_性别
    /// </summary>
    public partial class AccountInfoCache : IAccountInfoCache
    {
        /// <summary>
        /// 检查邮箱是否存在
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        bool IAccountInfoCache.CheckMailExists(string mail)
        {
            if (string.IsNullOrEmpty(mail))
            {
                throw new ArgumentNullException("exists mail can no be null");
            }
            string key = string.Format("DB_AI_*_{0}", mail);
            IList<string> list = Session.Current.ScanAllKeys(key);
            if(list != null && list.Count > 0)
            {
                return true;
            }

            if(DBConnectionManager.Instance.Reader.Count(new AccountCheckSpefication(mail,1).Satifasy()) > 0)
            {
                return true;
            }

            return false;

        }

        /// <summary>
        /// 检查电话是否存在
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool IAccountInfoCache.CheckPhoneExists(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException("exists phone can no be null");
            }
            string key = string.Format("DB_AI_{0}_*", phone);
            IList<string> list = Session.Current.ScanAllKeys(key);
            if (list != null && list.Count > 0)
            {
                return true;
            }

            if (DBConnectionManager.Instance.Reader.Count(new AccountCheckSpefication(phone, 0).Satifasy()) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 更新账户信息缓存
        /// </summary>
        /// <param name="account"></param>
        void IAccountInfoCache.UpdateAccount(AccountInfo account)
        {
            if(account == null)
            {
                throw new ArgumentNullException("account info can not be null");
            }
            //从缓存中拿数据
            string key = string.Format("DB_AI_{0}_*", account.Phone);
            IList<string> list = Session.Current.ScanAllKeys(key);
            if (list != null && list.Count > 0)
            {
                AccountInfo infocache = Session.Current.Get<AccountInfo>(list[0]);

                //修改数据
                infocache.NickName = account.NickName;
                infocache.Sex = account.Sex;
                infocache.Description = account.Description;
                infocache.BirthdayDate = account.BirthdayDate;

                //保存
                Session.Current.Set(infocache.GetKeyName(), infocache);
                Session.Current.Expire(infocache.GetKeyName(), ExpireTime);
            }
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
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<AccountInfo>(keys[0]);
            }
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
