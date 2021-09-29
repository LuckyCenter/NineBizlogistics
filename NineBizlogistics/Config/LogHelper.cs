using System;
using System.IO;
using System.Text;

namespace NineBizlogistics.Config
{
    public class LogHelper
    {
        static string LogPath = "";
        static string ExceptionPath = "";
        public static void SetPath(string logdir, string exceptiondir)
        {
            LogPath = logdir;
            ExceptionPath = exceptiondir;
        }

        public static void LogException(Exception ex)
        {
            var sb = CreatExceptionMsg(ex);
            if (ex.InnerException != null)
            {
                sb = CreatExceptionMsg(ex.InnerException, sb);
            }
            string filename = $"{Path.Combine(ExceptionPath, $"bug_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{Guid.NewGuid().ToString()}.txt") }";
            try
            {
                File.WriteAllText(filename, sb.ToString());
            }
            catch { }
        }
        static StringBuilder CreatExceptionMsg(Exception ex, StringBuilder sb = null)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(ex.Message);
            if (ex.Source != null)
            {
                sb.AppendLine(ex.Source);
            }
            sb.AppendLine(ex.StackTrace);
            return sb;
        }

        public static void LogMsg(string msg)
        {
            string filename = Path.Combine(LogPath, $"Log_{DateTime.Now.ToString("yyyyMMdd")}.txt");
            try
            {
                File.AppendAllLines(filename, new string[] { msg });
            }
            catch { }
        }
    }
}
