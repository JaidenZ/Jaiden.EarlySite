namespace EarlySite.Drms.Spefication
{
    using System.Text;
    public class AccountResetPassSpefication : SpeficationBase
    {
        private string _account;
        private string _securityCode;
        private int _type;

        /// <summary>
        /// 账户密码重置规约
        /// </summary>
        /// <param name="account"></param>
        /// <param name="securityCode"></param>
        /// <param name="type">
        /// 重置类型
        /// 0:邮箱
        /// 1:手机
        /// </param>
        public AccountResetPassSpefication(string account,string securityCode, int type)
        {
            _account = account;
            _securityCode = securityCode;
            _type = type;
        }

        public override string Satifasy()
        {
            string sql = string.Empty;

            if(_type == 0)
            {
                sql = string.Format("update which_account set SecurityCode = '{0}' where Email = '{1}'", _securityCode, _account);
            }
            else if(_type == 1)
            {
                sql = string.Format("update which_account set Phone = '{0}' where Email = '{1}'", _securityCode, _account);
            }

            return sql;
        }
    }
}
