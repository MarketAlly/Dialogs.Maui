using Microsoft.Maui.Graphics;

namespace MarketAlly.Dialogs.Maui.Models
{
    /// <summary>
    /// Defines the theme properties for dialog appearance
    /// </summary>
    public class DialogTheme
    {
        // Background colors
        public Color BackgroundColor { get; set; } = Color.FromRgba("#FFFFFF");
        public Color OverlayColor { get; set; } = Color.FromRgba("#80000000");
        public Color BorderColor { get; set; } = Color.FromRgba("#CCCCCC");
        public bool ShowOverlay { get; set; } = true;

        // Text colors
        public Color TitleTextColor { get; set; } = Color.FromRgba("#000000");
        public Color DescriptionTextColor { get; set; } = Color.FromRgba("#333333");
        public Color ButtonTextColor { get; set; } = Color.FromRgba("#FFFFFF");
        public Color SecondaryButtonTextColor { get; set; } = Color.FromRgba("#000000");

        // Button colors
        public Color ButtonBackgroundColor { get; set; } = Color.FromRgba("#007AFF");
        public Color ButtonBorderColor { get; set; } = Color.FromRgba("#007AFF");
        public Color SecondaryButtonBackgroundColor { get; set; } = Color.FromRgba("#F5F5F5");
        public Color SecondaryButtonBorderColor { get; set; } = Color.FromRgba("#CCCCCC");

        // Font properties
        public string TitleFontFamily { get; set; } = string.Empty;
        public double TitleFontSize { get; set; } = 16;
        public FontAttributes TitleFontAttributes { get; set; } = FontAttributes.Bold;

        public string DescriptionFontFamily { get; set; } = string.Empty;
        public double DescriptionFontSize { get; set; } = 14;

        public string ButtonFontFamily { get; set; } = string.Empty;
        public double ButtonFontSize { get; set; } = 14;

        // Dimensions
        public double DialogWidth { get; set; } = 300;
        public double DialogHeight { get; set; } = 250;
        public double DialogCornerRadius { get; set; } = 8;
        public double DialogPadding { get; set; } = 20;
        public double ButtonHeight { get; set; } = 44;

        // Animation
        public uint AnimationDuration { get; set; } = 250;
        public bool EnableAnimation { get; set; } = true;

        // Shadow
        public bool HasShadow { get; set; } = true;

        /// <summary>
        /// Creates a default light theme
        /// </summary>
        public static DialogTheme LightTheme => new DialogTheme();

        /// <summary>
        /// Creates a default dark theme
        /// </summary>
        public static DialogTheme DarkTheme => new DialogTheme
        {
            BackgroundColor = Color.FromRgba("#1C1C1E"),
            OverlayColor = Color.FromRgba("#80000000"),
            BorderColor = Color.FromRgba("#3A3A3C"),
            ShowOverlay = true,
            TitleTextColor = Color.FromRgba("#FFFFFF"),
            DescriptionTextColor = Color.FromRgba("#EBEBF5"),
            ButtonTextColor = Color.FromRgba("#FFFFFF"),
            SecondaryButtonTextColor = Color.FromRgba("#FFFFFF"),
            ButtonBackgroundColor = Color.FromRgba("#0A84FF"),
            ButtonBorderColor = Color.FromRgba("#0A84FF"),
            SecondaryButtonBackgroundColor = Color.FromRgba("#2C2C2E"),
            SecondaryButtonBorderColor = Color.FromRgba("#3A3A3C")
        };

        /// <summary>
        /// Clones the current theme
        /// </summary>
        public DialogTheme Clone()
        {
            return new DialogTheme
            {
                BackgroundColor = BackgroundColor,
                OverlayColor = OverlayColor,
                BorderColor = BorderColor,
                ShowOverlay = ShowOverlay,
                TitleTextColor = TitleTextColor,
                DescriptionTextColor = DescriptionTextColor,
                ButtonTextColor = ButtonTextColor,
                SecondaryButtonTextColor = SecondaryButtonTextColor,
                ButtonBackgroundColor = ButtonBackgroundColor,
                ButtonBorderColor = ButtonBorderColor,
                SecondaryButtonBackgroundColor = SecondaryButtonBackgroundColor,
                SecondaryButtonBorderColor = SecondaryButtonBorderColor,
                TitleFontFamily = TitleFontFamily,
                TitleFontSize = TitleFontSize,
                TitleFontAttributes = TitleFontAttributes,
                DescriptionFontFamily = DescriptionFontFamily,
                DescriptionFontSize = DescriptionFontSize,
                ButtonFontFamily = ButtonFontFamily,
                ButtonFontSize = ButtonFontSize,
                DialogWidth = DialogWidth,
                DialogHeight = DialogHeight,
                DialogCornerRadius = DialogCornerRadius,
                DialogPadding = DialogPadding,
                ButtonHeight = ButtonHeight,
                AnimationDuration = AnimationDuration,
                EnableAnimation = EnableAnimation
            };
        }
    }
}