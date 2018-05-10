namespace Jaiden.Redis
{
    using ServiceStack.Redis;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class RedisKeyValue
    {
        private IRedisClient m_r = RedisClientManager.GetReadOnlyClient();
        private IRedisClient m_w = RedisClientManager.GetClient();

        /// <summary>
        /// 获取在缓存服务器中所有存在的Key值
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllKeys()
        {
            lock (m_r)
            {
                return m_r.GetAllKeys();
            }
        }

        /// <summary>
        /// 扫描符合条件的键名迭代器
        /// </summary>
        /// <param name="pattern">通配符表达式（语法：A-*、表示匹配前面包含A-的任意键）</param>
        /// <returns></returns>
        public IList<string> ScanAllKeys(string pattern)
        {
            lock (m_r)
            {
                return m_r.GetKeysByPattern(pattern).ToList(); // ScanAllKeys
            }
        }
        /// <summary>
        /// 设置Key的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Set(string key, object value)
        {
            lock (m_w)
            {
                return m_w.Set(key, value);
            }
        }
        /// <summary>
        /// 获取Key的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            lock (m_r)
            {
                return m_r.Get<T>(key);
            }
        }
        /// <summary>
        /// 获取Keys内所有的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            lock (m_r)
            {
                return m_r.GetAll<T>(keys);
            }
        }
        /// <summary>
        /// 设置一个key-value组合到缓存服务器内
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="keyValues">键值对</param>
        public void SetAll<T>(IDictionary<string, T> keyValues)
        {
            lock (m_w)
            {
                m_w.SetAll<T>(keyValues);
            }
        }
        /// <summary>
        /// 删除key与其关联的value
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            lock (m_w)
            {
                return m_w.Remove(key);
            }
        }
        /// <summary>
        /// 删除key与其关联的value
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public void Remove(IEnumerable<string> keys)
        {
            lock (m_w)
            {
                m_w.RemoveAll(keys);
            }
        }
        /// <summary>
        /// 删除全部
        /// </summary>
        public void RemoveAll()
        {
            List<string> keys = GetAllKeys();
            if (keys != null && keys.Count > 0)
            {
                this.Remove(keys);
            }
        }
        /// <summary>
        /// 检查Key是否已存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            lock (m_r)
            {
                return m_r.ContainsKey(key);
            }
        }
        /// <summary>
        /// 在key原有value上追加内容
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public long Append(string key, string value)
        {
            lock (m_w)
            {
                return m_w.AppendToValue(key, value);
            }
        }
        /// <summary>
        /// 获取以前的值在赋上一个新值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public string GetAndSetValue(string key, string value)
        {
            lock (m_w)
            {
                return m_w.GetAndSetValue(key, value);
            }
        }
        /// <summary>
        /// 获取键的值长度
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public long GetValueCount(string key)
        {
            lock (m_r)
            {
                return m_r.GetStringCount(key);
            }
        }
        /// <summary>
        /// 在特定键上自增其值
        /// </summary>
        /// <returns></returns>
        public long IncrementValue(string key)
        {
            lock (m_w)
            {
                return m_w.IncrementValue(key);
            }
        }

        /// <summary>
        /// 在特定键上增加其值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="num">递加的值</param>
        /// <returns></returns>
        public long IncrementValue(string key, int num)
        {
            lock (m_w)
            {
                return m_w.Increment(key, unchecked((uint)num));
            }
        }

        /// <summary>
        /// 在特定键上增加其值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="num">递加的值</param>
        /// <returns></returns>
        public double IncrementValue(string key, double num)
        {
            lock (m_w)
            {
                return m_w.IncrementValueBy(key, num);
            }
        }
        /// <summary>
        /// 在特定键上递减其值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public long DecrementValue(string key)
        {
            lock (m_w)
            {
                return m_w.DecrementValue(key);
            }
        }
        /// <summary>
        /// 在特定键上递减其值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="num">递减的值</param>
        /// <returns></returns>
        public long DecrementValue(string key, int num)
        {
            lock (m_w)
            {
                return m_w.DecrementValueBy(key, num);
            }
        }
        /// <summary>
        /// 调整过期时间
        /// </summary>
        public bool Expire(string key, DateTime expire)
        {
            lock (m_w)
            {
                return m_w.ExpireEntryAt(key, expire);
            }
        }
        /// <summary>
        /// 调整过期时间
        /// </summary>
        /// <returns></returns>
        public bool Expire(string key, TimeSpan expire)
        {
            lock (m_w)
            {
                return m_w.ExpireEntryIn(key, expire);
            }
        }
    }
}
