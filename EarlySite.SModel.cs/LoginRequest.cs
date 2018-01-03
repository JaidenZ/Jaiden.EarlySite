namespace EarlySite.SModel
{
    public class LoginRequest
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        public string LoginUsername { get; set; }

        /// <summary>
        /// 登录安全码
        /// </summary>
        public string LoginSecurity { get; set; }
    }
}
