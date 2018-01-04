namespace EarlySite.Core.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 安全集合(带锁)
    /// </summary>
    public class SafetyList<T> : IList<T>
    {
        private object g_lock = new object();
        private IList<T> g_buffer = null;

        public SafetyList()
        {
            g_buffer = new List<T>();
        }

        private SafetyList(IList<T> list)
        {
            g_buffer = list;
        }

        public SafetyList(int capacity)
        {
            g_buffer = new List<T>(capacity);
        }

        public T this[int index]
        {
            get
            {
                lock (g_lock)
                    return g_buffer[index];
            }
            set
            {
                lock (g_lock)
                    g_buffer[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return g_buffer.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return g_buffer.IsReadOnly;
            }
        }

        public void Add(T item)
        {
            lock (g_lock)
                g_buffer.Add(item);
        }

        public void Clear()
        {
            lock (g_lock)
                g_buffer.Clear();
        }

        public bool Contains(T item)
        {
            lock (g_lock)
                return g_buffer.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (g_lock)
                g_buffer.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            //lock (g_lock)
            for (int i = 0; i < g_buffer.Count; i++)
                yield return g_buffer[i];
        }

        public int IndexOf(T item)
        {
            lock (g_lock)
                return g_buffer.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            lock (g_lock)
                g_buffer.Insert(index, item);
        }

        public bool Remove(T item)
        {
            lock (g_lock)
                return g_buffer.Remove(item);
        }

        public void RemoveAt(int index)
        {
            lock (g_lock)
                g_buffer.RemoveAt(index);
        }

        public void AddRange(SafetyList<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            lock (g_lock)
                foreach (var item in collection)
                {
                    g_buffer.Add(item);
                }
        }

        public static SafetyList<T> Wrapper(IList<T> value)
        {
            return new SafetyList<T>(value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            int i = 0;
            while (true)
            {
                T value = default(T);
                lock (g_lock)
                {
                    if (i >= g_buffer.Count)
                        break;
                    else
                        value = g_buffer[i];
                }
                i++;
                yield return value;
            }
        }
    }
}
