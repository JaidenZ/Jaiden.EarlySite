namespace Jaiden.Redis
{
    using ServiceStack.Redis;
    using System;
    using System.Collections.Generic;

    public sealed partial class RedisList
    {
        private IRedisClient redis = null;
        
        /// <summary>
        /// 获取当前列表在缓冲系统中的标识符
        /// </summary>
        public string Key
        {
            get;
            internal set;
        }

        /// <summary>
        /// 链表容器的最大数量
        /// </summary>
        public const int MAX_CAPACITY = (1 << 31);

        public RedisList(string key)
        {
            this.Key = key;
            this.redis = RedisClientManager.GetClient();
        }

        public RedisList(string key, string host, int port)
        {
            RedisEndpoint conf = new RedisEndpoint(host, port);
            this.Key = key;
            this.redis = new RedisClient(conf);
        }
        /// <summary>
        /// 在链表左侧压入一条字符串数据
        /// </summary>
        /// <param name="value">缓存的字符串数据</param>
        public void LPush(string value)
        {
            redis.PushItemToList(Key, value);
        }
        /// <summary>
        /// 在链表右侧压入一条字符串数据
        /// </summary>
        /// <param name="value">缓存的字符串数据</param>
        public void RPush(string value)
        {
            redis.PrependItemToList(Key, value);
        }
        /// <summary>
        /// 在链表右侧压入多条字符串数据
        /// </summary>
        /// <param name="values"></param>
        public void RPushRange(List<string> values)
        {
            redis.PrependRangeToList(Key, values);
        }
        /// <summary>
        /// 在链表左侧压入多条字符串数据
        /// </summary>
        public void LPushRange(List<string> values)
        {
            if (values != null)
            {
                foreach (string value in values)
                {
                    this.LPush(value);
                }
            }
        }
        /// <summary>
        /// 在链表尾部追加多条字符串数据
        /// </summary>
        /// <param name="values"></param>
        public void AddRange(List<string> values)
        {
            redis.AddRangeToList(Key, values);
        }
        /// <summary>
        /// 调整过期时间
        /// </summary>
        public bool Expire(DateTime expire)
        {
            return redis.ExpireEntryAt(Key, expire);
        }
        /// <summary>
        /// 调整过期时间
        /// </summary>
        /// <returns></returns>
        public bool Expire(TimeSpan expire)
        {
            return redis.ExpireEntryIn(Key, expire);
        }
        /// <summary>
        /// 在尾部追加一条字符串数据
        /// </summary>
        /// <param name="value">缓存的字符串数据</param>
        public void Add(string value)
        {
            redis.AddItemToList(Key, value);
        }
        /// <summary>
        /// 删除链表内所有的字符串数据
        /// </summary>
        public void RemoveAll()
        {
            redis.RemoveAllFromList(Key);
        }
        /// <summary>
        /// 移除list中，key/value,与参数相同的值，并返回移除的数量
        /// </summary>
        /// <param name="value">欲被删除的值</param>
        /// <returns></returns>
        public long Remove(string value)
        {
            return redis.RemoveItemFromList(Key, value);
        }
        /// <summary>
        /// 从尾部移除一条字符串数据，返回移除的数据
        /// </summary>
        /// <returns></returns>
        public string Pop()
        {
            return redis.PopItemFromList(Key);
        }
        /// <summary>
        /// 从list的头部移除一个数据，返回移除的值
        /// </summary>
        /// <returns></returns>
        public string LPop()
        {
            return redis.RemoveStartFromList(Key);
        }
        /// <summary>
        /// 从尾部移除一个数据，返回移除的数据
        /// </summary>
        /// <returns></returns>
        public string RPop()
        {
            return redis.RemoveEndFromList(Key);
        }
        /// <summary>
        /// 获取链表内的总数量
        /// </summary>
        /// <returns></returns>
        public long Count
        {
            get
            {
                return redis.GetListCount(Key);
            }
        }
        /// <summary>
        /// 获取或设置索引位置的字符串数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get
            {
                return redis.GetItemFromList(Key, index);
            }
            set
            {
                redis.SetItemInList(Key, index, value);
            }
        }
        /// <summary>
        /// 获取key中下标为star到end的值集合
        /// </summary>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns></returns>
        public List<string> Range(int start, int end)
        {
            return redis.GetRangeFromList(Key, start, end);
        }
    }
}
