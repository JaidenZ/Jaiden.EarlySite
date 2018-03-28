namespace EarlySite.Drms.Spefication
{
    using System;
    using EarlySite.Model.Database;

    public class AccountAddSpefication : SpeficationBase
    {
        private AccountInfo _account;

        public AccountAddSpefication(AccountInfo account)
        {
            _account = account;
        }

        public override string Satifasy()
        {
            return string.Format("insert into which_account (Phone,Email,SecurityCode,CreatDate,BirthdayDate,NickName,Avator,BackCorver," +
                "Sex,Description,RequiredStatus) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                _account.Phone, _account.Email, _account.SecurityCode, _account.CreatDate, _account.BirthdayDate, _account.NickName,
                _account.Avator, _account.BackCorver, _account.Sex.GetHashCode(), _account.Description, _account.RequiredStatus.GetHashCode());
        }
    }
}
