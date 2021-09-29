using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineBizlogistics.Model
{
    public class CustomerReq
    {
        public bool IsAdd { get; set; }
        public string Uid { get; set; } = "";
        public string ContractId { get; set; } = "";
        public string Name { get; set; } = "";
        public string WechatId { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Product { get; set; } = "";
        public string Area { get; set; } = "";
        public string QQ { get; set; } = "";
        public string HigherLevelWechat { get; set; } = "";
        public string Level { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public string Note { get; set; } = "";
    }
}
