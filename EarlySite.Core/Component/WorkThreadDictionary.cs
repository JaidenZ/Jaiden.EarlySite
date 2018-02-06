namespace EarlySite.Core.Component
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Runtime.InteropServices;
    using Utils;

    [DebuggerNonUserCode, DebuggerDisplay("null"), TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FRestricted)]
    public sealed class WorkThreadDictionary : IEnumerable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IList<object> g_pValues = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IList<string> g_pKeys = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object g_pLook = new object();

        private WorkThreadDictionary(Thread thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null);
            g_pValues = new List<object>();
            g_pKeys = new List<string>();
            ThreadUtils.SetObject(thread, this);
        }

        public bool ContainsKey(string key)
        {
            lock (g_pLook)
            {
                return g_pKeys.Contains(key);
            }
        }

        public object Get(string key)
        {
            lock (g_pLook)
            {
                int index = g_pKeys.IndexOf(key);
                if (index < 0)
                {
                    return null;
                }
                return g_pValues[index];
            }
        }

        public T Get<T>(string key)
        {
            lock (g_pLook)
            {
                object value = this.Get(key);
                if (value == null)
                {
                    return default(T);
                }
                return (T)value;
            }
        }

        public void Set(string key, object value)
        {
            lock (g_pLook)
            {
                int index = g_pKeys.IndexOf(key);
                if (index < 0)
                {
                    g_pKeys.Add(key);
                    g_pValues.Add(value);
                }
                else
                {
                    g_pValues[index] = value;
                }
            }
        }

        public string[] GetAllKeys()
        {
            string[] keys = new string[g_pKeys.Count];
            g_pKeys.CopyTo(keys, 0);
            return keys;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < g_pKeys.Count; i++)
            {
                yield return new KeyValuePair<string, object>(g_pKeys[i], g_pValues[i]);
            }
        }

        public static WorkThreadDictionary Create(Thread thread)
        {
            try
            {
                Contract.Requires<ArgumentNullException>(thread != null);
                return new WorkThreadDictionary(thread);
            }
            catch
            {
                return null;
            }
        }

        public static WorkThreadDictionary Get(Thread thread)
        {
            Contract.Requires<ArgumentNullException>(thread != null);
            object value = ThreadUtils.GetObject(thread);
            if (value == null || !(value is WorkThreadDictionary))
            {
                return null;
            }
            return (WorkThreadDictionary)value;
        }

        public static WorkThreadDictionary Get()
        {
            return WorkThreadDictionary.Get(Thread.CurrentThread);
        }
    }

}
