namespace MarketAlly.Dialogs.Maui.Telemetry
{
    /// <summary>
    /// Interface for dialog telemetry providers
    /// </summary>
    public interface IDialogTelemetry
    {
        /// <summary>
        /// Gets whether telemetry is currently enabled
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Called when a dialog is opened
        /// </summary>
        /// <param name="eventData">The telemetry event data</param>
        void OnDialogOpened(DialogTelemetryEvent eventData);

        /// <summary>
        /// Called when a dialog is closed
        /// </summary>
        /// <param name="eventData">The telemetry event data</param>
        void OnDialogClosed(DialogTelemetryEvent eventData);
    }
}
