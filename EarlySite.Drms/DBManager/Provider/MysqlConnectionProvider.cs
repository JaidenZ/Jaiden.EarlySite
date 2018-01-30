namespace EarlySite.Drms.DBManager.Provider
{
    using MySql;
    using MySql.Data;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using EarlySite.Core.Utils;


    public class MysqlConnectionProvider : IConnectionHandler
    {
        private readonly static string mysqlConnStr = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;

        private static MySql.Data.MySqlClient.MySqlConnection mySqlConnection;

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
        

        public IList<object> ExecuteDataTableList(string command)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteNonQuery(string command)
        {
            int result = 0;
            try
            {
                using (MySql.Data.MySqlClient.MySqlCommand mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(command, mySqlConnection))
                {
                    result = mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            catch(Exception ex)
            {
                result = 0;
                LoggerUtils.ColectExceptionMessage(ex, "MySqlConnectionProvider 67lines");
            }
            return result;
        }

        public void Start()
        {
            try
            {
                mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(mysqlConnStr);
                if (mySqlConnection.Ping())
                {
                    mySqlConnection.Open();
                }
            }
            catch(Exception ex)
            {
                LoggerUtils.ColectExceptionMessage(ex, "MySqlConnectionProvider 85 lines");
            }
        }

        public void Stop()
        {
            try
            {
                if (mySqlConnection != null)
                {
                    mySqlConnection.Close();
                    mySqlConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                LoggerUtils.ColectExceptionMessage(ex, "MySqlConnectionProvider 81lines");
            }
        }
    }
}
