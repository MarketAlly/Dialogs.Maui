using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a dialog with multi-line text input (Editor)
    /// </summary>
    public class EditorDialog : BaseDialog
    {
        private readonly TaskCompletionSource<string?> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;
        private readonly Editor _inputEditor;
        private readonly Button _okButton;
        private readonly Button _cancelButton;
        private readonly Image _iconImage;
        private readonly ScrollView _scrollView;

        public EditorDialog(
            string title,
            string? description = null,
            string? placeholder = null,
            string? okText = null,
            string? cancelText = null,
            DialogType dialogType = DialogType.None,
            Keyboard? keyboard = null,
            int minLines = 3,
            int maxLines = 10)
        {
            DialogType = dialogType;

            // Create UI elements
            _iconImage = new Image
            {
                HeightRequest = 48,
                WidthRequest = 48,
                HorizontalOptions = LayoutOptions.Center,
                Source = GetDialogIcon()
            };

            _titleLabel = CreateTitleLabel(title);

            if (!string.IsNullOrEmpty(description))
            {
                _descriptionLabel = CreateDescriptionLabel(description);
            }
            else
            {
                _descriptionLabel = new Label { IsVisible = false };
            }

            var theme = CurrentTheme;
            _inputEditor = new Editor
            {
                Placeholder = placeholder,
                FontSize = theme.DescriptionFontSize,
                TextColor = theme.DescriptionTextColor,
                PlaceholderColor = theme.DescriptionTextColor.WithAlpha(0.5f),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Keyboard = keyboard ?? Keyboard.Default,
                AutoSize = EditorAutoSizeOption.TextChanges,
                MinimumHeightRequest = minLines * 20, // Approximate line height
                MaximumHeightRequest = maxLines * 20
            };

            // Wrap editor in a scrollview for long content
            _scrollView = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = _inputEditor
            };

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
                    new RowDefinition(GridLength.Auto),  // Icon
                    new RowDefinition(GridLength.Auto),  // Title
                    new RowDefinition(GridLength.Auto),  // Description
                    new RowDefinition(1),                // Separator
                    new RowDefinition(GridLength.Star),  // Editor in ScrollView
                    new RowDefinition(GridLength.Auto)   // Buttons
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Star)
                }
            };

            int row = 0;

            // Add icon if dialog type is specified
            if (dialogType != DialogType.None)
            {
                grid.Add(_iconImage, 0, row);
                Grid.SetColumnSpan(_iconImage, 2);
                row++;
            }

            grid.Add(_titleLabel, 0, row);
            Grid.SetColumnSpan(_titleLabel, 2);
            row++;

            if (!string.IsNullOrEmpty(description))
            {
                grid.Add(_descriptionLabel, 0, row);
                Grid.SetColumnSpan(_descriptionLabel, 2);
                row++;
            }

            var separator = CreateSeparator();
            grid.Add(separator, 0, row);
            Grid.SetColumnSpan(separator, 2);
            row++;

            grid.Add(_scrollView, 0, row);
            Grid.SetColumnSpan(_scrollView, 2);
            row++;

            grid.Add(_cancelButton, 0, row);
            grid.Add(_okButton, 1, row);

            // Create frame with dynamic sizing
            var frame = CreateThemedFrame(grid);
            frame.MinimumHeightRequest = 350;
            frame.MaximumHeightRequest = 600;

            // Set content
            Content = frame;
        }

        /// <summary>
        /// Gets or sets the input text
        /// </summary>
        public string Text
        {
            get => _inputEditor.Text ?? string.Empty;
            set => _inputEditor.Text = value;
        }

        /// <summary>
        /// Gets or sets the placeholder text
        /// </summary>
        public string? Placeholder
        {
            get => _inputEditor.Placeholder;
            set => _inputEditor.Placeholder = value;
        }

        /// <summary>
        /// Gets or sets the keyboard type
        /// </summary>
        public Keyboard Keyboard
        {
            get => _inputEditor.Keyboard;
            set => _inputEditor.Keyboard = value;
        }

        /// <summary>
        /// Gets or sets whether spellcheck is enabled
        /// </summary>
        public bool IsSpellCheckEnabled
        {
            get => _inputEditor.IsSpellCheckEnabled;
            set => _inputEditor.IsSpellCheckEnabled = value;
        }

        /// <summary>
        /// Gets or sets whether text prediction is enabled
        /// </summary>
        public bool IsTextPredictionEnabled
        {
            get => _inputEditor.IsTextPredictionEnabled;
            set => _inputEditor.IsTextPredictionEnabled = value;
        }

        /// <summary>
        /// Shows an editor dialog with default settings
        /// </summary>
        public static async Task<string?> ShowAsync(
            string title,
            string? description = null,
            string? placeholder = null,
            DialogType type = DialogType.None)
        {
            return await ShowAsync(title, description, placeholder, null, null, type);
        }

        /// <summary>
        /// Shows an editor dialog with custom button text
        /// </summary>
        public static async Task<string?> ShowAsync(
            string title,
            string? description,
            string? placeholder,
            string? okText,
            string? cancelText,
            DialogType type = DialogType.None,
            int minLines = 3,
            int maxLines = 10)
        {
            // Check if an editor dialog is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is EditorDialog))
                return null;

            var dialog = new EditorDialog(title, description, placeholder, okText, cancelText, type, Keyboard.Text, minLines, maxLines);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Shows this instance of the dialog
        /// </summary>
        public async Task<string?> ShowAsync()
        {
            await MopupService.Instance.PushAsync(this);
            return await _taskCompletionSource.Task;
        }

        /// <summary>
        /// Hides the current editor dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is EditorDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
            }
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
            _inputEditor.TextColor = theme.DescriptionTextColor;
            _inputEditor.PlaceholderColor = theme.DescriptionTextColor.WithAlpha(0.5f);

            // Update icon if theme changes
            if (DialogType != DialogType.None)
            {
                _iconImage.Source = GetDialogIcon();
            }
        }

        private async void OnOkClicked(object? sender, EventArgs e)
        {
            _taskCompletionSource.TrySetResult(_inputEditor.Text ?? string.Empty);
            await MopupService.Instance.PopAsync();
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            _taskCompletionSource.TrySetResult(null);
            await MopupService.Instance.PopAsync();
        }
    }
}