namespace MarketAlly.Dialogs.Maui.Telemetry
{
    /// <summary>
    /// Represents a telemetry event for dialog usage
    /// </summary>
    public class DialogTelemetryEvent
    {
        /// <summary>
        /// Gets or sets the unique dialog instance ID
        /// </summary>
        public Guid DialogId { get; set; }

        /// <summary>
        /// Gets or sets the dialog type name
        /// </summary>
        public string DialogTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event type
        /// </summary>
        public TelemetryEventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the event occurred
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the duration the dialog was open (only for Close events)
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets whether dark theme is being used
        /// </summary>
        public bool IsDarkTheme { get; set; }

        /// <summary>
        /// Gets or sets the platform name
        /// </summary>
        public string? Platform { get; set; }

        /// <summary>
        /// Gets or sets the result of the dialog (e.g., "Confirmed", "Cancelled")
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// Creates a new opened event
        /// </summary>
        public static DialogTelemetryEvent CreateOpened(string dialogTypeName, bool isDarkTheme)
        {
            return new DialogTelemetryEvent
            {
                DialogId = Guid.NewGuid(),
                DialogTypeName = dialogTypeName,
                EventType = TelemetryEventType.Opened,
                Timestamp = DateTime.UtcNow,
                IsDarkTheme = isDarkTheme,
                Platform = DeviceInfo.Platform.ToString()
            };
        }

        /// <summary>
        /// Creates a closed event from an opened event
        /// </summary>
        public static DialogTelemetryEvent CreateClosed(DialogTelemetryEvent openedEvent, string? result = null)
        {
            return new DialogTelemetryEvent
            {
                DialogId = openedEvent.DialogId,
                DialogTypeName = openedEvent.DialogTypeName,
                EventType = TelemetryEventType.Closed,
                Timestamp = DateTime.UtcNow,
                Duration = DateTime.UtcNow - openedEvent.Timestamp,
                IsDarkTheme = openedEvent.IsDarkTheme,
                Platform = openedEvent.Platform,
                Result = result
            };
        }
    }

    /// <summary>
    /// Types of telemetry events
    /// </summary>
    public enum TelemetryEventType
    {
        /// <summary>
        /// Dialog was opened
        /// </summary>
        Opened,

        /// <summary>
        /// Dialog was closed
        /// </summary>
        Closed
    }
}
