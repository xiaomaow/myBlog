using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Web.common
{
    public class CookieHelper
    {
        public static void SetCookie(string key, string value, DateTime? expires = null, string domain = "")
        {
            if (null == key) throw new ArgumentNullException("cookie的键不能为空");
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Domain = domain;
            if (expires != null)
            {
                cookie.Expires = Convert.ToDateTime(expires);
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static HttpCookie GetCookie(string key)
        {
            HttpCookie _cookie = HttpContext.Current.Request.Cookies[key];
            return _cookie;
        }
    }
}