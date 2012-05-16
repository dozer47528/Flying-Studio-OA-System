using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility
{
    public static class CookieHelper
    {
        private const string COOKIE_PREFIX = "oa_";


        private static readonly TimeSpan EXCEED_TIME = new TimeSpan(30, 0, 0, 0, 0);
        private static readonly TimeSpan EXCEED_TIME_Temp = new TimeSpan(1, 0, 0);

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
            Request.Cookies.Remove(key);
        }
        public static void Post(string key, string value)
        {
            var cookie = new HttpCookie(COOKIE_PREFIX + key, value) { Expires = DateTime.Now + EXCEED_TIME };
            Response.Cookies.Set(cookie);
            Request.Cookies.Set(cookie);
        }
        public static void PostTemp(string key, string value)
        {
            var cookie = new HttpCookie(COOKIE_PREFIX + key, value) { Expires = DateTime.Now + EXCEED_TIME_Temp };
            Response.Cookies.Set(cookie);
            Request.Cookies.Set(cookie);
        }
        public static string Get(string key)
        {
            try
            {
                var req = Request.Cookies[COOKIE_PREFIX + key].Value;
                if (!string.IsNullOrEmpty(req)) return req;
                return string.Empty;
            }
            catch
            {

                return string.Empty;
            }
        }
    }
}
