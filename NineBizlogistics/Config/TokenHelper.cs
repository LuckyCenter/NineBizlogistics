using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using NineBizlogistics.Model;

namespace NineBizlogistics.Config
{
    /// <summary>
    /// 身份验证
    /// </summary>
    public class TokenHelper
    {
        static TokenHelper()
        {
              Dic.Add("1", new UserInfo() { Contact = "1", PersonName = "maomap", UserName = "1" });
        }
        static Dictionary<string, UserInfo> Dic = new Dictionary<string, UserInfo>();
        /// <summary>
        /// 强制下线
        /// </summary>
        /// <param name="f"></param>
        public static void Delete(Func<KeyValuePair<string, UserInfo>, bool> f)
        {
            var temp = Dic.Where(f).FirstOrDefault();
            if (temp.Value != null)
            {
                Dic.Remove(temp.Key);
            }
        }
        /// <summary>
        /// 获取已登录信息
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static KeyValuePair<string, UserInfo> GetValue(Func<KeyValuePair<string, UserInfo>, bool> f)
        {
            var temp = Dic.Where(f).FirstOrDefault();
            return temp;

        }
        /// <summary>
        /// 获取缓存用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static void UpdateUsers(Func<KeyValuePair<string, UserInfo>, bool> f, UserInfo U)
        {
            var temp = Dic.Where(f).FirstOrDefault();
            if (temp.Key != null)
            {
                if (Dic.ContainsKey(temp.Key))
                {
                    Dic[temp.Key] = U;
                }
            }

        }
        /// <summary>
        /// 登录后缓存
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        public static void UpdateToken(string token, UserInfo user)
        {
            if (Dic.ContainsKey(token))
            {
                Dic[token] = user;
            }
            else
            {
                Dic.Add(token, user);
            }
        }
        /// <summary>
        /// 尝试获取已登录信息
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static UserInfo TryGetUser(HttpRequest Request)
        {
            UserInfo temp = null;
            if (Request.Headers.TryGetValue("Authorization", out var auth))
            {
                if (Dic.ContainsKey(auth))
                {
                    temp = Dic[auth];
                }
            }
            return temp;
        }
    }
}