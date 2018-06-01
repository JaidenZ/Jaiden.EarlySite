namespace EarlySite.Core.DDD.Service
{
    using EarlySite.Core.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    abstract class IoCContainerBase<IoC> where IoC : class
    {
        public IoCContainerBase()
        {
            Container = new List<IoC>();
        }

        protected IList<IoC> Container
        {
            get;
            private set;
        }

        public virtual void Load(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            try
            {
                Type service = typeof(IoC);
                foreach (Type clazz in assembly.GetExportedTypes())
                {
                    if (!clazz.IsAbstract && !clazz.IsInterface && service.IsAssignableFrom(clazz))
                    {
                        IoC value = Resolve(clazz, false) as IoC;
                        if (value != null)
                        {
                            Container.Add(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual object Resolve(Type clazz, bool alloc, params Type[] exceptive)
        {
            if (clazz != null)
            {
                if (!alloc && Container.FirstOrDefault((s) => clazz.IsInstanceOfType(s)) != null)
                {
                    return null;
                }
                if (clazz.IsInterface)
                {
                    var assembly = clazz.Assembly;
                    var types = assembly.GetExportedTypes();
                    return Resolve(types.FirstOrDefault((i) => !i.IsInterface && clazz.IsAssignableFrom(i)), alloc);
                }
                else
                {
                    var ctors = clazz.GetConstructors();
                    if (ctors.Length > 0)
                    {
                        var service = typeof(IoC);
                        for (int i = 0; i < ctors.Length; i++)
                        {
                            var ctor = ctors[i];
                            var args = ctor.GetParameters();
                            var success = true;
                            for (int j = 0; j < args.Length; j++)
                            {
                                if (!ListUtils.IsNullOrEmpty<Type>(exceptive))
                                {
                                    success = exceptive.FirstOrDefault((s) => s.IsAssignableFrom(args[j].ParameterType)) != null;
                                }
                                else
                                {
                                    success = service.IsAssignableFrom(args[j].ParameterType);
                                }
                                if (!success) break;
                            }
                            if (success)
                            {
                                return Resolve(ctor, alloc);
                            }
                        }
                    }
                }
            }
            return null;
        }

        protected virtual object Resolve(ConstructorInfo ctor, bool alloc, IDictionary<Type, object> exceptive = null)
        {
            var args = ctor.GetParameters();
            var values = new object[args.Length];
            for (int j = 0; j < args.Length; j++)
            {
                var clazz = args[j].ParameterType;
                var type = Container.FirstOrDefault((s) => clazz.IsInstanceOfType(s));
                //
                if (!alloc && type != null)
                {
                    values[j] = type;
                }
                else
                {
                    if (exceptive != null)
                    {
                        KeyValuePair<Type, object> pair = exceptive.FirstOrDefault((e) => e.Key == clazz);
                        if (pair.Value != null)
                        {
                            values[j] = pair.Value;
                        }
                    }
                    if (values[j] == null)
                    {
                        values[j] = Resolve(clazz, alloc);
                        var service = values[j] as IoC;
                        if (!alloc && service != null)
                        {
                            Container.Add(service);
                        }
                    }
                }
            }
            return ctor.Invoke(values);
        }
        /// <summary>
        /// 从容器中获取一个持久化的对象（效率高）
        /// </summary>
        /// <typeparam name="T">检索的接口</typeparam>
        /// <returns></returns>
        public virtual T Get<T>() where T : class
        {
            Type clazz = typeof(T);
            return this.Get(clazz) as T;
        }

        public virtual object Get(Type clazz)
        {
            if (clazz == null)
            {
                throw new ArgumentNullException();
            }
            lock (Container)
            {
                IoC s = Container.FirstOrDefault((i) => clazz.IsInstanceOfType(i));
                if (s == null)
                {
                    return null;
                }
                return s;
            }
        }
        /// <summary>
        /// 分配一个有效的对象（效率低）
        /// </summary>
        /// <typeparam name="T">检索的接口</typeparam>
        /// <returns></returns>
        public virtual T New<T>() where T : class
        {
            object o = New(typeof(T));
            if (o == null)
            {
                return null;
            }
            return (T)o;
        }

        public virtual object New(Type clazz)
        {
            if (clazz == null)
            {
                throw new ArgumentNullException();
            }
            return Resolve(clazz, true);
        }
    }
}
