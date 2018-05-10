namespace Jaiden.Redis
{
    using EarlySite.Core.Component;
    using System;

    /// <summary>
    /// 消息队列的消息消费者
    /// </summary>
    public abstract class RedisMqConsumer<T> where T : class, new()
    {
        /// <summary>
        /// 实例化一个新消息消费者
        /// </summary>
        /// <param name="service">消息队列服务</param>
        /// <param name="chanels">消息队列管道</param>
        public RedisMqConsumer(RedisQueueService service, string chanels)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(chanels));
            Contract.Requires<ArgumentNullException>(service != null);
            {
                service.Subscribe<T>(chanels, Handle);
                service.Start();
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message"></param>
        public abstract void Handle(T message);
    }
}
