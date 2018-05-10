namespace EarlySite.Core.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static partial class WebSerializer
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IList<Type> m_mapTabType = new Type[] { typeof(double), typeof(float), typeof(long), typeof(ulong), typeof(byte),
            typeof(sbyte), typeof(short), typeof(ushort), typeof(char), typeof(int), typeof(uint), typeof(bool),
            typeof(DateTime),typeof(string) };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Encoding m_strEncoding = Encoding.Default;
    }

    public static partial class WebSerializer
    {
        /// <summary>
        /// 是否是基础类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsBasicsType(Type type)
        {
            return m_mapTabType.Contains(type);
        }

        /// <summary>
        /// 获取集合类型
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static Type GetArrayElmentType(Type array)
        {
            if (array.IsArray)
                return array.GetElementType();
            if (array.IsGenericType && (typeof(IList<>).GUID == array.GUID || typeof(List<>).GUID == array.GUID))
                return array.GetGenericArguments()[0];
            return null;
        }


        /// <summary>
        /// 获取类型标识
        /// </summary>
        /// <param name="type"></param>
        /// <returns> 
        /// 0x00 标准分隔符
        /// 0x01 基础类型数组
        /// 0x02 对象类型数组
        /// 0x03 对象
        /// </returns>
        private static string GetTypeStr(Type type)
        {
            if (IsBasicsType(type))
            {
                return "\x00";
            }

            Type elmenttype = GetArrayElmentType(type);
            if (elmenttype != null)
            {
                if (IsBasicsType(elmenttype))
                {
                    return "\x01";
                }

                return "\x02";
            }
            else
            {
                return "\x03";
            }
        }
    }

    public static partial class WebSerializer
    {
        public static string Serialize(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            Type objtype = obj.GetType();
            return Serialize(objtype, obj);
        }


        public static string Serialize(Type type, object obj)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            StringBuilder sb = new StringBuilder();
            PropertyInfo[] parray = type.GetProperties();
            for (int i = 0; i < parray.Length; i++)
            {
                PropertyInfo pinfo = parray[i];
                object pobjvalue = pinfo.GetValue(obj, null);
                if (pobjvalue == null)
                {
                    continue;
                }

                string typestr = GetTypeStr(pinfo.PropertyType);
                string valuestr = string.Empty;
                if (typestr == "\x00")
                {
                    valuestr = GetBasicsTypeData(pobjvalue, pinfo.PropertyType);
                }
                else if (typestr == "\x01")
                {
                    valuestr = GetBasicsArryayData(pobjvalue, pinfo.PropertyType);
                }
                else if (typestr == "\x02")
                {
                    valuestr = GetObjArryayData(pobjvalue, pinfo.PropertyType);
                }
                else if (typestr == "\x03")
                {
                    valuestr = GetObjTypeData(pobjvalue, pinfo.PropertyType);
                }
                else
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(valuestr))
                {
                    sb.Append(pinfo.Name);
                    if (typestr != "\x00")
                    {
                        sb.Append("\x00");
                    }

                    sb.Append(typestr);
                    sb.Append(valuestr);
                    if (i != parray.Length - 1)
                    {
                        sb.Append("\x00");
                    }
                }
            }

            if (sb[sb.Length - 1] == '\x00')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string GetObjTypeData(object value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string objstr = Serialize(type, value);

            return string.Format("\x00{0}\x00{1}", objstr.Split(new string[] { "\x00" }, StringSplitOptions.None).Length, objstr);

        }

        /// <summary>
        /// 获取基础类型的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string GetBasicsTypeData(object value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return ObjToValue(type, value);
        }

        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ObjToValue(Type type, object value)
        {
            if (value != null)
            {
                if (type == typeof(DateTime))
                    return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss:fff");
                if (IsBasicsType(type))
                    return value.ToString();
            }

            return null;
        }

        /// <summary>
        /// 获取基础类型数组数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string GetBasicsArryayData(object value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            StringBuilder sb = new StringBuilder();
            IList array = value as IList;
            if (array != null && array.Count > 0)
            {
                Type elmenttype = GetArrayElmentType(type);
                sb.Append("\x00");
                sb.Append(array.Count);
                sb.Append("\x00");
                for (int i = 0; i < array.Count; i++)
                {
                    sb.Append(ObjToValue(elmenttype, array[i]));
                    if (i != array.Count - 1)
                    {
                        sb.Append("\x00");
                    }
                }
            }

            return sb.ToString();
        }

        private static string GetObjArryayData(object obj, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            StringBuilder sb = new StringBuilder();
            IList array = obj as IList;
            if (array != null && array.Count > 0)
            {
                Type elmenttype = GetArrayElmentType(type);
                StringBuilder tempsb = new StringBuilder();
                string oneobjstr = string.Empty;

                for (int i = 0; i < array.Count; i++)
                {
                    oneobjstr = Serialize(elmenttype, array[i]);
                    tempsb.Append(oneobjstr.Split(new string[] { "\x00" }, StringSplitOptions.None).Length);
                    tempsb.Append("\x00");
                    tempsb.Append(oneobjstr);
                    if (i != array.Count - 1)
                    {
                        tempsb.Append("\x00");
                    }
                }

                //foreach (var item in array)
                //{
                //    oneobjstr = Serialize(elmenttype, item);
                //    tempsb.Append(oneobjstr.Split(new string[] { "\x00" }, StringSplitOptions.RemoveEmptyEntries).Length);
                //    tempsb.Append("\x00");
                //    tempsb.Append(oneobjstr);
                //    tempsb.Append("\x00");
                //}

                string objstr = tempsb.ToString();
                sb.Append("\x00");
                sb.Append(objstr.Split(new string[] { "\x00" }, StringSplitOptions.None).Length);
                sb.Append("\x00");
                sb.Append(objstr);
            }

            return sb.ToString();
        }
    }

    public static partial class WebSerializer
    {

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string str) where T : class, new()
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            Type type = typeof(T);
            string[] alldatas = str.Split(new string[] { "\x00" }, StringSplitOptions.None);
            object obj = BuildObj(type, alldatas);
            return (T)obj;
        }

        /// <summary>
        /// 构建对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        public static object BuildObj(Type type, IList<string> dataarray)
        {
            object obj = Activator.CreateInstance(type);
            List<PropertyInfo> plist = type.GetProperties().ToList();
            for (int i = 0; i < dataarray.Count;)
            {
                PropertyInfo pone = plist.FirstOrDefault(x => x.Name == dataarray[i]);
                if (pone == null)
                {
                    i++;
                    continue;
                }

                string nextdata = dataarray[i + 1];
                if (IsBasicTypeStr(nextdata))
                {
                    SetBasicTypeValue(obj, pone, nextdata);
                    i += 2;
                }
                else
                {
                    int startindex = i + 3;
                    int length = Convert.ToInt32(dataarray[i + 2]);
                    IList<string> tempdataarray = dataarray.Skip(startindex).Take(length).ToList();
                    if (nextdata == "\x01")
                    {
                        SetBasicArrayTypeValue(obj, pone, tempdataarray);
                    }
                    else if (nextdata == "\x02")
                    {
                        SetObjArrayTypeValue(obj, pone, tempdataarray);
                    }
                    else if (nextdata == "\x03")
                    {
                        SetObjValue(obj, pone, tempdataarray);
                    }

                    i = startindex + length;
                }
            }

            return obj;
        }

        /// <summary>
        /// 是否是基础类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsBasicTypeStr(string type)
        {
            return type != "\x01" && type != "\x02" && type != "\x03";
        }

        /// <summary>
        /// 赋值对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <param name="dataarray"></param>
        private static void SetObjValue(object obj, PropertyInfo p, IList<string> dataarray)
        {
            p.SetValue(obj, BuildObj(p.PropertyType, dataarray), null);
        }

        /// <summary>
        /// 赋值基础数据类型的值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="p">字段</param>
        /// <param name="value">值</param>
        private static void SetBasicTypeValue(object obj, PropertyInfo p, string value)
        {
            p.SetValue(obj, GetValue(p.PropertyType, value), null);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object GetValue(Type type, string value)
        {
            if (type == typeof(int))
            {
                return Convert.ToInt32(value);
            }
            if (type == typeof(uint))
            {
                return Convert.ToUInt32(value);
            }
            if (type == typeof(long))
            {
                return Convert.ToInt64(value);
            }
            if (type == typeof(ulong))
            {
                return Convert.ToUInt64(value);
            }
            if (type == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }
            if (type == typeof(byte))
            {
                return Convert.ToByte(value);
            }
            if (type == typeof(sbyte))
            {
                return Convert.ToSByte(value);
            }
            if (type == typeof(short))
            {
                return Convert.ToInt16(value);
            }
            if (type == typeof(ushort))
            {
                return Convert.ToUInt16(value);
            }
            if (type == typeof(string))
            {
                return value;
            }
            if (type == typeof(DateTime))
            {
                return Convert.ToDateTime(value);
            }
            if (type == typeof(double))
            {
                return Convert.ToDouble(value);
            }
            if (type == typeof(float))
            {
                return Convert.ToSingle(value);
            }
            if (type == typeof(char))
            {
                return Convert.ToChar(value);
            }

            return type;
        }

        /// <summary>
        /// 构建基础数据数组
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        private static object BuildBasicArrayTypeValue(PropertyInfo p, IList<string> dataarray)
        {
            Type elmenttype = GetArrayElmentType(p.PropertyType);
            if (p.PropertyType.IsArray)
            {
                return BuildArray(elmenttype, dataarray);
            }
            else if (p.PropertyType.IsGenericType)
            {
                return BuildList(elmenttype, dataarray);
            }

            return null;
        }

        /// <summary>
        /// 构建数组
        /// </summary>
        /// <param name="elmentType"></param>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        private static object BuildArray(Type elmentType, IList<string> dataarray)
        {
            ArrayList list = new ArrayList();
            foreach (var item in dataarray)
            {
                list.Add(GetValue(elmentType, item));
            }

            return list.ToArray(elmentType);
        }

        /// <summary>
        /// 构建集合
        /// </summary>
        /// <param name="elmentType"></param>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        private static object BuildList(Type elmentType, IList<string> dataarray)
        {
            Type listtype = typeof(List<>).MakeGenericType(elmentType);
            var list = Activator.CreateInstance(listtype);
            MethodInfo minfo = listtype.GetMethod("Add");
            foreach (var item in dataarray)
            {
                minfo.Invoke(list, new object[] { GetValue(elmentType, item) });
            }

            return list;
        }

        /// <summary>
        /// 设置基础类型数组
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <param name="dataarray"></param>
        private static void SetBasicArrayTypeValue(object obj, PropertyInfo p, IList<string> dataarray)
        {
            p.SetValue(obj, BuildBasicArrayTypeValue(p, dataarray), null);
        }

        /// <summary>
        /// 设置对象类型数组
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <param name="dataarray"></param>
        private static void SetObjArrayTypeValue(object obj, PropertyInfo p, IList<string> dataarray)
        {
            p.SetValue(obj, BuildObjArrayTypeValue(p, dataarray), null);
        }


        /// <summary>
        /// 构建基础数据数组
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        private static object BuildObjArrayTypeValue(PropertyInfo p, IList<string> dataarray)
        {
            Type elmenttype = GetArrayElmentType(p.PropertyType);
            if (p.PropertyType.IsArray)
            {
                return BuildObjArray(elmenttype, dataarray);
            }
            else if (p.PropertyType.IsGenericType)
            {
                return BuildObjList(elmenttype, dataarray);
            }

            return null;
        }

        /// <summary>
        /// 构建对象数组
        /// </summary>
        /// <param name="elmentType"></param>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        private static object BuildObjArray(Type elmentType, IList<string> dataarray)
        {
            ArrayList list = new ArrayList();
            foreach (var item in dataarray)
            {
                list.Add(BuildObj(elmentType, dataarray));
            }

            return list.ToArray(elmentType);
        }

        /// <summary>
        /// 构建对象集合
        /// </summary>
        /// <param name="elmentType"></param>
        /// <param name="dataarray"></param>
        /// <returns></returns>
        private static object BuildObjList(Type elmentType, IList<string> dataarray)
        {
            Type listtype = typeof(List<>).MakeGenericType(elmentType);
            var list = Activator.CreateInstance(listtype);
            MethodInfo minfo = listtype.GetMethod("Add");
            for (int i = 0; i < dataarray.Count;)
            {
                int length = Convert.ToInt32(dataarray[i]);
                minfo.Invoke(list, new object[] { BuildObj(elmentType, dataarray.Skip(i + 1).Take(Convert.ToInt32(length)).ToList()) });
                i += length + 1;
            }

            return list;
        }
    }
}