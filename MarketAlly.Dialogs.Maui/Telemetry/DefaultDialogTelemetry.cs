namespace MarketAlly.Dialogs.Maui.Telemetry
{
    /// <summary>
    /// Default telemetry implementation that raises events for external analytics integration
    /// </summary>
    public class DefaultDialogTelemetry : IDialogTelemetry
    {
        private readonly TelemetryConfiguration _configuration;

        /// <summary>
        /// Event raised when any telemetry event occurs
        /// </summary>
        public event EventHandler<DialogTelemetryEvent>? TelemetryEvent;

        /// <summary>
        /// Event raised specifically when a dialog is opened
        /// </summary>
        public event EventHandler<DialogTelemetryEvent>? DialogOpened;

        /// <summary>
        /// Event raised specifically when a dialog is closed
        /// </summary>
        public event EventHandler<DialogTelemetryEvent>? DialogClosed;

        public DefaultDialogTelemetry() : this(TelemetryConfiguration.Disabled)
        {
        }

        public DefaultDialogTelemetry(TelemetryConfiguration configuration)
        {
            _configuration = configuration ?? TelemetryConfiguration.Disabled;
        }

        public bool IsEnabled => _configuration.IsEnabled;

        public void OnDialogOpened(DialogTelemetryEvent eventData)
        {
            if (!_configuration.IsEnabled || !_configuration.TrackOpenEvents)
                return;

            // Clean up event data based on configuration
            if (!_configuration.TrackPlatform)
                eventData.Platform = null;

            if (!_configuration.TrackTheme)
                eventData.IsDarkTheme = false;

            TelemetryEvent?.Invoke(this, eventData);
            DialogOpened?.Invoke(this, eventData);
        }

        public void OnDialogClosed(DialogTelemetryEvent eventData)
        {
            if (!_configuration.IsEnabled || !_configuration.TrackCloseEvents)
                return;

            // Clean up event data based on configuration
            if (!_configuration.TrackDuration)
                eventData.Duration = null;

            if (!_configuration.TrackPlatform)
                eventData.Platform = null;

            if (!_configuration.TrackTheme)
                eventData.IsDarkTheme = false;

            TelemetryEvent?.Invoke(this, eventData);
            DialogClosed?.Invoke(this, eventData);
        }

        /// <summary>
        /// Updates the telemetry configuration
        /// </summary>
        public void Configure(TelemetryConfiguration configuration)
        {
            if (configuration != null)
            {
                // Copy settings (we don't replace the instance to preserve event handlers)
                _configuration.IsEnabled = configuration.IsEnabled;
                _configuration.TrackOpenEvents = configuration.TrackOpenEvents;
                _configuration.TrackCloseEvents = configuration.TrackCloseEvents;
                _configuration.TrackDuration = configuration.TrackDuration;
                _configuration.TrackPlatform = configuration.TrackPlatform;
                _configuration.TrackTheme = configuration.TrackTheme;
            }
        }

        /// <summary>
        /// Enables telemetry
        /// </summary>
        public void Enable()
        {
            _configuration.IsEnabled = true;
        }

        /// <summary>
        /// Disables telemetry
        /// </summary>
        public void Disable()
        {
            _configuration.IsEnabled = false;
        }
    }
}
