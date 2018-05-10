namespace EarlySite.Drms.Spefication
{
    using Model.Database;

    /// <summary>
    /// 更新账户基础信息规约
    /// </summary>
    public class AccountUpdateInfoSpefication : SpeficationBase
    {
        private AccountInfo _account;

        /// <summary>
        /// 更新账户基础信息规约
        /// </summary>
        /// <param name="account">账户信息</param>
        public AccountUpdateInfoSpefication(AccountInfo account)
        {
            _account = account;
        }
        public override string Satifasy()
        {
            string sql = string.Format("update which_account set BirthdayDate = '{0}',NickName = '{1}', Sex = '{2}',Description = '{3}' where Phone = '{4}'", _account.BirthdayDate, _account.NickName, _account.Sex.GetHashCode(), _account.Description, _account.Phone);
            
            return sql;
        }
    }
}
