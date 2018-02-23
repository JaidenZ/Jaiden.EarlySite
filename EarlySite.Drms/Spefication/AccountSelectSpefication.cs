namespace EarlySite.Drms.Spefication
{
    using System;
    using System.Text;

    public class AccountSelectSpefication : SpeficationBase
    {
        private int _type = 0;
        private string _searchText = string.Empty;
        private string _securityCode = string.Empty;

        /// <summary>
        /// 账户查询规约
        /// </summary>
        /// <param name="type">
        /// 查询类型
        /// 0:按手机号查询
        /// 1:按手机号密码匹配查询
        /// 2:按昵称模糊
        /// </param>
        /// <param name="search"></param>
        /// <param name="securityCode"></param>
        public AccountSelectSpefication(int type ,string search,string securityCode = null)
        {
            _type = type;
            _searchText = search;
            _securityCode = securityCode;
        }

        public override string Satifasy()
        {
            string sql = "";

            if(_type == 0)
            {
                sql = string.Format(" select Phone,Email,SecurityCode,CreatDate,BirthdayDate,NickName,Avator,BackCorver,Sex,Description,RequiredStatus " +
                    " where Phone = {0} ", _searchText);
            }
            if (_type == 1)
            {
                sql = string.Format(" select Phone,Email,SecurityCode,CreatDate,BirthdayDate,NickName,Avator,BackCorver,Sex,Description,RequiredStatus " +
                    " where Phone = '{0}' and SecurityCode = '{1}' ", _searchText, _securityCode);
            }
            if (_type == 2)
            {
                sql = string.Format(" select Phone,Email,SecurityCode,CreatDate,BirthdayDate,NickName,Avator,BackCorver,Sex,Description,RequiredStatus " +
                    " where NickName like '%{0}%' ", _searchText);
            }

            return sql;
        }
    }
}
