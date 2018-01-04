namespace EarlySite.Cache
{
    using System;
    using Core.Collection;
    using Model.Database;


    public class AccountInfoCache
    {
        private static AccountInfoCache _instance;


        private static SafetyList<AccountInfo> _list;

        private static AccountInfo _currentAccount;

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

        public AccountInfo CurrentAccount
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
