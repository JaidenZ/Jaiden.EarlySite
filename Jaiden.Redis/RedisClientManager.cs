namespace Jaiden.Redis
{
    using ServiceStack.Caching;
    using ServiceStack.Redis;

    public static class RedisClientManager
    {
        private static PooledRedisClientManager prcm = null;
        private static object look = new object();

        /// <summary>
        /// 可读写的缓存服务器主机池
        /// </summary>
        public static string[] ReadWriteHosts
        {
            get;
            set;
        }

        /// <summary>
        /// 只读的缓存服务器主机池
        /// </summary>
        public static string[] ReadOnlyHosts { get; set; }

        static RedisClientManager()
        {
            RedisClientManager.ReadWriteHosts = new string[] { "127.0.0.1" };
            RedisClientManager.ReadOnlyHosts = new string[] { "127.0.0.1" };
        }

        public static IRedisClientsManager GetClientManager()
        {
            lock (look)
            {
                if (prcm == null)
                {
                    RedisClientManagerConfig conf = new RedisClientManagerConfig()
                    {
                        AutoStart = true,
                    };
                    conf.MaxReadPoolSize = 12800;
                    conf.MaxWritePoolSize = 12800;
                    prcm = new PooledRedisClientManager(RedisClientManager.ReadWriteHosts, RedisClientManager.ReadOnlyHosts, conf)
                    {
                        PoolTimeout = 600000
                    };
                }
            }
            return prcm;
        }

        /// <summary>
        /// 获取一个Redis缓存服务器的客户端 
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            var prcm = RedisClientManager.GetClientManager();
            return prcm.GetClient();
        }

        /// <summary>
        /// 获取一个Redis缓存客户端
        /// </summary>
        /// <returns></returns>
        public static ICacheClient GetCacheClient()
        {
            var prcm = RedisClientManager.GetClientManager();
            return prcm.GetCacheClient();
        }

        /// <summary>
        /// 获取一个Redis只读客户端
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetReadOnlyClient()
        {
            var prcm = RedisClientManager.GetClientManager();
            return prcm.GetReadOnlyClient();
        }

        /// <summary>
        /// 异步：要求Redis对缓存进行持久化
        /// </summary>
        public static void SaveAsync()
        {
            IRedisClient redis = RedisClientManager.GetClient();
            redis.SaveAsync();
        }
    }
}
