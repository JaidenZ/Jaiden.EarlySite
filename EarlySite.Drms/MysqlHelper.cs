using MySql;
using MySql.Data;
using System.Configuration;


namespace EarlySite.Drms
{
    public class MysqlHelper
    {
        public static string mysqlConnStr = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;




    }
}
