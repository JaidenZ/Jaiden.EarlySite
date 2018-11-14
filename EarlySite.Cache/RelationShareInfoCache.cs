namespace EarlySite.Cache
{
    using CacheBase;
    using EarlySite.Drms.DBManager;
    using EarlySite.Drms.Spefication.RelationSpefication;
    using EarlySite.Model.Database;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 分享关系信息缓存
    /// <!--Redis Key格式-->
    /// DB_RS_食谱编号_食物编号_手机号
    /// </summary>
    public partial class RelationShareInfoCache : IRelationShareInfoCache
    {
        /// <summary>
        /// 获取单品下的分享关系集合
        /// </summary>
        /// <param name="dishId"></param>
        /// <returns></returns>
        IList<RelationShareInfo> IRelationShareInfoCache.GetRelationShareByDishId(int dishId)
        {
            IList<RelationShareInfo> result = new List<RelationShareInfo>();

            if (dishId == 0)
            {
                throw new ArgumentNullException("dishId can not be zero");
            }
            string key = string.Format("DB_RS_*_{0}_*", dishId);
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                foreach (string k in keys)
                {
                    result.Add(Session.Current.Get<RelationShareInfo>(k));
                }
            }
            else
            {
                //从数据库拿取
                result = DBConnectionManager.Instance.Reader.Select<RelationShareInfo>(new RelationShareSelectSpefication(dishId.ToString(), 2).Satifasy());
                if (result != null && result.Count > 0)
                {
                    foreach (RelationShareInfo item in result)
                    {
                        //同步到缓存
                        Session.Current.Set(item.GetKeyName(), item);
                        Session.Current.Expire(item.GetKeyName(), ExpireTime);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取单品下的分享关系集合
        /// </summary>
        /// <param name="dishId"></param>
        /// <returns></returns>
        IList<RelationShareInfo> IRelationShareInfoCache.GetRelationShareByDishId(IList<int> dishId)
        {
            IList<RelationShareInfo> result = new List<RelationShareInfo>();

            if (dishId == null || dishId.Count == 0)
            {
                throw new ArgumentNullException("dishIds can not be null");
            }

            foreach (int ditem in dishId)
            {
                string key = string.Format("DB_RS_*_{0}_*", ditem);
                IList<string> keys = Session.Current.ScanAllKeys(key);
                if (keys != null && keys.Count > 0)
                {
                    foreach (string k in keys)
                    {
                        result.Add(Session.Current.Get<RelationShareInfo>(k));
                    }
                }
                else
                {
                    //从数据库拿取
                    result = DBConnectionManager.Instance.Reader.Select<RelationShareInfo>(new RelationShareSelectSpefication(ditem.ToString(), 2).Satifasy());
                    if (result != null && result.Count > 0)
                    {
                        foreach (RelationShareInfo item in result)
                        {
                            //同步到缓存
                            Session.Current.Set(item.GetKeyName(), item);
                            Session.Current.Expire(item.GetKeyName(), ExpireTime);
                        }
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// 根据手机号获取分享集合缓存
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        IList<RelationShareInfo> IRelationShareInfoCache.GetRelationShareByPhone(long phone)
        {
            IList<RelationShareInfo> result = new List<RelationShareInfo>();

            if(phone == 0)
            {
                throw new ArgumentNullException("phone can not be zero");
            }
            string key = string.Format("DB_RS_*_*_{0}",phone);
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                foreach (string k in keys)
                {
                    result.Add(Session.Current.Get<RelationShareInfo>(k));
                }
            }
            else
            {
                //从数据库拿取
                result = DBConnectionManager.Instance.Reader.Select<RelationShareInfo>(new RelationShareSelectSpefication(phone.ToString(), 0).Satifasy());
                if (result != null && result.Count > 0)
                {
                    foreach (RelationShareInfo item in result)
                    {
                        //同步到缓存
                        Session.Current.Set(item.GetKeyName(), item);
                        Session.Current.Expire(item.GetKeyName(), ExpireTime);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据食谱编号获取分享集合
        /// </summary>
        /// <param name="receipeId"></param>
        /// <returns></returns>
        IList<RelationShareInfo> IRelationShareInfoCache.GetRelationShareByReceipId(int receipeId)
        {
            IList<RelationShareInfo> result = new List<RelationShareInfo>();

            if (receipeId == 0)
            {
                throw new ArgumentNullException("receipeId can not be zero");
            }
            string key = string.Format("DB_RS_{0}_*_*", receipeId);
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                foreach (string k in keys)
                {
                    result.Add(Session.Current.Get<RelationShareInfo>(k));
                }
            }
            else
            {
                //从数据库拿取
                result = DBConnectionManager.Instance.Reader.Select<RelationShareInfo>(new RelationShareSelectSpefication(receipeId.ToString(), 1).Satifasy());
                if (result != null && result.Count > 0)
                {
                    foreach (RelationShareInfo item in result)
                    {
                        //同步到缓存
                        Session.Current.Set(item.GetKeyName(), item);
                        Session.Current.Expire(item.GetKeyName(), ExpireTime);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 解除用户的食谱关系
        /// </summary>
        /// <param name="recipesId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool IRelationShareInfoCache.RemoveRelationShareByPhone(long phone)
        {
            bool result = false;
            if(phone == 0)
            {
                throw new ArgumentNullException("phone can not be zero");
            }
            string key = string.Format("DB_RS_*_*_{0}", phone);
            if (Session.Current.Contains(key))
            {
                result = Session.Current.Remove(key);
            }
            return result;
        }

        /// <summary>
        /// 根据食谱编号 手机号移除缓存关系
        /// </summary>
        /// <param name="recipesId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool IRelationShareInfoCache.RemoveRelationShareByRecipes(int recipesId, long phone)
        {
            if(recipesId == 0 ||  phone == 0)
            {
                throw new ArgumentNullException("recipesid or phone can not be zero");
            }

            bool result = false;
            string key = string.Format("DB_RS_{0}_*_{1}", recipesId, phone);
            if (Session.Current.Contains(key))
            {
                result = Session.Current.Remove(key);
            }
            return result;
        }


    }


    /// <summary>
    /// 分享关系信息缓存
    /// <!--Redis Key格式-->
    /// DB_RS_食谱编号_食物编号_手机号
    /// </summary>
    public partial class RelationShareInfoCache : IRelationShareInfoCache
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
        void ICache<RelationShareInfo>.LoadCache()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 通过key移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ICache<RelationShareInfo>.RemoveInfo(string key)
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

        bool ICache<RelationShareInfo>.RemoveInfo(RelationShareInfo param)
        {
            if(param == null)
            {
                throw new ArgumentNullException("RelationShare info can not be null");
            }

            string key = param.GetKeyName();
            bool issuccess = Session.Current.Remove(key);
            return issuccess;
        }

        bool ICache<RelationShareInfo>.SaveInfo(RelationShareInfo param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("RelationShare info can not be null");
            }
            string key = param.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Set(key, param);
            Session.Current.Expire(key, ExpireTime);
            return issuccess;
        }

        RelationShareInfo ICache<RelationShareInfo>.SearchInfoByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("search key can not be null");
            }

            RelationShareInfo result = null;
            IList<string> keys = Session.Current.ScanAllKeys(key);
            if (keys != null && keys.Count > 0)
            {
                result = Session.Current.Get<RelationShareInfo>(keys[0]);
            }
            return result;
        }
    }
}
