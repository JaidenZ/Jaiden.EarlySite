namespace EarlySite.Drms
{
    using MySql;
    using MySql.Data;
    using System.Configuration;


    public class MysqlHelper
    {
        private readonly static string mysqlConnStr = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;



    }
}
