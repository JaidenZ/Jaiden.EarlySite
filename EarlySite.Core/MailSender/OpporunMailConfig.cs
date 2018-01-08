namespace EarlySite.Core.MailSender
{
    using System;
    public class OpporunMailConfig
    {
        /// <summary>
        /// 邮箱服务器
        /// </summary>
        public string MailServer { get; set; }
        /// <summary>
        /// 邮箱服务器端口
        /// </summary>
        public string MailServerPort { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string SendUser { get; set; }
    }
}
