namespace Jaiden.Redis
{
    using EarlySite.Core.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// 消息队列服务
    /// </summary>
    public sealed class RedisQueueService
    {
        private IList<MqSubscribeContext> m_contexts = null;
        private Thread m_thrWorkMQ = null;
        private RedisQueue m_workReadMQ = null;
        private RedisQueue m_workWriteMQ = null;
        private bool m_exitWorkMQ = true;

        /// <summary>
        /// 消息队列订阅上下文
        /// </summary>
        private sealed class MqSubscribeContext
        {
            public Action<string> EventHandler { get; set; }
            public Type EventType { get; set; }
            public string MqChanels { get; set; }
        }

        internal RedisQueueService()
        {
            m_contexts = new List<MqSubscribeContext>();
            m_thrWorkMQ = new Thread(() =>
            {
                while (true)
                {
                    if (m_exitWorkMQ)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        bool untreated = false; // 未处理过
                        for (int i = 0; i < m_contexts.Count; i++)
                        {
                            MqSubscribeContext context = m_contexts[i];
                            m_workReadMQ.Chanels = context.MqChanels;
                            string message = m_workReadMQ.Dequeue();
                            if (message != null)
                            {
                                untreated = true;
                                try
                                {
                                    Action<string> d = context.EventHandler;
                                    d(message);
                                }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
                                catch(Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
                                {
                                    continue;
                                }
                            }
                        }
                        if (!untreated) Thread.Sleep(1);
                    }
                }
            })
            { IsBackground = true, Priority = ThreadPriority.Lowest };
            m_thrWorkMQ.Start();
            m_workReadMQ = new RedisQueue(typeof(RedisQueueService).Name);
            m_workWriteMQ = new RedisQueue(typeof(RedisQueueService).Name);
        }

        /// <summary>
        /// 是否运行中
        /// </summary>
        public bool Runing
        {
            get
            {
                return m_exitWorkMQ;
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            m_exitWorkMQ = false;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            m_exitWorkMQ = true;
        }

        /// <summary>
        /// 订阅消息队列
        /// </summary>
        /// <param name="chanels">消息队列</param>
        public void Subscribe<T>(string chanels, Action<T> handler) where T : class, new()
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            if (string.IsNullOrEmpty(chanels))
            {
                throw new ArgumentException("chanels");
            }
            lock (m_contexts)
            {
                if (m_contexts.FirstOrDefault((i) => i.MqChanels == chanels) != null)
                {
                    throw new ArgumentException("repeat");
                }
                MqSubscribeContext context = new MqSubscribeContext
                {
                    EventHandler = (message) =>
                    {
                        T value = null;
                        if (typeof(string) != typeof(T))
                        {
                            if (!string.IsNullOrEmpty(message))
                            {
                                value = StringFormatter.Deserialize<T>(message);
                            }
                        }
                        else
                        {
                            object obj = message;
                            value = (T)obj;
                        }
                        handler(value);
                    },
                    EventType = typeof(T),
                    MqChanels = chanels
                };
                m_contexts.Add(context);
            }
        }

        /// <summary>
        /// 发布消息到队列
        /// </summary>
        /// <param name="chanels">通信管道</param>
        /// <param name="message">发送的消息</param>
        public void Publish(string chanels, string message)
        {
            if (string.IsNullOrEmpty(chanels))
            {
                throw new ArgumentException("chanels");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("message");
            }
            lock (m_workWriteMQ)
            {
                m_workWriteMQ.Chanels = chanels;
                m_workWriteMQ.Enqueue(message);
            }
        }

        /// <summary>
        /// 发送消息到队列
        /// </summary>
        /// <param name="chanels">通信管道</param>
        /// <param name="value">发送的消息</param>
        public void Publish(string chanels, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            string message = StringFormatter.Serialize(value);
            this.Publish(chanels, message);
        }
    }
}
