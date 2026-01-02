using UnityEngine;

namespace Core.Scripts
{
    public class Logger
    {
        public static void Log(string argMessage, LogType argLogType = LogType.Normal, Color argColor = default)
        {
            if (argColor == default && argLogType == LogType.Normal)
            {
                //Common case fast
                Debug.Log(argMessage);
                return;
            }

            string message = argMessage;
            if (argColor != default)
            {
                message = $"<color={ColorToHex(argColor)}>" + argMessage + "</color>";
            }

            if (argLogType == LogType.Normal)
            {
                Debug.Log(message);
            }
            else if (argLogType == LogType.Warning)
            {
                Debug.LogWarning(message);
            }
            else if (argLogType == LogType.Error)
            {
                Debug.LogError(message);
            }
        }

        public static void LogError(string argMessage, bool forcePause = false)
        {
            Log(argMessage, LogType.Error, Color.red);

            if (forcePause)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPaused = true;
#endif
            }
        }

        public static string ColorToHex(Color argColor)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(argColor)}";
        }
    }

    public enum LogType
    {
        Normal,
        Warning,
        Error,
    }
}
