// Author：yaoshuai
// Email：yscode@126.com
// Date：2021-11-25
// ------------------------------
using System;
using System.Text;

namespace YUnity
{
    #region 初始化
    public partial class LogTool
    {
        private static bool DisableLog;

        internal static void Init(bool enableLog)
        {
            DisableLog = !enableLog;
        }
    }
    #endregion
    public partial class LogTool
    {
        private static string GetMessage(string message, LogColor color = LogColor.None)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("# ");
            sb.Append(DateTime.Now.ToString("HH:mm:ss:fff"));
            sb.Append(" >> ");
            sb.Append(message);

            switch (color)
            {
                case LogColor.Red: return string.Format("<color=#FF0000>{0}</color>", sb);
                case LogColor.Green: return string.Format("<color=#00FF00>{0}</color>", sb);
                case LogColor.Blue: return string.Format("<color=#0000FF>{0}</color>", sb);
                case LogColor.Cyan: return string.Format("<color=#00FFFF>{0}</color>", sb);
                case LogColor.Magenta: return string.Format("<color=#FF00FF>{0}</color>", sb);
                case LogColor.Yellow: return string.Format("<color=#FFFF00>{0}</color>", sb);
                case LogColor.Black: return string.Format("<color=#000000>{0}</color>", sb);
                case LogColor.White: return string.Format("<color=#FFFFff>{0}</color>", sb);
                case LogColor.Gray: return string.Format("<color=#7F7F7F>{0}</color>", sb);
                default: return sb.ToString();
            }
        }
    }
    public partial class LogTool
    {
        public static void Log(string message, params object[] args)
        {
            if (DisableLog) { return; }
            UnityEngine.Debug.Log(GetMessage(string.Format(message, args)));
        }
        public static void Log(object obj)
        {
            if (DisableLog) { return; }
            UnityEngine.Debug.Log(GetMessage(obj.ToString()));
        }
        public static void ColorLog(LogColor color, string message, params object[] args)
        {
            if (DisableLog) { return; }
            UnityEngine.Debug.Log(GetMessage(string.Format(message, args), color));
        }
        public static void ColorLog(LogColor color, object obj)
        {
            if (DisableLog) { return; }
            UnityEngine.Debug.Log(GetMessage(obj.ToString(), color));
        }
    }
    public partial class LogTool
    {
        public static void Warning(string message, params object[] args)
        {
            ColorLog(LogColor.Yellow, message, args);
        }
        public static void Warning(object obj)
        {
            ColorLog(LogColor.Yellow, obj);
        }
        public static void Error(string message, params object[] args)
        {
            ColorLog(LogColor.Red, message, args);
        }
        public static void Error(object obj)
        {
            ColorLog(LogColor.Red, obj);
        }
    }
}