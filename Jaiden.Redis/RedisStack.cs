namespace Jaiden.Redis
{
    using System;

    /// <summary>
    /// 基于Redis链表的一种栈队列(先进后出)
    /// </summary>
    public partial class RedisStack
    {
        private RedisList redis = null;

        /// <summary>
        /// 实例化一个栈队列
        /// </summary>
        /// <param name="chanels">栈队列的管道名</param>
        public RedisStack(string chanels)
        {
            if (string.IsNullOrEmpty(chanels))
            {
                throw new ArgumentException("chanels");
            }
            redis = new RedisList(chanels);
        }
        /// <summary>
        /// 入栈
        /// </summary>
        /// <param name="value">值</param>
        public void Push(string value)
        {
            redis.RPush(value);
        }
        /// <summary>
        /// 移出
        /// </summary>
        /// <returns></returns>
        public string Pop()
        {
            return redis.RPop();
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
