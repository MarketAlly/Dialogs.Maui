namespace MarketAlly.Dialogs.Maui.Interfaces
{
    /// <summary>
    /// Interface for logging dialog-related events and errors
    /// </summary>
    public interface IDialogLogger
    {
        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Debug(string message);

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Info(string message);

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Warning(string message);

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Error(string message);

        /// <summary>
        /// Logs an error message with an exception
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception that occurred</param>
        void Error(string message, Exception exception);
    }

    /// <summary>
    /// Default no-op logger implementation (does nothing)
    /// </summary>
    public class NullDialogLogger : IDialogLogger
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static readonly NullDialogLogger Instance = new();

        private NullDialogLogger() { }

        public void Debug(string message) { }
        public void Info(string message) { }
        public void Warning(string message) { }
        public void Error(string message) { }
        public void Error(string message, Exception exception) { }
    }

    /// <summary>
    /// Debug logger that writes to System.Diagnostics.Debug
    /// </summary>
    public class DebugDialogLogger : IDialogLogger
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static readonly DebugDialogLogger Instance = new();

        private DebugDialogLogger() { }

        public void Debug(string message) => System.Diagnostics.Debug.WriteLine($"[Dialog DEBUG] {message}");
        public void Info(string message) => System.Diagnostics.Debug.WriteLine($"[Dialog INFO] {message}");
        public void Warning(string message) => System.Diagnostics.Debug.WriteLine($"[Dialog WARNING] {message}");
        public void Error(string message) => System.Diagnostics.Debug.WriteLine($"[Dialog ERROR] {message}");
        public void Error(string message, Exception exception) => System.Diagnostics.Debug.WriteLine($"[Dialog ERROR] {message}: {exception}");
    }
}
