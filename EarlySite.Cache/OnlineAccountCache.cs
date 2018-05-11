namespace EarlySite.Cache
{
    using Model.Database;
    using System;
    using System.Collections.Generic;
    public class OnlineAccountCache
    {
        
        /**
         * 在线账户缓存Redis Key格式
         * OnlineAI_手机号_邮箱号
         * */



        /// <summary>
        /// 有效时间
        /// </summary>
        private const int EffectiveTime = 7;

        /// <summary>
        /// 失效时间
        /// </summary>
        private static DateTime ExpireTime { get { return DateTime.Now.AddDays(EffectiveTime); } }

        /// <summary>
        /// 根据手机号码获取缓存在线用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static AccountInfo GetOnlineAccountInfoByPhone(string phone)
        {
            AccountInfo accountinfo = Session.Current.Get<AccountInfo>(string.Format("OnlineAI_{0}",phone));

            return accountinfo;   
        }

        /// <summary>
        /// 根据邮箱获取缓存在线用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static AccountInfo GetOnlineAccountInfoByEmail(string email)
        {
            AccountInfo accountinfo = Session.Current.Get<AccountInfo>(string.Format("OnlineAI_*_{0}", email));

            return accountinfo;
        }

        

        /// <summary>
        /// 保存在线用户缓存
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool SaveOnlineAccountInfoToCache(AccountInfo account)
        {
            string key = string.Format("OnlineAI_{0}_{1}", account.Phone, account.Email);
            bool issuccess = false;
            issuccess = Session.Current.Set(key, account);
            Session.Current.Expire(key, ExpireTime);
            return issuccess;
        }


    }
}
