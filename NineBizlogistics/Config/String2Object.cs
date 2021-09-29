using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NineBizlogistics.Config
{
    public static class String2Object
    {
        /// <summary>
        /// 计算字符串的md5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(this string str)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                string result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str)));
                result = result.Replace("-", string.Empty).ToLower();
                return result;
            }
        }
    }
}
