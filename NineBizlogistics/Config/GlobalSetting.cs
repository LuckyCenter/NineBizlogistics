using System.IO;

namespace NineBizlogistics.Config
{
    class GlobalSetting
    {
        /// <summary>
        /// 初始化路径
        /// </summary>
        public static void Init()
        {
            Directory.CreateDirectory(PathBase);
            Directory.CreateDirectory(PathBug);
            Directory.CreateDirectory(PathLog);
            Directory.CreateDirectory(PathBackup);

            LogHelper.SetPath(PathLog, PathBug);
        }
        public const string MysqlHost = "localhost";
        public const int MysqlPort = 3306;
        public const string MysqlUserName = "root";
        public const string MysqlPwd = "root";
        public const string MysqlDBName = "WBA";

        public const int HttpPort = 80;

        public const string PathBase = @"C:\ProgramData\Nine_Bizlogistics";
        public const string PathBug = PathBase + @"\Bug";
        public const string PathLog = PathBase + @"\Log";
        public const string PathBackup = PathBase + @"\Backup";
        public const string PathSQLiteDBFile = PathBase + @"\Data.db3";
    }
}
