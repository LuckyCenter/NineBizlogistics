using NineBizlogistics.DB;

namespace ModelClass.DB
{
    public class SystemConfig : DBBase<SystemConfig>
    {
        public int SoundVolumn { get; set; } = 80;
        public bool ScreenAutoLock { get; set; } = false;
        public ulong ScreenTimeOutMinute { get; set; } = 1;
        public string TimeHost { get; set; } = "";
        public string HISHost { get; set; } = "";
        public ushort HISPort { get; set; } = 0;
        public string Version { get; set; } = "NoHis";
    }
}
