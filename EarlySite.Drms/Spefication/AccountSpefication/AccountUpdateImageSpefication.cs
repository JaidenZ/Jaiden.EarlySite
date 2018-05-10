namespace EarlySite.Drms.Spefication
{
    public class AccountUpdateImageSpefication : SpeficationBase
    {

        private string _base64str = "";
        private string _phone = 0;
        private int _type = 0;
        /// <summary>
        /// 账户修改图片(头像,背景图)规约
        /// </summary>
        /// <param name="base64str"></param>
        /// <param name="phone"></param>
        /// <param name="type">
        /// 修改类型
        /// 0:头像
        /// 1:背景图
        /// </param>
        public AccountUpdateImageSpefication(string base64str,string phone ,int type)
        {
            _base64str = base64str;
            _type = type;
            _phone = phone;
        }

        public override string Satifasy()
        {
            string sql = "";

            if(_type == 0)
            {
                sql = string.Format("update which_account set Avator = '{0}' where Phone = '{1}'", _base64str, _phone);
            }

            if(_type == 1)
            {
                sql = string.Format("update which_account set BackCorver = '{0}' where Phone = '{1}'", _base64str, _phone);
            }
            return sql;
        }
    }
}
