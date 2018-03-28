namespace EarlySite.Drms.Spefication
{
    /// <summary>
    /// 账户认证SQL
    /// </summary>
    public class AccountRequireSpefication : SpeficationBase
    {
        private bool _required = false;
        private long _phone;

        public AccountRequireSpefication(bool require,long phoneID)
        {
            _required = require;
            _phone = phoneID;
        }

        public override string Satifasy()
        {
            return string.Format("update which_account set RequiredStatus = '{0}' where Phone = '{1}'", _required ? 1 : 0, _phone);
        }
    }
}
