namespace Jaiden.Redis
{
    /// <summary>
    /// 消息队列管理器
    /// </summary>
    public static class RedisQueueManager
    {
        private static RedisQueueService m_splMq = null;
        private static RedisQueueService m_stdMq = null;

        /// <summary>
        /// 特殊队列
        /// </summary>
        public static RedisQueueService Special
        {
            get
            {
                if (m_splMq == null)
                {
                    m_splMq = new RedisQueueService();
                }
                return m_splMq;
            }
        }

        /// <summary>
        /// 标准队列
        /// </summary>
        public static RedisQueueService Standard
        {
            get
            {
                if (m_stdMq == null)
                {
                    m_stdMq = new RedisQueueService();
                }
                return m_stdMq;
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
            Special.Start();
            Standard.Start();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public static void Stop()
        {
            Special.Stop();
            Standard.Stop();
        }
    }
}
