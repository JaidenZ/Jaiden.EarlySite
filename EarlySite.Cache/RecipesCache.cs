namespace EarlySite.Cache
{
    using System;
    using Model.Database;
    using EarlySite.Cache.CacheBase;
    using System.Collections.Generic;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication;

    /// <summary>
    /// 食谱信息缓存
    /// <!--Redis Key格式-->
    /// DB_RI_食谱编号_手机号
    /// </summary>
    public partial class RecipesCache : IRecipesCache
    {
        RecipesInfo IRecipesCache.GetRecipesInfoById(int recipesId)
        {
            string key = string.Format("DB_RI_{0}_*", recipesId);

            RecipesInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<RecipesInfo>(keys[0]);
            }
            if (result == null)
            {

                //从数据库获取数据
                IList<RecipesInfo> recipeslist = DBConnectionManager.Instance.Reader.Select<RecipesInfo>(new RecipesSelectSpefication(recipesId.ToString(), 0).Satifasy());

                if (recipeslist != null && recipeslist.Count > 0)
                {
                    //更新缓存
                    result = recipeslist[0];
                    Session.Current.Set(result.GetKeyName(), result);
                    Session.Current.Expire(result.GetKeyName(), ExpireTime);
                }
            }
            return result;
        }
    }


    /// <summary>
    /// 食谱信息缓存
    /// <!--Redis Key格式-->
    /// DB_RI_食谱编号_手机号
    /// </summary>
    public partial class RecipesCache : IRecipesCache
    {
        /// <summary>
        /// 有效时间
        /// </summary>
        public const int EffectiveTime = 15;


        /// <summary>
        /// 失效时间
        /// </summary>
        public static DateTime ExpireTime { get { return DateTime.Now.AddDays(EffectiveTime); } }


        void ICache<RecipesInfo>.LoadCache()
        {
            throw new NotImplementedException();
        }

        bool ICache<RecipesInfo>.RemoveInfo(string key)
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

        bool ICache<RecipesInfo>.RemoveInfo(RecipesInfo param)
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

        bool ICache<RecipesInfo>.SaveInfo(RecipesInfo param)
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

        RecipesInfo ICache<RecipesInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("search key can not be null");
            }

            RecipesInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<RecipesInfo>(keys[0]);
            }
            return result;
        }
    }
}
