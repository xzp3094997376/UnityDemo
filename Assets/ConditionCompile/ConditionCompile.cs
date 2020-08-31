using System.Diagnostics;

namespace UnityEngine
{
    public static class DebugExtension
    {
        [Conditional("EnableLog")]
        public static void Logger(this object obj, string message)
        {
            Debug.Log(message);
        }
    }
}