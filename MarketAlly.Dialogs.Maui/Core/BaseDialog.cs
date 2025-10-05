using MarketAlly.Dialogs.Maui.Models;
using Mopups.Pages;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.IO;

namespace MarketAlly.Dialogs.Maui.Core
{
    /// <summary>
    /// Base class for all dialog implementations
    /// </summary>
    public abstract class BaseDialog : PopupPage
    {
        protected DialogService DialogService => DialogService.Instance;
        protected DialogTheme CurrentTheme => DialogService.CurrentTheme;
        protected DialogType DialogType { get; set; }

        /// <summary>
        /// Gets or sets custom icon sources
        /// </summary>
        public string? CustomLightIcon { get; set; }
        public string? CustomDarkIcon { get; set; }

        /// <summary>
        /// Gets or sets the padding for description labels
        /// </summary>
        public Thickness DescriptionPadding { get; set; } = new Thickness(10, 5);

        protected BaseDialog()
        {
            InitializeBase();
        }

        private void InitializeBase()
        {
            // Set base properties
            BackgroundInputTransparent = false;
            CloseWhenBackgroundIsClicked = false;
            HasSystemPadding = true;

            var theme = CurrentTheme;
            // Apply overlay color if enabled, otherwise transparent
            BackgroundColor = theme.ShowOverlay ? theme.OverlayColor : Colors.Transparent;

            // Apply theme when appearing
            Appearing += OnDialogAppearing;
        }

        private void OnDialogAppearing(object? sender, EventArgs e)
        {
            ApplyTheme();
        }

        /// <summary>
        /// Override back button behavior to prevent dismissal by default
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            return HandleBackButton();
        }

        /// <summary>
        /// Override to customize back button behavior
        /// </summary>
        protected virtual bool HandleBackButton()
        {
            // Return true to prevent back button from dismissing the dialog
            return true;
        }

        /// <summary>
        /// Applies the current theme to the dialog
        /// </summary>
        protected virtual void ApplyTheme()
        {
            var theme = CurrentTheme;

            // Apply theme to the dialog resources
            foreach (var kvp in DialogService.CreateThemedStyles())
            {
                Resources[kvp.Key] = kvp.Value;
            }

            OnThemeApplied(theme);
        }

        /// <summary>
        /// Override to apply custom theme properties
        /// </summary>
        protected virtual void OnThemeApplied(DialogTheme theme)
        {
        }

        /// <summary>
        /// Gets the appropriate icon source for the current theme
        /// </summary>
        protected ImageSource? GetDialogIcon()
        {
            // Use the DialogService's current theme which respects overrides
            var currentTheme = DialogService.CurrentTheme;
            var isDarkTheme = currentTheme == DialogService.DarkTheme;

            // Check for custom icons first
            if (!string.IsNullOrEmpty(CustomLightIcon) && !string.IsNullOrEmpty(CustomDarkIcon))
            {
                var customSource = isDarkTheme ? CustomDarkIcon : CustomLightIcon;
                return ImageSource.FromFile(customSource);
            }

            // Use dialog type icons
            var iconSource = DialogService.GetDialogIcon(DialogType, isDarkTheme);
            if (string.IsNullOrEmpty(iconSource))
                return null;

            // Create a cache key based on the icon source
            var cacheKey = $"{iconSource}_{isDarkTheme}";

            return ImageCache.GetOrCreate(cacheKey, () =>
            {
                // ALWAYS use embedded resources to ensure icons work in NuGet consumers
                var pngFileName = System.IO.Path.GetFileName(iconSource);
                var resourceName = $"MarketAlly.Dialogs.Maui.Resources.Images.{pngFileName}";

                // Check if resource exists first (fast cached check)
                if (ImageCache.ResourceExists(resourceName))
                {
                    // Get cached bytes or load them
                    var buffer = ImageCache.GetResourceBytes(resourceName);
                    if (buffer != null)
                    {
                        // Return a stream source that creates a new stream each time
                        return ImageSource.FromStream(() => new System.IO.MemoryStream(buffer));
                    }
                }

                // Fallback for development - only if embedded resource not found
                // This allows the test project to work during development
                var imageName = iconSource.Replace(".svg", "").Replace(".png", "");
                try
                {
#if WINDOWS
                    // Windows needs the .png extension for MauiImage
                    var windowsImageName = imageName + ".png";
                    return ImageSource.FromFile(windowsImageName);
#else
                    // Android/iOS work with just the base name (no extension)
                    return ImageSource.FromFile(imageName);
#endif
                }
                catch
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Creates a themed border for the dialog content
        /// </summary>
        protected Border CreateThemedFrame(View content)
        {
            var theme = CurrentTheme;

            return new Border
            {
                Margin = new Thickness(theme.DialogPadding),
                HeightRequest = theme.DialogHeight,
                WidthRequest = theme.DialogWidth,
                MinimumWidthRequest = 250,
                BackgroundColor = theme.BackgroundColor,
                Stroke = new SolidColorBrush(theme.BorderColor),
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(theme.DialogCornerRadius)
                },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = 0,
                Shadow = theme.HasShadow ? new Shadow
                {
                    Brush = new SolidColorBrush(Colors.Black),
                    Offset = new Point(5, 5),
                    Radius = 10,
                    Opacity = 0.3f
                } : null,
                Content = content
            };
        }

        /// <summary>
        /// Creates a themed title label
        /// </summary>
        protected Label CreateTitleLabel(string text = "")
        {
            var theme = CurrentTheme;

            return new Label
            {
                Text = text,
                TextColor = theme.TitleTextColor,
                FontSize = theme.TitleFontSize,
                FontAttributes = theme.TitleFontAttributes,
                FontFamily = theme.TitleFontFamily,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Start
            };
        }

        /// <summary>
        /// Creates a themed description label
        /// </summary>
        protected Label CreateDescriptionLabel(string text = "")
        {
            var theme = CurrentTheme;

            return new Label
            {
                Text = text,
                TextColor = theme.DescriptionTextColor,
                FontSize = theme.DescriptionFontSize,
                FontFamily = theme.DescriptionFontFamily,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                MaxLines = 3,
                Padding = DescriptionPadding
            };
        }

        /// <summary>
        /// Creates a themed primary button
        /// </summary>
        protected Button CreatePrimaryButton(string text, EventHandler clickHandler)
        {
            var theme = CurrentTheme;

            var button = new Button
            {
                Text = text,
                TextColor = theme.ButtonTextColor,
                BackgroundColor = theme.ButtonBackgroundColor,
                BorderColor = theme.ButtonBorderColor,
                FontSize = theme.ButtonFontSize,
                FontFamily = theme.ButtonFontFamily,
                HeightRequest = theme.ButtonHeight,
                VerticalOptions = LayoutOptions.End
            };

            button.Clicked += clickHandler;
            return button;
        }

        /// <summary>
        /// Creates a themed secondary button
        /// </summary>
        protected Button CreateSecondaryButton(string text, EventHandler clickHandler)
        {
            var theme = CurrentTheme;

            var button = new Button
            {
                Text = text,
                TextColor = theme.SecondaryButtonTextColor,
                BackgroundColor = theme.SecondaryButtonBackgroundColor,
                BorderColor = theme.SecondaryButtonBorderColor,
                BorderWidth = 1,
                FontSize = theme.ButtonFontSize,
                FontFamily = theme.ButtonFontFamily,
                HeightRequest = theme.ButtonHeight,
                VerticalOptions = LayoutOptions.End
            };

            button.Clicked += clickHandler;
            return button;
        }

        /// <summary>
        /// Creates a separator line
        /// </summary>
        protected BoxView CreateSeparator()
        {
            var theme = CurrentTheme;

            return new BoxView
            {
                BackgroundColor = theme.BorderColor,
                HeightRequest = 1,
                Margin = new Thickness(20, 0)
            };
        }
    }
}