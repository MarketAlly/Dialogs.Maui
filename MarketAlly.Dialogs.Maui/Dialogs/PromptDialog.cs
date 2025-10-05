using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.IO;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a prompt dialog for single-line text input (Entry)
    /// Use EditorDialog for multi-line text input
    /// </summary>
    public class PromptDialog : BaseDialog
    {
        private readonly TaskCompletionSource<string?> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly Entry _inputEntry;
        private readonly Button _okButton;
        private readonly Button _cancelButton;
        private readonly Image _iconImage;
        private readonly ImageButton? _togglePasswordButton;
        private readonly bool _isPasswordField;

        public PromptDialog(
            string title,
            string? placeholder = null,
            string? okText = null,
            string? cancelText = null,
            DialogType dialogType = DialogType.None,
            Keyboard? keyboard = null,
            bool isPassword = false)
        {
            DialogType = dialogType;
            _isPasswordField = isPassword;

            // Create UI elements
            _iconImage = new Image
            {
                HeightRequest = 48,
                WidthRequest = 48,
                HorizontalOptions = LayoutOptions.Center,
                Source = GetDialogIcon()
            };

            _titleLabel = CreateTitleLabel(title);

            var theme = CurrentTheme;
            _inputEntry = new Entry
            {
                Placeholder = placeholder,
                FontSize = theme.DescriptionFontSize,
                TextColor = theme.DescriptionTextColor,
                PlaceholderColor = theme.DescriptionTextColor.WithAlpha(0.5f),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Keyboard = keyboard ?? Keyboard.Default,
                IsPassword = isPassword
            };

            // Create password toggle button if this is a password field
            if (isPassword)
            {
                _togglePasswordButton = new ImageButton
                {
                    Source = GetPasswordToggleIcon(true),
                    BackgroundColor = Colors.Transparent,
                    WidthRequest = 24,
                    HeightRequest = 24,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(0, 0, 5, 0)
                };
                _togglePasswordButton.Clicked += OnTogglePasswordVisibility;
            }

            _okButton = CreatePrimaryButton(
                okText ?? DialogService.Localization.OkButtonText,
                OnOkClicked);

            _cancelButton = CreateSecondaryButton(
                cancelText ?? DialogService.Localization.CancelButtonText,
                OnCancelClicked);

            // Build layout
            var grid = new Grid
            {
                Padding = 10,
                BackgroundColor = Colors.Transparent,
                ColumnSpacing = 5,
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(1),
                    new RowDefinition(GridLength.Star),
                    new RowDefinition(GridLength.Auto)
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Star)
                }
            };

            // Add icon if dialog type is specified
            if (dialogType != DialogType.None)
            {
                grid.Add(_iconImage, 0, 0);
                Grid.SetColumnSpan(_iconImage, 2);
            }

            grid.Add(_titleLabel, 0, 1);
            Grid.SetColumnSpan(_titleLabel, 2);

            var separator = CreateSeparator();
            grid.Add(separator, 0, 2);
            Grid.SetColumnSpan(separator, 2);

            // Add input field with optional password toggle
            if (_isPasswordField && _togglePasswordButton != null)
            {
                var inputGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Auto)
                    }
                };
                inputGrid.Add(_inputEntry, 0, 0);
                inputGrid.Add(_togglePasswordButton, 1, 0);

                grid.Add(inputGrid, 0, 3);
                Grid.SetColumnSpan(inputGrid, 2);
            }
            else
            {
                grid.Add(_inputEntry, 0, 3);
                Grid.SetColumnSpan(_inputEntry, 2);
            }

            grid.Add(_cancelButton, 0, 4);
            grid.Add(_okButton, 1, 4);

            // Create frame with auto-sizing for prompt
            var frame = CreateThemedFrame(grid);
            frame.MinimumHeightRequest = 250;

            // Set content
            Content = frame;
        }

        /// <summary>
        /// Gets or sets the input text
        /// </summary>
        public string Text
        {
            get => _inputEntry.Text ?? string.Empty;
            set => _inputEntry.Text = value;
        }

        /// <summary>
        /// Gets or sets the placeholder text
        /// </summary>
        public string? Placeholder
        {
            get => _inputEntry.Placeholder;
            set => _inputEntry.Placeholder = value;
        }

        /// <summary>
        /// Gets or sets the keyboard type
        /// </summary>
        public Keyboard Keyboard
        {
            get => _inputEntry.Keyboard;
            set => _inputEntry.Keyboard = value;
        }

        /// <summary>
        /// Gets or sets whether the input is masked (for passwords)
        /// </summary>
        public bool IsPassword
        {
            get => _inputEntry.IsPassword;
            set => _inputEntry.IsPassword = value;
        }

        /// <summary>
        /// Shows a prompt dialog with default settings
        /// </summary>
        public static async Task<string?> ShowAsync(string title, string? placeholder = null, DialogType type = DialogType.None)
        {
            return await ShowAsync(title, placeholder, null, null, type);
        }

        /// <summary>
        /// Shows a prompt dialog with custom button text
        /// </summary>
        public static async Task<string?> ShowAsync(
            string title,
            string? placeholder,
            string? okText,
            string? cancelText,
            DialogType type = DialogType.None)
        {
            // Check if a prompt dialog is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is PromptDialog))
                return null;

            var dialog = new PromptDialog(title, placeholder, okText, cancelText, type);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Shows a prompt dialog with custom keyboard
        /// </summary>
        public static async Task<string?> ShowAsync(
            string title,
            string? placeholder,
            Keyboard keyboard,
            DialogType type = DialogType.None)
        {
            if (MopupService.Instance.PopupStack.Any(p => p is PromptDialog))
                return null;

            var dialog = new PromptDialog(title, placeholder, null, null, type, keyboard);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Hides the current prompt dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is PromptDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
            }
        }

        /// <summary>
        /// Shows this instance of the dialog
        /// </summary>
        public async Task<string?> ShowAsync()
        {
            await MopupService.Instance.PushAsync(this);
            return await _taskCompletionSource.Task;
        }

        protected override bool HandleBackButton()
        {
            // Back button acts as cancel
            _taskCompletionSource.TrySetResult(null);
            MopupService.Instance.PopAsync();
            return true;
        }

        protected override void OnThemeApplied(DialogTheme theme)
        {
            base.OnThemeApplied(theme);

            // Update editor colors
            _inputEntry.TextColor = theme.DescriptionTextColor;
            _inputEntry.PlaceholderColor = theme.DescriptionTextColor.WithAlpha(0.5f);

            // Update icon if theme changes
            if (DialogType != DialogType.None)
            {
                _iconImage.Source = GetDialogIcon();
            }
        }

        private async void OnOkClicked(object? sender, EventArgs e)
        {
            _taskCompletionSource.TrySetResult(_inputEntry.Text ?? string.Empty);
            await MopupService.Instance.PopAsync();
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            _taskCompletionSource.TrySetResult(null);
            await MopupService.Instance.PopAsync();
        }

        private void OnTogglePasswordVisibility(object? sender, EventArgs e)
        {
            if (_inputEntry != null)
            {
                _inputEntry.IsPassword = !_inputEntry.IsPassword;
                if (_togglePasswordButton != null)
                {
                    _togglePasswordButton.Source = GetPasswordToggleIcon(_inputEntry.IsPassword);
                }
            }
        }

        private ImageSource GetPasswordToggleIcon(bool isHidden)
        {
            var isDarkTheme = DialogService.CurrentTheme == DialogService.DarkTheme;

            // Load eye icons from cache
            var iconName = isHidden
                ? (isDarkTheme ? "eye_white.png" : "eye_black.png")
                : (isDarkTheme ? "eye_off_white.png" : "eye_off_black.png");

            var cacheKey = $"eye_{iconName}";

            return Core.ImageCache.GetOrCreate(cacheKey, () =>
            {
                // ALWAYS use embedded resource first for NuGet consumers
                var resourceName = $"MarketAlly.Dialogs.Maui.Resources.Images.{iconName}";

                if (Core.ImageCache.ResourceExists(resourceName))
                {
                    var buffer = Core.ImageCache.GetResourceBytes(resourceName);
                    if (buffer != null)
                    {
                        return ImageSource.FromStream(() => new MemoryStream(buffer));
                    }
                }

                // Fallback for development only
                try
                {
                    return ImageSource.FromFile(iconName);
                }
                catch
                {
                    return null;
                }
            }) ?? ImageSource.FromFile("");
        }
    }
}