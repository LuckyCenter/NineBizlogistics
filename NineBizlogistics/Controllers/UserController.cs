using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using NineBizlogistics.Config;
using NineBizlogistics.Model;

namespace NineBizlogistics.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        ///// <summary>
        ///// 计算md5
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonModel ToMd5(string key)
        {
            JsonModel J = new JsonModel();
            J.SetContent(key.ToMD5());
            return J;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Pwd">密码md5</param>
        /// <param name="IsMobile">是否移动端</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public JsonModel Login(string userName, string pwd)
        {
            JsonModel J = new JsonModel();

            var tempuser = UserInfo.FirstOrDefault(zz => zz.UserName == userName && zz.Pwd == pwd);
            J.Check("用户名或密码错误", () => tempuser != null);
            if (J.CanContinue)
            {
                var tm = TokenHelper.GetValue(zz => zz.Value.UserName == userName && zz.Value.Pwd == pwd);
                bool Islogin = tm.Value != null;
                string token = null;
                if (Islogin)
                {
                    token = tm.Key;
                }
                else
                {
                    token = Guid.NewGuid().ToString();
                }
                TokenHelper.UpdateToken(token, tempuser);
                J.Add("token", token);
                J.Add("info", tempuser);
            }
            return J;
        }

        /// <summary>
        /// 获取登录账户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonModel MyInfo()
        {
            JsonModel J = new JsonModel();
            J.SetContent(TokenHelper.TryGetUser(Request));
            return J;
        }
    }
}