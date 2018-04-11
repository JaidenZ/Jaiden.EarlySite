namespace EarlySite.Drms.DBManager
{
    using System.Collections;
    using System.Collections.Generic;
    using Connection;
    using Provider;

    public class DBConnectionManager
    {

        private static DBConnectionManager _manager;

        public MySqlDBReader Reader;
        public MySqlDBWriter Writer;



        private DBConnectionManager()
        {
            Reader = new MySqlDBReader();
            Writer = new MySqlDBWriter();

        }

        public static DBConnectionManager Instance
        {
            get
            {
                if(_manager == null)
                {
                    _manager = new DBConnectionManager();
                }
                return _manager;
            }
        }
        
    }
    
}
