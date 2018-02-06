namespace EarlySite.Drms.DBManager
{
    using System.Collections;
    using System.Collections.Generic;
    using Connection;

    public class DBConnectionManager
    {
        public static IConnection MySqlConnection
        {
            get
            {
                return new MysqlConnection();
            }
        }



    }
    
}
