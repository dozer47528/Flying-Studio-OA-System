using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

namespace Utility
{
    public static class StringExtension
    {
        /// <summary>
        /// 区分中英文的字符截取
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="length">长度</param>
        /// <returns>截取好的字符串</returns>
        public static string SubByte(this string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var bytes = Encoding.Unicode.GetBytes(s);
            var n = 0;  //  表示当前的字节数
            var i = 0;  //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < length; i++)
            {
                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                if (i % 2 == 0)
                {
                    n++;      //  在UCS2第一个字节时n加1
                }
                else
                {
                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }
            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                if (bytes[i] > 0)
                {
                    i = i - 1;
                }
                //  该UCS2字符是字母或数字，则保留该字符
                else
                {
                    i = i + 1;
                }
            }
            return i < bytes.Length ? Encoding.Unicode.GetString(bytes, 0, i) + "…" : Encoding.Unicode.GetString(bytes, 0, i);
        }

        /// <summary>
        /// 计算MD5
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string MD5(this string s)
        {
            var temp = FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5");
            return FormsAuthentication.HashPasswordForStoringInConfigFile(string.Concat(temp, "oa"), "MD5");
        }
    }
}
