namespace EarlySite.Core.Data
{
    using Core.ValueType;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Reflection;

    public static partial class DataModelConverter
    {

        /// <summary>
        /// 将 DataSet 转换成数据模型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="value">DataSet</param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataSet value) where T : class
        {
            if (value == null || value.Tables.Count <= 0)
            {
                throw new ArgumentException();
            }
            return DataModelConverter.ToList<T>(value.Tables[0]);
        }

        /// <summary>
        /// 将 DataTable 转换成数据模型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="value">DataSet</param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable value) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            IList<KeyValuePair<PropertyInfo, int>> properties = new List<KeyValuePair<PropertyInfo, int>>();
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                foreach (DataColumn cols in value.Columns)
                {
                    if ((cols.ColumnName).ToUpper() == (property.Name).ToUpper())
                    {
                        properties.Add(new KeyValuePair<PropertyInfo, int>(property, cols.Ordinal));
                    }
                }
            }
            var ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            IList<T> buffer = new List<T>();
            foreach (DataRow row in value.Rows)
            {
                T model = (T)ctor.Invoke(null);
                try
                {
                    buffer.Add(model);
                }
                finally
                {
                    foreach (KeyValuePair<PropertyInfo, int> pair in properties)
                    {
                        object args = row[pair.Value];
                        if (args == DBNull.Value)
                        {
                            args = null;
                        }
                        Type prop = pair.Key.PropertyType;
                        if (args != null && !prop.IsInstanceOfType(args))
                        {
                            if (prop.IsEnum)
                            {
                                prop = prop.GetEnumUnderlyingType();
                            }
                            if (ValueTypeUtils.IsFloatType(prop))
                            {
                                args = ValueTypeFormatter.Parse(Convert.ToString(args), prop, NumberStyles.Number | NumberStyles.Float);
                            }
                            else
                            {
                                args = ValueTypeFormatter.Parse(Convert.ToString(args), prop);
                            }
                        }
                        if(pair.Key.SetMethod != null)
                        {
                            pair.Key.SetValue(model, args, null);
                        }
                        
                    }
                }
            }
            return buffer;
        }
    }
}
