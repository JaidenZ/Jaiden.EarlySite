namespace EarlySite.Model.Database
{
    using EarlySite.Model.Enum;
    using System;

    /// <summary>
    /// 账户信息 数据库模型
    /// </summary>
    public class AccountInfo : IKeyNameSpecification
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public long Phone { get; set; }

        /// <summary>
        /// 电子邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        public string SecurityCode { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatDate { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthdayDate { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avator { get; set; }

        /// <summary>
        /// 背景墙
        /// </summary>
        public string BackCorver { get; set; }

        /// <summary>
        /// 账户认证状态
        /// </summary>
        public int RequiredStatus { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 个人描述
        /// </summary>
        public string Description { get; set; }

        public string GetKeyName()
        {
            //DB_AI_手机号_邮箱号_昵称_性别
            return string.Format("DB_AI_{0}_{1}_{2}_{3}",this.Phone,this.Email,this.NickName,this.Sex.GetHashCode());
        }
    }
}
