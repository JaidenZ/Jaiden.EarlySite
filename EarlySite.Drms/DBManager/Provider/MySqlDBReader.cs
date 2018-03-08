namespace EarlySite.Drms.DBManager.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public DataTable Select(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql))
            {
                return this.Select(cmd);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public DataTable Select(StringBuilder sql)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql");
            }
            return this.Select(Convert.ToString(sql));
        }


        public IList<T> Select<T>(string sql) where T : class
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql", "sql can not be empty or null");
            }
            MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(sql);
            return this.Select<T>(command);
        }


        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public int Count(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (DataTable dt = this.Select(sql))
            {
                DataRowCollection rows = dt.Rows;
                if (rows.Count > 0)
                {
                    return Convert.ToInt32(rows[0][0]);
                }
                return default(int);
            }
        }

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public int Count(StringBuilder sql)
        {
            if (sql == null)
            {
                throw new ArgumentException("sql");
            }
            return this.Count(sql.ToString());
        }

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public int Count(string table, string where)
        {
            string sql = string.Format("SELECT COUNT(1) FROM {0} ", table);
            if (!string.IsNullOrEmpty(where))
            {
                sql += string.Format("WHERE {0}", where);
            }
            return this.Count(sql);
        }

    }
}
