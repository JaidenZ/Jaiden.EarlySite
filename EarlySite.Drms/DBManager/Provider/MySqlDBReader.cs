namespace EarlySite.Drms.DBManager.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using EarlySite.Core.AOP.Data;
    using EarlySite.Core.Data;
    using EarlySite.Core.Utils;
    using EarlySite.Drms.DBManager.Connection;

    public class MySqlDBReader
    {
        private IList<T> ToList<T>(DataTable table) where T : class
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            DataRowCollection rows = table.Rows;
            if (rows.Count <= 0)
            {
                return new List<T>(0);
            }
            if (typeof(T) == typeof(object))
            {
                DataModelProxyConverter proxy = DataModelProxyConverter.GetInstance();
                return (IList<T>)proxy.ToList(table);
            }
            else
            {
                return DataModelConverter.ToList<T>(table);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public DataTable Select(MySql.Data.MySqlClient.MySqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            cmd.Connection = MysqlConnection.Current;
            lock (cmd.Connection)
            {
                try
                {
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                    
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">映射类型</typeparam>
        /// <param name="cmd">命令行</param>
        /// <returns></returns>
        public IList<T> Select<T>(MySql.Data.MySqlClient.MySqlCommand cmd) where T : class
        {
            using (DataTable dt = this.Select(cmd))
            {
                return this.ToList<T>(dt);
            }
        }
    }
}
