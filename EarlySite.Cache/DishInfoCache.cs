namespace EarlySite.Cache
{

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

    }
}
