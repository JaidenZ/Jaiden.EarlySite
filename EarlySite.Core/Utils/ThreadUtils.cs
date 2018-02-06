namespace EarlySite.Core.Utils
{
    using Component;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;

    public static class ThreadUtils
    {
        private static Func<Thread, object> g_pGetObject = null;
        private static object g_pLookGetObj = new object();
        private static Action<Thread, object> g_pSetObject = null;
        private static object g_pLookSetObj = new object();

        private static FieldInfo GetObjectField()
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type clazz = typeof(Thread);
            return clazz.GetField("m_ThreadStartArg", flags);
        }

        public static object GetObject()
        {
            Thread g_pThread = Thread.CurrentThread;
            return ThreadUtils.GetObject(g_pThread);
        }

        public static T GetObject<T>(this Thread key)
        {
            object g_pState = ThreadUtils.GetObject(key);
            if (g_pState == null)
            {
                return default(T);
            }
            return (T)g_pState;
        }

        public static T GetObject<T>()
        {
            Thread g_pThread = Thread.CurrentThread;
            return ThreadUtils.GetObject<T>(g_pThread);
        }

        public static object GetObject(this Thread key)
        {
            Contract.Requires<ArgumentNullException>(key != null);
            lock (g_pLookGetObj)
            {
                if (g_pGetObject == null)
                {
                    ParameterExpression g_pThread = Expression.Parameter(typeof(Thread));
                    FieldInfo g_pField = ThreadUtils.GetObjectField();
                    g_pGetObject = Expression.Lambda<Func<Thread, object>>(Expression.Field(g_pThread, g_pField), g_pThread).Compile();
                }
            }
            return g_pGetObject(key);
        }

        public static void SetObject(object value)
        {
            Thread g_pThread = Thread.CurrentThread;
            ThreadUtils.SetObject(g_pThread, value);
        }

        public static void SetObject(this Thread key, object value)
        {
            Contract.Requires<ArgumentNullException>(key != null);
            lock (g_pLookSetObj)
            {
                if (g_pSetObject == null)
                {
                    ParameterExpression g_pThread = Expression.Parameter(typeof(Thread));
                    FieldInfo g_pField = ThreadUtils.GetObjectField();
                    ParameterExpression g_pState = Expression.Parameter(typeof(object));
                    g_pSetObject = Expression.Lambda<Action<Thread, object>>(Expression.Assign(Expression.Field(g_pThread, g_pField), g_pState),
                        g_pThread, g_pState).Compile();
                }
            }
            g_pSetObject(key, value);
        }
    }
}
