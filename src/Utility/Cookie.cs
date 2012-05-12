using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility
{
    public static class Cookie
    {
        private const string COOKIE_PREFIX = "oa_";


        private static readonly TimeSpan EXCEED_TIME = new TimeSpan(30, 0, 0, 0, 0);

        private static HttpRequestBase Request
        {
            get { return new HttpContextWrapper(HttpContext.Current).Request; }
        }
        private static HttpResponseBase Response
        {
            get { return new HttpContextWrapper(HttpContext.Current).Response; }
        }


        public static void Delete(string key)
        {
            Response.Cookies.Remove(key);
        }
        public static void Post(string key, string value)
        {
            var cookie = new HttpCookie(COOKIE_PREFIX + key, value) { Expires = DateTime.Now + EXCEED_TIME };
            Response.AppendCookie(cookie);
        }
        public static string Get(string key)
        {
            try
            {
                return Request.Cookies[COOKIE_PREFIX + key].Value;
            }
            catch
            {
                return "";
            }
        }
    }
}
