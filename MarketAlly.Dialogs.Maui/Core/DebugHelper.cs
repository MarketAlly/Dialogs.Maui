using System.Diagnostics;

namespace MarketAlly.Dialogs.Maui.Core
{
    internal static class DebugHelper
    {
        [Conditional("DEBUG")]
        public static void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
        public static void WriteLine(string format, params object[] args)
        {
            Debug.WriteLine(format, args);
        }
    }
}