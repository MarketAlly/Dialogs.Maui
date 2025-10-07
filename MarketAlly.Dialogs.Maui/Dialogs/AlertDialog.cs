using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays an alert dialog with a message and OK button
    /// </summary>
    public class AlertDialog : BaseDialog
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;
        private readonly Button _okButton;
        private readonly Image _iconImage;

        public AlertDialog(string title, string description, string? okText = null, DialogType dialogType = DialogType.None)
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
            _okButton = CreatePrimaryButton(okText ?? DialogService.Localization.OkButtonText, OnOkClicked);

            // Build layout
            var grid = new Grid
            {
                Padding = 10,
                BackgroundColor = Colors.Transparent,
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(1),
                    new RowDefinition(GridLength.Star),
                    new RowDefinition(GridLength.Auto)
                }
            };

            // Add icon if dialog type is specified
            if (dialogType != DialogType.None)
            {
                grid.Add(_iconImage, 0, 0);
            }

            grid.Add(_titleLabel, 0, 1);
            grid.Add(CreateSeparator(), 0, 2);
            grid.Add(_descriptionLabel, 0, 3);
            grid.Add(_okButton, 0, 4);

            // Set content
            Content = CreateThemedFrame(grid);
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
        /// Shows an alert dialog and waits for user acknowledgment
        /// </summary>
        public static async Task<bool> ShowAsync(string title, DialogType type = DialogType.None)
        {
            return await ShowAsync(title, "", null, type);
        }

        /// <summary>
        /// Shows an alert dialog with title and description
        /// </summary>
        public static async Task<bool> ShowAsync(string title, string description, DialogType type = DialogType.None)
        {
            return await ShowAsync(title, description, null, type);
        }

        /// <summary>
        /// Shows an alert dialog with custom OK button text
        /// </summary>
        public static async Task<bool> ShowAsync(string title, string description, string? okText, DialogType type = DialogType.None)
        {
            // Check if an alert is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is AlertDialog))
                return false;

            var dialog = new AlertDialog(title, description, okText, type);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Shows an alert dialog with specified max lines for description
        /// </summary>
        public static async Task<bool> ShowAsync(string title, string description, int maxLines, DialogType type = DialogType.None)
        {
            if (MopupService.Instance.PopupStack.Any(p => p is AlertDialog))
                return false;

            var dialog = new AlertDialog(title, description, null, type);
            dialog.SetDescriptionMaxLines(maxLines);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Hides the current alert dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var alert = MopupService.Instance.PopupStack.FirstOrDefault(p => p is AlertDialog);
            if (alert != null)
            {
                await MopupService.Instance.RemovePageAsync(alert);
            }
        }

        protected override void OnThemeApplied(DialogTheme theme)
        {
            base.OnThemeApplied(theme);

            // Update icon if theme changes
            if (DialogType != DialogType.None)
            {
                _iconImage.Source = GetDialogIcon();
            }
        }

        private async void OnOkClicked(object? sender, EventArgs e)
        {
            _okButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(true);
        }
    }
}