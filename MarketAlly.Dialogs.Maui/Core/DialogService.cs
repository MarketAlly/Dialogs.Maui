using MarketAlly.Dialogs.Maui.Interfaces;
using MarketAlly.Dialogs.Maui.Localization;
using MarketAlly.Dialogs.Maui.Models;

namespace MarketAlly.Dialogs.Maui.Core
{
    /// <summary>
    /// Main service for managing dialogs with theming and localization support.
    /// This class is thread-safe for concurrent access.
    /// </summary>
    public class DialogService
    {
        private static readonly object _instanceLock = new();
        private static DialogService? _instance;
        private readonly object _stateLock = new();

        private DialogTheme _lightTheme = DialogTheme.LightTheme;
        private DialogTheme _darkTheme = DialogTheme.DarkTheme;
        private bool _useSystemTheme = true;
        private DialogTheme? _currentThemeOverride;
        private IDialogLocalization _localization = new DefaultDialogLocalization();
        private IDialogLogger _logger = NullDialogLogger.Instance;
        private readonly Dictionary<DialogType, DialogIconMapping> _customIcons = new();

        /// <summary>
        /// Gets the singleton instance of the DialogService
        /// </summary>
        public static DialogService Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_instanceLock)
                {
                    _instance ??= new DialogService();
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Gets or sets the light theme for dialogs
        /// </summary>
        public DialogTheme LightTheme
        {
            get { lock (_stateLock) return _lightTheme; }
            set { lock (_stateLock) _lightTheme = value ?? DialogTheme.LightTheme; }
        }

        /// <summary>
        /// Gets or sets the dark theme for dialogs
        /// </summary>
        public DialogTheme DarkTheme
        {
            get { lock (_stateLock) return _darkTheme; }
            set { lock (_stateLock) _darkTheme = value ?? DialogTheme.DarkTheme; }
        }

        /// <summary>
        /// Gets or sets whether to automatically use the system theme
        /// </summary>
        public bool UseSystemTheme
        {
            get { lock (_stateLock) return _useSystemTheme; }
            set { lock (_stateLock) _useSystemTheme = value; }
        }

        /// <summary>
        /// Gets or sets the current theme override (null to use system theme)
        /// </summary>
        public DialogTheme? CurrentThemeOverride
        {
            get { lock (_stateLock) return _currentThemeOverride; }
            set { lock (_stateLock) _currentThemeOverride = value; }
        }

        /// <summary>
        /// Gets or sets the localization provider
        /// </summary>
        public IDialogLocalization Localization
        {
            get { lock (_stateLock) return _localization; }
            set { lock (_stateLock) _localization = value ?? new DefaultDialogLocalization(); }
        }

        /// <summary>
        /// Gets or sets the logger for dialog events and errors
        /// </summary>
        public IDialogLogger Logger
        {
            get { lock (_stateLock) return _logger; }
            set { lock (_stateLock) _logger = value ?? NullDialogLogger.Instance; }
        }

        /// <summary>
        /// Gets custom icon mappings for dialog types (returns a copy for thread safety)
        /// </summary>
        public Dictionary<DialogType, DialogIconMapping> CustomIcons
        {
            get
            {
                lock (_stateLock)
                {
                    return new Dictionary<DialogType, DialogIconMapping>(_customIcons);
                }
            }
        }

        /// <summary>
        /// Gets the current active theme based on settings and system theme
        /// </summary>
        public DialogTheme CurrentTheme
        {
            get
            {
                lock (_stateLock)
                {
                    if (_currentThemeOverride != null)
                        return _currentThemeOverride;

                    if (!_useSystemTheme)
                        return _lightTheme;

                    var currentAppTheme = Application.Current?.RequestedTheme ?? AppTheme.Light;
                    return currentAppTheme == AppTheme.Dark ? _darkTheme : _lightTheme;
                }
            }
        }

        /// <summary>
        /// Initializes the dialog service with custom themes
        /// </summary>
        /// <param name="lightTheme">Custom light theme</param>
        /// <param name="darkTheme">Custom dark theme</param>
        public void Initialize(DialogTheme? lightTheme = null, DialogTheme? darkTheme = null)
        {
            lock (_stateLock)
            {
                if (lightTheme != null)
                    _lightTheme = lightTheme;

                if (darkTheme != null)
                    _darkTheme = darkTheme;
            }
        }

        /// <summary>
        /// Sets a custom localization provider
        /// </summary>
        /// <param name="localization">The localization provider</param>
        public void SetLocalization(IDialogLocalization localization)
        {
            lock (_stateLock)
            {
                _localization = localization ?? new DefaultDialogLocalization();
            }
        }

        /// <summary>
        /// Enables or disables the background overlay for all dialogs
        /// </summary>
        /// <param name="showOverlay">Whether to show the overlay</param>
        public void SetOverlayEnabled(bool showOverlay)
        {
            lock (_stateLock)
            {
                _lightTheme.ShowOverlay = showOverlay;
                _darkTheme.ShowOverlay = showOverlay;
                if (_currentThemeOverride != null)
                {
                    _currentThemeOverride.ShowOverlay = showOverlay;
                }
            }
        }

        /// <summary>
        /// Sets the overlay color and opacity for all themes
        /// </summary>
        /// <param name="color">The overlay color (should include alpha for transparency)</param>
        public void SetOverlayColor(Color color)
        {
            lock (_stateLock)
            {
                _lightTheme.OverlayColor = color;
                _darkTheme.OverlayColor = color;
                if (_currentThemeOverride != null)
                {
                    _currentThemeOverride.OverlayColor = color;
                }
            }
        }

        /// <summary>
        /// Registers a custom icon for a dialog type
        /// </summary>
        /// <param name="dialogType">The dialog type</param>
        /// <param name="lightIcon">Light theme icon source</param>
        /// <param name="darkIcon">Dark theme icon source</param>
        public void RegisterCustomIcon(DialogType dialogType, string lightIcon, string darkIcon)
        {
            lock (_stateLock)
            {
                _customIcons[dialogType] = new DialogIconMapping(lightIcon, darkIcon);
            }
        }

        /// <summary>
        /// Gets the icon for a dialog type and theme
        /// </summary>
        /// <param name="dialogType">The dialog type</param>
        /// <param name="isDarkTheme">Whether using dark theme</param>
        /// <returns>The icon source or null</returns>
        public string? GetDialogIcon(DialogType dialogType, bool isDarkTheme)
        {
            // Check custom icons first (thread-safe access)
            lock (_stateLock)
            {
                if (_customIcons.TryGetValue(dialogType, out var customIcon))
                {
                    return isDarkTheme ? customIcon.DarkIcon : customIcon.LightIcon;
                }
            }

            // Return default icons (using PNG for reliable NuGet distribution)
            return dialogType switch
            {
                DialogType.Error => isDarkTheme ? "error_outline_white_48dp.png" : "error_outline_black_48dp.png",
                DialogType.Warning => isDarkTheme ? "warning_amber_white_48dp.png" : "warning_amber_black_48dp.png",
                DialogType.Success => isDarkTheme ? "task_alt_white_48dp.png" : "task_alt_black_48dp.png",
                DialogType.Info => isDarkTheme ? "info_white_48dp.png" : "info_black_48dp.png",
                DialogType.Help => isDarkTheme ? "help_outline_white_48dp.png" : "help_outline_black_48dp.png",
                DialogType.Decide => isDarkTheme ? "fork_right_white_48dp.png" : "fork_right_black_48dp.png",
                DialogType.Stop => isDarkTheme ? "pan_tool_white_48dp.png" : "pan_tool_black_48dp.png",
                _ => null
            };
        }

        /// <summary>
        /// Creates a themed style dictionary for the current theme
        /// </summary>
        /// <returns>A resource dictionary with themed styles</returns>
        public ResourceDictionary CreateThemedStyles()
        {
            var theme = CurrentTheme;
            var resources = new ResourceDictionary();

            // Background colors
            resources["DialogBackgroundColor"] = theme.BackgroundColor;
            resources["DialogOverlayColor"] = theme.OverlayColor;
            resources["DialogBorderColor"] = theme.BorderColor;

            // Text colors
            resources["DialogTitleTextColor"] = theme.TitleTextColor;
            resources["DialogDescriptionTextColor"] = theme.DescriptionTextColor;
            resources["DialogButtonTextColor"] = theme.ButtonTextColor;
            resources["DialogSecondaryButtonTextColor"] = theme.SecondaryButtonTextColor;

            // Button colors
            resources["DialogButtonBackgroundColor"] = theme.ButtonBackgroundColor;
            resources["DialogButtonBorderColor"] = theme.ButtonBorderColor;
            resources["DialogSecondaryButtonBackgroundColor"] = theme.SecondaryButtonBackgroundColor;
            resources["DialogSecondaryButtonBorderColor"] = theme.SecondaryButtonBorderColor;

            // Font properties
            resources["DialogTitleFontSize"] = theme.TitleFontSize;
            resources["DialogDescriptionFontSize"] = theme.DescriptionFontSize;
            resources["DialogButtonFontSize"] = theme.ButtonFontSize;

            // Dimensions
            resources["DialogWidth"] = theme.DialogWidth;
            resources["DialogHeight"] = theme.DialogHeight;
            resources["DialogCornerRadius"] = theme.DialogCornerRadius;
            resources["DialogPadding"] = theme.DialogPadding;
            resources["DialogButtonHeight"] = theme.ButtonHeight;

            return resources;
        }

        /// <summary>
        /// Resets the service to default settings
        /// </summary>
        public void Reset()
        {
            lock (_stateLock)
            {
                _lightTheme = DialogTheme.LightTheme;
                _darkTheme = DialogTheme.DarkTheme;
                _useSystemTheme = true;
                _currentThemeOverride = null;
                _localization = new DefaultDialogLocalization();
                _logger = NullDialogLogger.Instance;
                _customIcons.Clear();
            }
        }

        /// <summary>
        /// Logs an error safely, catching any logging exceptions
        /// </summary>
        internal void LogError(string message, Exception? exception = null)
        {
            try
            {
                if (exception != null)
                    Logger.Error(message, exception);
                else
                    Logger.Error(message);
            }
            catch
            {
                // Ignore logging failures to prevent cascading errors
            }
        }

        /// <summary>
        /// Logs a warning safely, catching any logging exceptions
        /// </summary>
        internal void LogWarning(string message)
        {
            try
            {
                Logger.Warning(message);
            }
            catch
            {
                // Ignore logging failures
            }
        }

        /// <summary>
        /// Logs debug info safely, catching any logging exceptions
        /// </summary>
        internal void LogDebug(string message)
        {
            try
            {
                Logger.Debug(message);
            }
            catch
            {
                // Ignore logging failures
            }
        }
    }

    /// <summary>
    /// Represents a mapping of icons for light and dark themes
    /// </summary>
    public class DialogIconMapping
    {
        public string LightIcon { get; }
        public string DarkIcon { get; }

        public DialogIconMapping(string lightIcon, string darkIcon)
        {
            LightIcon = lightIcon;
            DarkIcon = darkIcon;
        }
    }
}