using System.Text.Json.Serialization;

namespace NineBizlogistics.Config
{
    /// <summary>
    /// 解析模板
    /// </summary>
    public class JsonModelBase
    {
        public int Status { get; set; } = 1;
        public object  Data { get; set; } = null;
        public string Error { get; set; } = "";
        [JsonIgnore]
        public bool CanContinue { get; set; } = true;
    }
}
