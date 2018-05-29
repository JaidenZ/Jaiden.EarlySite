namespace EarlySite.Cache.CacheBase
{
    using Core.DDD.Service;

    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache<T> : IServiceBase
    {
        
        /// <summary>
        /// 加载缓存
        /// </summary>
        void LoadCache();

        /// <summary>
        /// 搜索信息
        /// </summary>
        /// <param name="key">Redis缓存主键</param>
        /// <returns></returns>
        T SearchInfoByKey(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">Redis缓存主键</param>
        /// <returns></returns>
        bool RemoveInfo(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="param">Redis缓存对象</param>
        /// <returns></returns>
        bool RemoveInfo(T param);

        /// <summary>
        /// 保存到缓存
        /// </summary>
        /// <param name="param">对象</param>
        /// <returns></returns>
        bool SaveInfo(T param);

    }
}
