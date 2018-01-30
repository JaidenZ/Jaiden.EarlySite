namespace EarlySite.Drms.DBManager.Provider
{
    using MySql;
    using MySql.Data;
    using System.Configuration;


    public class MysqConnectionProvider:IConnectionHandler
    {
        private readonly static string mysqlConnStr = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;

        private static MySql.Data.MySqlClient.MySqlConnection mySqlConnection;
        

        public static int ExecuteNonQuery(string command)
        {
            int result = 0;
            
            using(MySql.Data.MySqlClient.MySqlCommand mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(command, mySqlConnection))
            {
                result = mySqlCommand.ExecuteNonQuery();
            }
            mySqlConnection.Close();



            return result;
        }
        
    }
}
