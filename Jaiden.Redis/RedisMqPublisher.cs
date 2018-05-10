namespace Jaiden.Redis
{
    using EarlySite.Core.Component;
    using System;

    /// <summary>
    /// 消息队列的消息发布者
    /// </summary>
    public sealed class RedisMqPublisher<T> where T : class
    {
        private RedisQueueService m_service = null;
        private string m_chanels = null;

        /// <summary>
        /// 实例化一个新消息消费者
        /// </summary>
        /// <param name="service">消息队列服务</param>
        /// <param name="chanels">消息队列管道</param>
        public RedisMqPublisher(RedisQueueService service, string chanels)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(chanels));
            Contract.Requires<ArgumentNullException>(service != null);
            {
                m_chanels = chanels;
                m_service = service;
                service.Start();
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">欲被发布的消息</param>
        public void Publish(T message)
        {
            Contract.Requires<ArgumentException>(message != null);
            m_service.Publish(m_chanels, message);
        }
    }
}
