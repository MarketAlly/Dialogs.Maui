using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a confirmation dialog with Yes/No or custom buttons
    /// </summary>
    public class ConfirmDialog : BaseDialog
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;
        private readonly Button _confirmButton;
        private readonly Button _cancelButton;
        private readonly Image _iconImage;
        private readonly Border _dialogFrame;
        private readonly Grid _mainGrid;

        public ConfirmDialog(
            string title,
            string description,
            string? confirmText = null,
            string? cancelText = null,
            DialogType dialogType = DialogType.None,
            double? dialogHeight = null)
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
            _descriptionLabel = CreateDescriptionLabel(description);
            _descriptionLabel.MaxLines = 4;

            _confirmButton = CreatePrimaryButton(
                confirmText ?? DialogService.Localization.YesButtonText,
                OnConfirmClicked);

            _cancelButton = CreateSecondaryButton(
                cancelText ?? DialogService.Localization.NoButtonText,
                OnCancelClicked);

            // Build layout
            _mainGrid = new Grid
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
                _mainGrid.Add(_iconImage, 0, 0);
                Grid.SetColumnSpan(_iconImage, 2);
            }

            _mainGrid.Add(_titleLabel, 0, 1);
            Grid.SetColumnSpan(_titleLabel, 2);

            var separator = CreateSeparator();
            _mainGrid.Add(separator, 0, 2);
            Grid.SetColumnSpan(separator, 2);

            _mainGrid.Add(_descriptionLabel, 0, 3);
            Grid.SetColumnSpan(_descriptionLabel, 2);

            _mainGrid.Add(_cancelButton, 0, 4);
            _mainGrid.Add(_confirmButton, 1, 4);

            // Create frame with custom height if specified
            _dialogFrame = CreateThemedFrame(_mainGrid);
            if (dialogHeight.HasValue)
            {
                _dialogFrame.HeightRequest = dialogHeight.Value;
                _mainGrid.HeightRequest = dialogHeight.Value;
            }

            // Set content
            Content = _dialogFrame;
        }

        /// <summary>
        /// Updates the title text
        /// </summary>
        public void SetTitle(string title)
        {
            _titleLabel.Text = title;
        }

        /// <summary>
        /// Updates the description text
        /// </summary>
        public void SetDescription(string description)
        {
            _descriptionLabel.Text = description;
        }

        /// <summary>
        /// Sets the maximum number of lines for the description
        /// </summary>
        public void SetDescriptionMaxLines(int maxLines)
        {
            _descriptionLabel.MaxLines = maxLines;
        }

        /// <summary>
        /// Shows a confirmation dialog with default Yes/No buttons
        /// </summary>
        public static async Task<bool> ShowAsync(string title, string description = "", DialogType type = DialogType.None)
        {
            return await ShowAsync(title, description, null, null, type);
        }

        /// <summary>
        /// Shows a confirmation dialog with custom button text
        /// </summary>
        public static async Task<bool> ShowAsync(
            string title,
            string description,
            string? confirmText,
            string? cancelText,
            DialogType type = DialogType.None,
            double? dialogHeight = null)
        {
            // Check if a confirm dialog is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is ConfirmDialog))
                return false;

            var dialog = new ConfirmDialog(title, description, confirmText, cancelText, type, dialogHeight);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Hides the current confirm dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is ConfirmDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
            }
        }

        /// <summary>
        /// Shows this instance of the dialog
        /// </summary>
        public async Task<bool> ShowAsync()
        {
            await MopupService.Instance.PushAsync(this);
            return await _taskCompletionSource.Task;
        }

        protected override bool HandleBackButton()
        {
            // Back button acts as cancel
            _confirmButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(false);
            return true;
        }

        protected override void OnThemeApplied(DialogTheme theme)
        {
            base.OnThemeApplied(theme);

            // Update icon if theme changes
            if (DialogType != DialogType.None)
            {
                _iconImage.Source = GetDialogIcon();
            }

            // Update title label properties from theme
            // Store text and clear to force re-render (MAUI bug workaround)
            var titleText = _titleLabel.Text;
            _titleLabel.Text = "";
            _titleLabel.MaxLines = theme.TitleMaxLines;
            _titleLabel.LineBreakMode = theme.TitleLineBreakMode;
            _titleLabel.FontSize = theme.TitleFontSize;
            _titleLabel.FontAttributes = theme.TitleFontAttributes;
            _titleLabel.TextColor = theme.TitleTextColor;
            _titleLabel.InvalidateMeasure();
            _titleLabel.Text = titleText;
            _titleLabel.InvalidateMeasure();

            // Update description label properties from theme
            _descriptionLabel.TextColor = theme.DescriptionTextColor;
            _descriptionLabel.FontSize = theme.DescriptionFontSize;
            _descriptionLabel.TextType = theme.DescriptionTextType;
        }

        private async void OnConfirmClicked(object? sender, EventArgs e)
        {
            _confirmButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(true);
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            _confirmButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(false);
        }
    }
}