namespace MarketAlly.Dialogs.Maui.Telemetry
{
    /// <summary>
    /// Configuration for dialog telemetry
    /// </summary>
    public class TelemetryConfiguration
    {
        /// <summary>
        /// Gets or sets whether telemetry is enabled (opt-in, default false)
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to track dialog open events
        /// </summary>
        public bool TrackOpenEvents { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to track dialog close events
        /// </summary>
        public bool TrackCloseEvents { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to track dialog duration
        /// </summary>
        public bool TrackDuration { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to track platform information
        /// </summary>
        public bool TrackPlatform { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to track theme information
        /// </summary>
        public bool TrackTheme { get; set; } = true;

        /// <summary>
        /// Creates a disabled telemetry configuration
        /// </summary>
        public static TelemetryConfiguration Disabled => new TelemetryConfiguration { IsEnabled = false };

        /// <summary>
        /// Creates an enabled telemetry configuration with all tracking options
        /// </summary>
        public static TelemetryConfiguration Enabled => new TelemetryConfiguration { IsEnabled = true };

        /// <summary>
        /// Creates a minimal telemetry configuration (only open/close events)
        /// </summary>
        public static TelemetryConfiguration Minimal => new TelemetryConfiguration
        {
            IsEnabled = true,
            TrackDuration = false,
            TrackPlatform = false,
            TrackTheme = false
        };
    }
}
