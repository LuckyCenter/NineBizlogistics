using NineBizlogistics.DB;

namespace NineBizlogistics.Model
{
    public class Customer : DBBase<Customer>
    {
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
