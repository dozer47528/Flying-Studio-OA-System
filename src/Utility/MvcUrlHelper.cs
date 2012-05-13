using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public static class MvcUrlHelper
    {
        public static string GetFromUrl(string url, string from)
        {
            var format = string.Empty;
            if (url.Contains("?"))
            {
                if (url.EndsWith("&"))
                {
                    format = "{0}from={1}";
                }
                else
                {
                    format = "{0}&from={1}";
                }
            }
            else
            {
                format = "{0}?from={1}";
            }
            return string.Format(format, url, Uri.EscapeDataString(from));

        }
    }
}
