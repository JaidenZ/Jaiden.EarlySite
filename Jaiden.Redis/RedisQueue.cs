namespace Jaiden.Redis
{
    using System;

    /// <summary>
    /// 基于Redis链表的一种队列(先进先出)
    /// </summary>
    public partial class RedisQueue
    {
        private RedisList redis = null;

        public string Chanels
        {
            get
            {
                return redis.Key;
            }
            internal set
            {
                redis.Key = value;
            }
        }
        /// <summary>
        /// 实例化一个消息队列
        /// </summary>
        /// <param name="chanels">消息队列的管道名</param>
        public RedisQueue(string chanels)
        {
            if (string.IsNullOrEmpty(chanels))
            {
                throw new ArgumentException("chanels");
            }
            redis = new RedisList(chanels);
        }
        /// <summary>
        /// 入列
        /// </summary>
        public void Enqueue(string value)
        {
            redis.RPush(value);
        }
        /// <summary>
        /// 出列
        /// </summary>
        public string Dequeue()
        {
            return redis.Pop();
        }
        /// <summary>
        /// 计数
        /// </summary>
        public long Count
        {
            get
            {
                return redis.Count;
            }
        }
    }
}
