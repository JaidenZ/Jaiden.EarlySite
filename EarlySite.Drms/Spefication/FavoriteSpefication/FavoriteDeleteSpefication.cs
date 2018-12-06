using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlySite.Drms.Spefication.FavoriteSpefication
{
    public class FavoriteDeleteSpefication : SpeficationBase
    {
        private long _phone;
        private int _favoriteid;


        public FavoriteDeleteSpefication(long phone, int favoriteid)
        {
            _phone = phone;
            _favoriteid = favoriteid;
        }

        public override string Satifasy()
        {
            string sql = string.Format("delete from relation_favorite where Phone = '{0}' and FavoriteId = '{1}'", _phone, _favoriteid);
            
            return sql;

        }
    }
}
