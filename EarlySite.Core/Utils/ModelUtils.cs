namespace EarlySite.Core.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Linq;

    public static class ModelUnit
    {
        public static T Copy<T>(this object value) where T : class, new()
        {
            if (value == null)
            {
                return null;
                //throw new ArgumentNullException("value");
            }
            Type clazz = value.GetType();
            T obj = new T();
            Type type = typeof(T);
            foreach (PropertyInfo pi in clazz.GetProperties())
            {
                PropertyInfo pp = type.GetProperty(pi.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pp != null && !pp.PropertyType.IsGenericType)
                {
                    object val = pi.GetValue(value, null);
                    if(pp.SetMethod != null)
                    {
                        pp.SetValue(obj, val, null);
                    }
                }
            }
            return obj;
        }

        public static T Copy<T>(this object value, Type objtype) where T : class, new()
        {
            if (value == null)
            {
                return null;
                //throw new ArgumentNullException("value");
            }

            var obj = Activator.CreateInstance(objtype);
            foreach (PropertyInfo pi in objtype.GetProperties())
            {
                PropertyInfo pp = objtype.GetProperty(pi.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pp != null && !pp.PropertyType.IsGenericType)
                {
                    object val = pi.GetValue(value, null);
                    if(pp.SetMethod != null)
                    {
                        pp.SetValue(obj, val, null);
                    }
                }
            }
            return (T)obj;
        }

        public static List<O> CopyList<I, O>(this IList<I> value) where O : new()
            where I : new()
        {
            List<O> list = new List<O>();

            if (value == null || !value.Any())
            {
                return list;
            }

            Type clazz = value[0].GetType();

            foreach (var item in value)
            {
                O obj = new O();
                Type type = typeof(O);
                foreach (PropertyInfo pi in clazz.GetProperties())
                {
                    var pp = type.GetProperty(pi.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (pp != null)
                    {
                        object val = pi.GetValue(item, null);
                        if (pp.SetMethod != null)
                        {
                            pp.SetValue(obj, val, null);
                        }
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// 可copy集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T CopyAll<T>(this object value) where T : class, new()
        {
            if (value == null)
            {
                return default(T);
            }

            Type clazz = value.GetType();
            T obj = new T();
            Type type = typeof(T);
            foreach (PropertyInfo pi in clazz.GetProperties())
            {
                PropertyInfo pp = type.GetProperty(pi.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pp != null)
                {
                    object val = null;

                    val = pi.GetValue(value, null);
                    if (val != null)
                    {
                        if (pp.PropertyType.IsGenericType)
                        {
                            Type element = GetArrayElement(pp.PropertyType);
                            if (!element.IsValueType && element != typeof(string))
                            {
                                val = GetList(val, pi.PropertyType, pp.PropertyType);
                            }
                        }
                        else if (!pp.PropertyType.IsValueType && pp.PropertyType != typeof(string))
                        {
                            val = GetObj(val, pi.PropertyType, pp.PropertyType);
                        }
                        if (pp.SetMethod != null)
                        {
                            pp.SetValue(obj, val, null);
                        }
                    }

                }
            }
            return obj;
        }

        public static object GetList(object value, Type imodel, Type omodel)
        {
            if (value == null)
            {
                return null;
            }

            Type inelementtype = GetArrayElement(imodel);
            Type oelementtype = GetArrayElement(omodel);
            Type olisttype = typeof(List<>).MakeGenericType(oelementtype);

            var listobj = Activator.CreateInstance(olisttype);
            foreach (var item in (value as IEnumerable))
            {
                var outmodel = Activator.CreateInstance(oelementtype);
                foreach (var pi in inelementtype.GetProperties())
                {
                    PropertyInfo pp = oelementtype.GetProperty(pi.Name,BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (pp != null)
                    {
                        object val = pi.GetValue(item, null);
                        if (pp.PropertyType.IsGenericType)
                        {
                            Type element = GetArrayElement(pp.PropertyType);
                            if (!element.IsValueType && element != typeof(string))
                            {
                                val = GetList(val, pi.PropertyType, pp.PropertyType);
                            }
                        }
                        else if (!pp.PropertyType.IsValueType && pp.PropertyType != typeof(string))
                        {
                            val = GetObj(val, pi.PropertyType, pp.PropertyType);
                        }

                        if (pp.SetMethod != null)
                        {
                            pp.SetValue(outmodel, val, null);
                        }

                        
                    }
                }

                olisttype.GetMethod("Add").Invoke(listobj, new object[] { outmodel });
            }

            return listobj;
        }

        private static Type GetArrayElement(Type array)
        {
            if (array.IsArray)
            {

                return array.GetElementType();
            }
            if (array.IsGenericType && (typeof(IList<>).GUID == array.GUID || typeof(List<>).GUID == array.GUID))
            {
                Type[] args = array.GetGenericArguments();
                return args[0];
            }
            return null;
        }

        /// <summary>
        /// 获取对象信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="imodel"></param>
        /// <param name="omodel"></param>
        /// <returns></returns>
        private static object GetObj(object value, Type imodel, Type omodel)
        {
            if (value == null)
            {
                return null;
            }

            var outmodel = Activator.CreateInstance(omodel);
            List<PropertyInfo> imodelPropertyInfo = imodel.GetProperties().ToList();
            foreach (var item in imodel.GetProperties())
            {
                var pp = omodel.GetProperty(item.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pp != null)
                {
                    object val = item.GetValue(value, null);
                    if (pp.PropertyType.IsGenericType)
                    {
                        Type element = GetArrayElement(pp.PropertyType);
                        if (!element.IsValueType && element != typeof(string))
                        {
                            val = GetList(val, item.PropertyType, pp.PropertyType);
                        }
                    }
                    else if (!pp.PropertyType.IsValueType && pp.PropertyType != typeof(string))
                    {
                        val = GetObj(val, item.PropertyType, pp.PropertyType);
                    }

                    if (pp.SetMethod != null)
                    {
                        pp.SetValue(outmodel, val, null);
                    }
                }
            }

            return outmodel;
        }
    }
}
