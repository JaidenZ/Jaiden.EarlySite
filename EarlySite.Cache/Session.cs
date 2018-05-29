namespace EarlySite.Cache
{
    using Jaiden.Redis;
    using EarlySite.Core.Component;
    using EarlySite.Core.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 服务器会话状态
    /// </summary>
    public partial class Session : IEnumerable
    {
        private RedisKeyValue redis = null;

        internal Session()
        {
            redis = new RedisKeyValue();
        }

        /// <summary>
        /// 获取内部操作的REDIS键值对访问
        /// </summary>
        public RedisKeyValue Redis
        {
            get
            {
                return redis;
            }
        }
        /// <summary>
        /// 获取与键关联的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class, new()
        {
            string message = this.Get(key);
            if (string.IsNullOrEmpty(message))
            {
                return default(T);
            }
            return StringFormatter.Deserialize<T>(message);
        }
        /// <summary>
        /// 获取与键关联的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return redis.Get<string>(key);
        }
        /// <summary>
        /// 获取与键相关的数据流
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] GetBytes(string key)
        {
            return redis.Get<byte[]>(key);
        }

        /// <summary>
        /// 检查Key是否已存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return redis.Contains(key);
        }

        /// <summary>
        /// 设置与键关联的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Set(string key, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            string message = null;
            if (typeof(string).IsInstanceOfType(value))
            {
                message = Convert.ToString(value);
            }
            else
            {
                message = StringFormatter.Serialize(value);
            }
            return redis.Set(key, message);
        }

        /// <summary>
        /// 在特定键上增加其值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public long Increment(string key)
        {
            return redis.IncrementValue(key);
        }

        /// <summary>
        /// 在特定键上增加其值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="num">递加的值</param>
        /// <returns></returns>
        public long Increment(string key, int num)
        {
            return redis.IncrementValue(key, num);
        }

        /// <summary>
        /// 在特定键上增加其值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="num">递加的值</param>
        /// <returns></returns>
        public double Increment(string key, double num)
        {
            return redis.IncrementValue(key, num);
        }

        /// <summary>
        /// 在特定键上减少其值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public long Decrement(string key)
        {
            return redis.DecrementValue(key);
        }

        /// <summary>
        /// 在特定键上递减其值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="num">递减的值</param>
        /// <returns></returns>
        public long Decrement(string key, int num)
        {
            return redis.DecrementValue(key, num);
        }

        /// <summary>
        /// 设置键的过期时间
        /// </summary>
        public bool Expire(string key, DateTime expire)
        {
            return redis.Expire(key, expire);
        }
        /// <summary>
        /// 设置键的过期时间
        /// </summary>
        public bool Expire(string key, TimeSpan expire)
        {
            return redis.Expire(key, expire);
        }
        /// <summary>
        /// 删除与键关联的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return redis.Remove(key);
        }
        /// <summary>
        /// 删除与键关联的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public void Remove(IEnumerable<string> keys)
        {
            redis.Remove(keys);
        }
        /// <summary>
        /// 删除全部的键与值
        /// </summary>
        public void RemoveAll()
        {
            redis.RemoveAll();
        }

        /// <summary>
        /// 扫描符合条件的键名迭代器
        /// </summary>
        /// <param name="pattern">通配符表达式（语法：A-*、表示匹配前面包含A-的任意键）</param>
        /// <returns></returns>
        public IList<string> ScanAllKeys(string pattern)
        {
            return redis.ScanAllKeys(pattern);
        }

        /// <summary>
        /// 获取在缓存服务器中所有存在的Key值
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllKeys()
        {
            return redis.GetAllKeys();
        }

        /// <summary>
        /// 持久化保存缓存
        /// </summary>
        public void SaveAsync()
        {
            RedisClientManager.SaveAsync();
        }

        private static object g_pLook = new object();
        private const string WORK_KEY_NAME = "Session";

        public static Session Deployment()
        {
            lock (g_pLook)
            {
                WorkThreadDictionary work = WorkThreadDictionary.Get();
                Session session = work.Get<Session>(WORK_KEY_NAME);
                if (session == null)
                {
                    session = new Session();
                    work.Set(WORK_KEY_NAME, session);
                }
                return session;
            }
        }

        public static Session Current
        {
            get
            {
                if(_deploymentStyle == 0)
                {
                    return Session.Deployment();
                }
                else if(_deploymentStyle == 1)
                {
                    return _sessionP;
                }
                return null;
            }
        }

        private static Session _sessionP = null;
        private static int _deploymentStyle = 0;
        
        public static Session DeploymentForWeb()
        {
            if(_sessionP == null)
            {
                _sessionP = new Session();
                _deploymentStyle = 1;
            }
            return _sessionP;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            IList<string> keys = redis.GetAllKeys();
            foreach (string key in keys)
            {
                yield return key;
            }
        }

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, T> GetAll<T>(IList<string> keys) where T : class, new()
        {
            if (keys == null || keys.Count == 0)
            {
                return null;
            }

            IDictionary<string, T> dic = new Dictionary<string, T>();
            IDictionary<string, string> data = redis.GetAll<string>(keys);
            if (data == null)
            {
                return null;
            }
            foreach (var item in data)
            {
                if (item.Value != null)
                {
                    dic.Add(item.Key, StringFormatter.Deserialize<T>(item.Value));
                }
                else
                {
                    dic.Add(item.Key, default(T));
                }
            }

            return dic;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        public void SetAll<T>(IDictionary<string, T> map)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            foreach (var item in map)
            {
                data.Add(item.Key, StringFormatter.Serialize(item.Value));
            }

            redis.SetAll(data);
        }
    }
}
