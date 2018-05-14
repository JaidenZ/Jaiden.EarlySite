﻿namespace EarlySite.Cache
{
    using System;
    using Core.Collection;
    using Model.Show;
    using Model.Database;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;

    public class AccountInfoCache
    {
        /**
         * 账户信息缓存Redis Key格式
         * DB_AI_手机号_邮箱号_昵称_性别
         * */


        /// <summary>
        /// 有效时间
        /// </summary>
        private const int EffectiveTime = 15;

        /// <summary>
        /// 失效时间
        /// </summary>
        private static DateTime ExpireTime { get { return DateTime.Now.AddDays(EffectiveTime); } }


        /// <summary>
        /// 保存账户信息集合到缓存
        /// </summary>
        /// <param name="accounts"></param>
        public static void SaveAccountInfoToCache(IList<AccountInfo> accounts)
        {
            if(accounts != null && accounts.Count > 0)
            {
                for (int i = 0; i < accounts.Count; i++)
                {
                     SaveAccountInfoToCache(accounts[i]);
                }
            }
        }

        /// <summary>
        /// 保存账户信息到缓存
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool SaveAccountInfoToCache(AccountInfo account)
        {
            string key = account.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Set(key, account);
            Session.Current.Expire(key, ExpireTime);
            return issuccess;
        }

        /// <summary>
        /// 从缓存删除账户信息
        /// </summary>
        /// <returns><c>true</c>, if acccount info from cache was deleted, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public static bool DeleteAcccountInfoFromCache(AccountInfo account)
        {
            string key = account.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Remove(key);
            return issuccess;
        }
        

        /// <summary>
        /// 根据手机号获取信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static AccountInfo GetAccountInfoByPhone(string phone)
        {
            AccountInfo result = null;
            string key = string.Format("DB_AI_{0}", phone);
            result = Session.Current.Get<AccountInfo>(key);
            if(result == null)
            {
                result = GetAccountInfoByPhoneFromDB(phone);
            }
            return result;
        }

        /// <summary>
        /// 根据手机号从数据库获取信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private static AccountInfo GetAccountInfoByPhoneFromDB(string phone)
        {
            AccountInfo result = null;
            IList<AccountInfo> inforesult = DBConnectionManager.Instance.Reader.Select<AccountInfo>(new AccountSelectSpefication(0, phone).Satifasy());
            if (inforesult != null && inforesult.Count > 0)
            {
                result = inforesult[0];
                //保存到缓存
                SaveAccountInfoToCache(result);
            }
            return result;
        }


        private static AccountInfoCache _instance;


        private static SafetyList<AccountInfo> _list;

        private static Account _currentAccount;

        public AccountInfoCache()
        {
            _list = new SafetyList<AccountInfo>();
        }

        public static AccountInfoCache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AccountInfoCache();
                return _instance;
            }
        }



        public SafetyList<AccountInfo> Array
        {
            get
            {
                if (_list == null)
                    _list = new SafetyList<AccountInfo>();
                return _list;
            }
            private set
            {
                _list = value;
            }
        }

        public Account CurrentAccount
        {
            get
            {
                return _currentAccount;
            }
            set
            {
                _currentAccount = value;
            }
        }

    }
}
