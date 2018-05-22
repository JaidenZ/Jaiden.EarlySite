namespace EarlySite.Cache
{
    using System.Collections.Generic;
    using EarlySite.Model.Database;
    using System;

    /// <summary>
    /// 单品信息缓存
    /// </summary>
    public class DishInfoCache
    {
        /**
         * 单品信息缓存Redis Key格式
         * DBDish_单品编号
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
        /// 保存单品信息到缓存
        /// </summary>
        /// <param name="dish">单品信息</param>
        /// <returns></returns>
        public static bool SaveDishInfoToCache(DishInfo dish)
        {

            string key = dish.GetKeyName();
            bool issuccess = false;
            issuccess = Session.Current.Set(key, dish);
            Session.Current.Expire(key, ExpireTime);
            return issuccess;
        }

        /// <summary>
        /// 保存单品信息到缓存
        /// </summary>
        /// <param name="dish">单品信息集合</param>
        public static void SaveDishInfoToCache(IList<DishInfo> dish)
        {
            if(dish == null)
            {
                throw new ArgumentNullException("DishInfo collection is null");
            }
            foreach (DishInfo item in dish)
            {
                SaveDishInfoToCache(item);
            }
        }

    }
}
