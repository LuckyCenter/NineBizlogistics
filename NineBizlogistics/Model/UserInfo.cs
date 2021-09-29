using System;
using NineBizlogistics.DB;

namespace NineBizlogistics.Model
{
    public class UserInfo : DBBase<UserInfo>
    {
        public string UserName { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Pwd { get; set; }
        public string PersonName { get; set; }
        public string Token { get; set; }
        public string Contact { get; set; } = "";
    }
}
