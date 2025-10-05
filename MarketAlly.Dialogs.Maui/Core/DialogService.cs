using MarketAlly.Dialogs.Maui.Interfaces;
using MarketAlly.Dialogs.Maui.Localization;
using MarketAlly.Dialogs.Maui.Models;

namespace MarketAlly.Dialogs.Maui.Core
{
    /// <summary>
    /// Main service for managing dialogs with theming and localization support
    /// </summary>
    public class DialogService
    {
        private static DialogService? _instance;

        /// <summary>
        /// Gets the singleton instance of the DialogService
        /// </summary>
        public static DialogService Instance => _instance ??= new DialogService();

        /// <summary>
        /// Gets or sets the light theme for dialogs
        /// </summary>
        public DialogTheme LightTheme { get; set; } = DialogTheme.LightTheme;

        /// <summary>
        /// Gets or sets the dark theme for dialogs
        /// </summary>
        public DialogTheme DarkTheme { get; set; } = DialogTheme.DarkTheme;

        /// <summary>
        /// Gets or sets whether to automatically use the system theme
        /// </summary>
        public bool UseSystemTheme { get; set; } = true;

        /// <summary>
        /// Gets or sets the current theme override (null to use system theme)
        /// </summary>
        public DialogTheme? CurrentThemeOverride { get; set; }

        /// <summary>
        /// Gets or sets the localization provider
        /// </summary>
        public IDialogLocalization Localization { get; set; } = new DefaultDialogLocalization();

        /// <summary>
        /// Gets or sets custom icon mappings for dialog types
        /// </summary>
        public Dictionary<DialogType, DialogIconMapping> CustomIcons { get; set; } = new();

        /// <summary>
        /// Gets the current active theme based on settings and system theme
        /// </summary>
        public DialogTheme CurrentTheme
        {
            get
            {
                if (CurrentThemeOverride != null)
                    return CurrentThemeOverride;

                if (!UseSystemTheme)
                    return LightTheme;

                var currentAppTheme = Application.Current?.RequestedTheme ?? AppTheme.Light;
                return currentAppTheme == AppTheme.Dark ? DarkTheme : LightTheme;
            }
        }

        /// <summary>
        /// Initializes the dialog service with custom themes
        /// </summary>
        /// <param name="lightTheme">Custom light theme</param>
        /// <param name="darkTheme">Custom dark theme</param>
        public void Initialize(DialogTheme? lightTheme = null, DialogTheme? darkTheme = null)
        {
            if (lightTheme != null)
                LightTheme = lightTheme;

            if (darkTheme != null)
                DarkTheme = darkTheme;
        }

        /// <summary>
        /// Sets a custom localization provider
        /// </summary>
        /// <param name="localization">The localization provider</param>
        public void SetLocalization(IDialogLocalization localization)
        {
            Localization = localization ?? new DefaultDialogLocalization();
        }

        /// <summary>
        /// Enables or disables the background overlay for all dialogs
        /// </summary>
        /// <param name="showOverlay">Whether to show the overlay</param>
        public void SetOverlayEnabled(bool showOverlay)
        {
            LightTheme.ShowOverlay = showOverlay;
            DarkTheme.ShowOverlay = showOverlay;
            if (CurrentThemeOverride != null)
            {
                CurrentThemeOverride.ShowOverlay = showOverlay;
            }
        }

        /// <summary>
        /// Sets the overlay color and opacity for all themes
        /// </summary>
        /// <param name="color">The overlay color (should include alpha for transparency)</param>
        public void SetOverlayColor(Color color)
        {
            LightTheme.OverlayColor = color;
            DarkTheme.OverlayColor = color;
            if (CurrentThemeOverride != null)
            {
                CurrentThemeOverride.OverlayColor = color;
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
            CustomIcons[dialogType] = new DialogIconMapping(lightIcon, darkIcon);
        }

        /// <summary>
        /// Gets the icon for a dialog type and theme
        /// </summary>
        /// <param name="dialogType">The dialog type</param>
        /// <param name="isDarkTheme">Whether using dark theme</param>
        /// <returns>The icon source or null</returns>
        public string? GetDialogIcon(DialogType dialogType, bool isDarkTheme)
        {
            // Check custom icons first
            if (CustomIcons.TryGetValue(dialogType, out var customIcon))
            {
                return isDarkTheme ? customIcon.DarkIcon : customIcon.LightIcon;
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
            LightTheme = DialogTheme.LightTheme;
            DarkTheme = DialogTheme.DarkTheme;
            UseSystemTheme = true;
            CurrentThemeOverride = null;
            Localization = new DefaultDialogLocalization();
            CustomIcons.Clear();
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