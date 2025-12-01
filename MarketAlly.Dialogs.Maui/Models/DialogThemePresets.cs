using Microsoft.Maui.Graphics;

namespace MarketAlly.Dialogs.Maui.Models
{
    /// <summary>
    /// Provides preset themes following popular design systems
    /// </summary>
    public static class DialogThemePresets
    {
        #region Material Design 3

        /// <summary>
        /// Material Design 3 Light theme
        /// </summary>
        public static DialogTheme MaterialLight => new DialogTheme
        {
            // Colors from Material Design 3
            BackgroundColor = Color.FromRgba("#FFFBFE"),
            OverlayColor = Color.FromRgba("#4D000000"),
            BorderColor = Color.FromRgba("#E7E0EC"),
            ShowOverlay = true,

            TitleTextColor = Color.FromRgba("#1C1B1F"),
            DescriptionTextColor = Color.FromRgba("#49454F"),
            ButtonTextColor = Color.FromRgba("#FFFFFF"),
            SecondaryButtonTextColor = Color.FromRgba("#6750A4"),

            ButtonBackgroundColor = Color.FromRgba("#6750A4"),
            ButtonBorderColor = Color.FromRgba("#6750A4"),
            SecondaryButtonBackgroundColor = Colors.Transparent,
            SecondaryButtonBorderColor = Color.FromRgba("#6750A4"),

            // Material typography
            TitleFontSize = 24,
            TitleFontAttributes = FontAttributes.Bold,
            DescriptionFontSize = 14,
            ButtonFontSize = 14,

            // Material dimensions - larger corner radius is the signature
            DialogWidth = 312,
            DialogHeight = 250,
            DialogCornerRadius = 28,
            DialogPadding = 24,
            ButtonHeight = 40,
            ButtonCornerRadius = 20,

            // Material animation (200ms standard)
            AnimationDuration = 200,
            EnableAnimation = true,

            HasShadow = true,
            IsDarkMode = false
        };

        /// <summary>
        /// Material Design 3 Dark theme
        /// </summary>
        public static DialogTheme MaterialDark => new DialogTheme
        {
            BackgroundColor = Color.FromRgba("#2D2D30"),
            OverlayColor = Color.FromRgba("#4D000000"),
            BorderColor = Color.FromRgba("#49454F"),
            ShowOverlay = true,

            TitleTextColor = Color.FromRgba("#E6E1E5"),
            DescriptionTextColor = Color.FromRgba("#CAC4D0"),
            ButtonTextColor = Color.FromRgba("#381E72"),
            SecondaryButtonTextColor = Color.FromRgba("#D0BCFF"),

            ButtonBackgroundColor = Color.FromRgba("#D0BCFF"),
            ButtonBorderColor = Color.FromRgba("#D0BCFF"),
            SecondaryButtonBackgroundColor = Colors.Transparent,
            SecondaryButtonBorderColor = Color.FromRgba("#D0BCFF"),

            TitleFontSize = 24,
            TitleFontAttributes = FontAttributes.Bold,
            DescriptionFontSize = 14,
            ButtonFontSize = 14,

            DialogWidth = 312,
            DialogHeight = 250,
            DialogCornerRadius = 28,
            DialogPadding = 24,
            ButtonHeight = 40,
            ButtonCornerRadius = 20,

            AnimationDuration = 200,
            EnableAnimation = true,

            HasShadow = true,
            IsDarkMode = true
        };

        #endregion

        #region Microsoft Fluent Design

        /// <summary>
        /// Microsoft Fluent Design Light theme
        /// </summary>
        public static DialogTheme FluentLight => new DialogTheme
        {
            // Fluent Design System colors
            BackgroundColor = Color.FromRgba("#FFFFFF"),
            OverlayColor = Color.FromRgba("#4D000000"),
            BorderColor = Color.FromRgba("#D6D6D6"),
            ShowOverlay = true,

            TitleTextColor = Color.FromRgba("#000000"),
            DescriptionTextColor = Color.FromRgba("#323130"),
            ButtonTextColor = Color.FromRgba("#FFFFFF"),
            SecondaryButtonTextColor = Color.FromRgba("#323130"),

            ButtonBackgroundColor = Color.FromRgba("#0078D4"),
            ButtonBorderColor = Color.FromRgba("#0078D4"),
            SecondaryButtonBackgroundColor = Color.FromRgba("#F3F2F1"),
            SecondaryButtonBorderColor = Color.FromRgba("#8A8886"),

            // Fluent typography - semibold not bold
            TitleFontSize = 20,
            TitleFontAttributes = FontAttributes.None,
            DescriptionFontSize = 14,
            ButtonFontSize = 14,

            // Fluent dimensions - more squared corners
            DialogWidth = 340,
            DialogHeight = 250,
            DialogCornerRadius = 8,
            DialogPadding = 24,
            ButtonHeight = 32,
            ButtonCornerRadius = 4,

            // Fluent animation (167ms fast)
            AnimationDuration = 167,
            EnableAnimation = true,

            HasShadow = true,
            IsDarkMode = false
        };

        /// <summary>
        /// Microsoft Fluent Design Dark theme
        /// </summary>
        public static DialogTheme FluentDark => new DialogTheme
        {
            BackgroundColor = Color.FromRgba("#2D2D2D"),
            OverlayColor = Color.FromRgba("#4D000000"),
            BorderColor = Color.FromRgba("#404040"),
            ShowOverlay = true,

            TitleTextColor = Color.FromRgba("#FFFFFF"),
            DescriptionTextColor = Color.FromRgba("#D2D0CE"),
            ButtonTextColor = Color.FromRgba("#000000"),
            SecondaryButtonTextColor = Color.FromRgba("#FFFFFF"),

            ButtonBackgroundColor = Color.FromRgba("#4CC2FF"),
            ButtonBorderColor = Color.FromRgba("#4CC2FF"),
            SecondaryButtonBackgroundColor = Color.FromRgba("#3D3D3D"),
            SecondaryButtonBorderColor = Color.FromRgba("#6B6B6B"),

            TitleFontSize = 20,
            TitleFontAttributes = FontAttributes.None,
            DescriptionFontSize = 14,
            ButtonFontSize = 14,

            DialogWidth = 340,
            DialogHeight = 250,
            DialogCornerRadius = 8,
            DialogPadding = 24,
            ButtonHeight = 32,
            ButtonCornerRadius = 4,

            AnimationDuration = 167,
            EnableAnimation = true,

            HasShadow = true,
            IsDarkMode = true
        };

        #endregion

        #region Apple Cupertino (iOS/macOS)

        /// <summary>
        /// Apple Cupertino (iOS) Light theme
        /// </summary>
        public static DialogTheme CupertinoLight => new DialogTheme
        {
            // iOS Human Interface Guidelines colors
            BackgroundColor = Color.FromRgba("#F2F2F7"),
            OverlayColor = Color.FromRgba("#4D000000"),
            BorderColor = Color.FromRgba("#C6C6C8"),
            ShowOverlay = true,

            TitleTextColor = Color.FromRgba("#000000"),
            DescriptionTextColor = Color.FromRgba("#3C3C43"),
            ButtonTextColor = Color.FromRgba("#FFFFFF"),
            SecondaryButtonTextColor = Color.FromRgba("#007AFF"),

            ButtonBackgroundColor = Color.FromRgba("#007AFF"),
            ButtonBorderColor = Color.FromRgba("#007AFF"),
            SecondaryButtonBackgroundColor = Colors.Transparent,
            SecondaryButtonBorderColor = Colors.Transparent,

            // iOS typography (SF Pro) - compact
            TitleFontSize = 17,
            TitleFontAttributes = FontAttributes.Bold,
            DescriptionFontSize = 13,
            ButtonFontSize = 17,

            // iOS dimensions - narrower, rounded buttons
            DialogWidth = 270,
            DialogHeight = 250,
            DialogCornerRadius = 14,
            DialogPadding = 16,
            ButtonHeight = 44,
            ButtonCornerRadius = 10,

            // iOS animation (250ms)
            AnimationDuration = 250,
            EnableAnimation = true,

            HasShadow = false, // iOS alerts typically don't have shadows
            IsDarkMode = false
        };

        /// <summary>
        /// Apple Cupertino (iOS) Dark theme
        /// </summary>
        public static DialogTheme CupertinoDark => new DialogTheme
        {
            BackgroundColor = Color.FromRgba("#1C1C1E"),
            OverlayColor = Color.FromRgba("#4D000000"),
            BorderColor = Color.FromRgba("#38383A"),
            ShowOverlay = true,

            TitleTextColor = Color.FromRgba("#FFFFFF"),
            DescriptionTextColor = Color.FromRgba("#EBEBF5"),
            ButtonTextColor = Color.FromRgba("#FFFFFF"),
            SecondaryButtonTextColor = Color.FromRgba("#0A84FF"),

            ButtonBackgroundColor = Color.FromRgba("#0A84FF"),
            ButtonBorderColor = Color.FromRgba("#0A84FF"),
            SecondaryButtonBackgroundColor = Colors.Transparent,
            SecondaryButtonBorderColor = Colors.Transparent,

            TitleFontSize = 17,
            TitleFontAttributes = FontAttributes.Bold,
            DescriptionFontSize = 13,
            ButtonFontSize = 17,

            DialogWidth = 270,
            DialogHeight = 250,
            DialogCornerRadius = 14,
            DialogPadding = 16,
            ButtonHeight = 44,
            ButtonCornerRadius = 10,

            AnimationDuration = 250,
            EnableAnimation = true,

            HasShadow = false,
            IsDarkMode = true
        };

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets a theme pair (light and dark) for the specified design system
        /// </summary>
        /// <param name="system">The design system</param>
        /// <returns>A tuple containing light and dark themes</returns>
        public static (DialogTheme Light, DialogTheme Dark) GetThemePair(DesignSystem system)
        {
            return system switch
            {
                DesignSystem.Material => (MaterialLight, MaterialDark),
                DesignSystem.Fluent => (FluentLight, FluentDark),
                DesignSystem.Cupertino => (CupertinoLight, CupertinoDark),
                _ => (DialogTheme.LightTheme, DialogTheme.DarkTheme)
            };
        }

        /// <summary>
        /// Gets the appropriate theme for the design system based on dark mode setting
        /// </summary>
        /// <param name="system">The design system</param>
        /// <param name="isDark">Whether to get the dark variant</param>
        /// <returns>The requested theme</returns>
        public static DialogTheme GetTheme(DesignSystem system, bool isDark)
        {
            return system switch
            {
                DesignSystem.Material => isDark ? MaterialDark : MaterialLight,
                DesignSystem.Fluent => isDark ? FluentDark : FluentLight,
                DesignSystem.Cupertino => isDark ? CupertinoDark : CupertinoLight,
                _ => isDark ? DialogTheme.DarkTheme : DialogTheme.LightTheme
            };
        }

        /// <summary>
        /// Applies a preset theme to the DialogService
        /// </summary>
        /// <param name="system">The design system to apply</param>
        /// <param name="isDark">Whether to apply the dark variant</param>
        public static void ApplyPreset(DesignSystem system, bool isDark = false)
        {
            var theme = GetTheme(system, isDark);
            Core.DialogService.Instance.CurrentThemeOverride = theme;
        }

        #endregion
    }

    /// <summary>
    /// Defines the available design systems for preset themes
    /// </summary>
    public enum DesignSystem
    {
        /// <summary>
        /// Default library theme
        /// </summary>
        Default,

        /// <summary>
        /// Google Material Design 3
        /// </summary>
        Material,

        /// <summary>
        /// Microsoft Fluent Design System
        /// </summary>
        Fluent,

        /// <summary>
        /// Apple Human Interface Guidelines (iOS/macOS)
        /// </summary>
        Cupertino
    }
}
