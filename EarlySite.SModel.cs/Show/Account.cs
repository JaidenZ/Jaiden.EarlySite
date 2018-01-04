namespace EarlySite.Model.Show
{
    using System;

    /// <summary>
    /// 账户模型
    /// </summary>
    public class Account
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
        /// 创建时间
        /// </summary>
        public DateTime CreatTime { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avator { get; set; }



    }
}
