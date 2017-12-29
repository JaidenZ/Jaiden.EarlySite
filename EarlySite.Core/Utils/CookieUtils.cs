namespace EarlySite.Core.Utils
{
    using System;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Cookie工具
    /// </summary>
    public class CookieUtils
    {

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <param name="expires">过期时间</param>
        /// <returns></returns>
        public static void SetCookie(string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = expires;
            if (HttpContext.Current.Response.Cookies.AllKeys.Any(o => o == key))
            {
                HttpContext.Current.Response.Cookies.Remove(key);
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <returns></returns>
        public static void SetCookie(string key, string value)
        {
            SetCookie(key, value, DateTime.Now.AddHours(24));
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <returns>cookie字符串</returns>
        public static string Get(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            return cookie == null ? string.Empty : cookie.Value;
        }

    }
}
