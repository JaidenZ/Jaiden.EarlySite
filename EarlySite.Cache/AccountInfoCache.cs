namespace EarlySite.Cache
{
    using System;
    using Core.Collection;
    using Model.Show;
    using Model.Database;
    using System.Collections.Generic;

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
