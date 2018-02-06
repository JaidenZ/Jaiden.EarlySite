namespace EarlySite.Drms.DBManager.Connection
{
    using MySql;
    using MySql.Data;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using EarlySite.Core.Utils;
    using EarlySite.Core.Data;
    using EarlySite.Drms.DBManager.Connection;
    using System.Data;
    using EarlySite.Core.Component;

    public class MysqlConnection : IConnection
    {
        private readonly static string mysqlConnStr = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;

        private static MySql.Data.MySqlClient.MySqlConnection mySqlConnection;

        private static string WORK_KEY_NAME = "MysqlConn";

        public static MySql.Data.MySqlClient.MySqlConnection Current
        {
            get
            {
                if(mySqlConnection == null)
                {
                    mySqlConnection = DeployInThread();
                }

                return mySqlConnection;
            }
        }


        public string connectionStr
        {
            get
            {
                return mysqlConnStr;
            }
        }

        public bool Connected
        {
            get
            {
                if (mySqlConnection != null)
                {
                    if (mySqlConnection.State != System.Data.ConnectionState.Broken &&
                        mySqlConnection.State != System.Data.ConnectionState.Closed &&
                        mySqlConnection.State != System.Data.ConnectionState.Connecting)
                    {

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }

        }
        

        public static MySql.Data.MySqlClient.MySqlConnection DeployInThread()
        {
            WorkThreadDictionary work = WorkThreadDictionary.Get();
            MySql.Data.MySqlClient.MySqlConnection conn = null;
            if (work != null)
            {
                conn = work.Get<MySql.Data.MySqlClient.MySqlConnection>(WORK_KEY_NAME);
            }
            if (conn == null)
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                if (work != null)
                {
                    work.Set(WORK_KEY_NAME, conn);
                }
                conn.Disposed += Connection_Disposed;
            }
            if (conn.State != ConnectionState.Open)
            {
                conn.ConnectionString = mysqlConnStr;
            }
            return conn;
        }


        private static void Connection_Disposed(object sender, EventArgs e)
        {
            System.Console.WriteLine("MysqlConnection is Disposed");
        }
    }
}
