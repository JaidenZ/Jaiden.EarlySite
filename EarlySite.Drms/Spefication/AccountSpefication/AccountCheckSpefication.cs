namespace EarlySite.Drms.Spefication
{
    public class AccountCheckSpefication : SpeficationBase
    {
        private string _checkStr = string.Empty;
        private int _type = 0;


        /// <summary>
        /// 账户检查规约
        /// </summary>
        /// <param name="checkStr">检查字符串</param>
        /// <param name="type">
        /// 类型
        /// 0:手机
        /// 1:邮箱
        /// </param>
        public AccountCheckSpefication(string checkStr,int type)
        {
            _checkStr = checkStr;
            _type = type;
        }


        public override string Satifasy()
        {
            string sql = string.Empty;
            if(_type == 0)
            {
                sql = string.Format("select count(1) from which_account where Phone = '{0}'", _checkStr);
            }
            else if(_type == 1)
            {
                sql = string.Format("select count(1) from which_account where Email = '{0}'", _checkStr);
            }
            return sql;
        }
    }
}
