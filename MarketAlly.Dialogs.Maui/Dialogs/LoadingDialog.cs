using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a loading dialog with optional cancellation
    /// </summary>
    public class LoadingDialog : BaseDialog, IDisposable
    {
        private static CancellationTokenSource? _cancellationTokenSource;
        private readonly Label _titleLabel;
        private readonly Button? _cancelButton;
        private readonly ActivityIndicator _activityIndicator;
        private bool _isDisposed;

        public LoadingDialog(string? label = null, bool canCancel = false)
        {
            DialogType = DialogType.None;

            // Create UI elements
            _activityIndicator = new ActivityIndicator
            {
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 60,
                WidthRequest = 60,
                Color = CurrentTheme.ButtonBackgroundColor
            };

            _titleLabel = CreateTitleLabel(label ?? DialogService.Localization.LoadingText);
            _titleLabel.Margin = new Thickness(0, 10);

            // Build layout - adjust rows based on whether cancel button is present
            var grid = new Grid
            {
                Padding = 10,
                BackgroundColor = Colors.Transparent
            };

            if (canCancel)
            {
                // With cancel button: 3 rows
                grid.RowDefinitions.Add(new RowDefinition(100));
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

                _cancelButton = CreatePrimaryButton(
                    DialogService.Localization.CancelButtonText,
                    OnCancelClicked);

                grid.Add(_activityIndicator, 0, 0);
                grid.Add(_titleLabel, 0, 1);
                grid.Add(_cancelButton, 0, 2);
            }
            else
            {
                // Without cancel button: 2 rows only
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

                grid.Add(_activityIndicator, 0, 0);
                grid.Add(_titleLabel, 0, 1);
            }

            // Create frame with minimum size - smaller height when no cancel button
            var frame = CreateThemedFrame(grid);
            frame.MinimumHeightRequest = canCancel ? 250 : 200;
            frame.MinimumWidthRequest = 250;

            // Set content
            Content = frame;
        }

        /// <summary>
        /// Updates the loading text
        /// </summary>
        public void UpdateText(string text)
        {
            _titleLabel.Text = text;
        }

        /// <summary>
        /// Shows a loading dialog without cancellation
        /// </summary>
        public static async Task ShowAsync(string label, Func<Task> action)
        {
            var dialog = new LoadingDialog(label, false);
            try
            {
                await MopupService.Instance.PushAsync(dialog);
                await action();
            }
            finally
            {
                dialog.Dispose();
            }
        }

        /// <summary>
        /// Shows a loading dialog with cancellation support
        /// </summary>
        public static async Task<bool> ShowCancelableAsync(string label, Func<Task> action)
        {
            var dialog = new LoadingDialog(label, true);
            bool wasCanceled = false;

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                await MopupService.Instance.PushAsync(dialog);

                var actionTask = action();
                var cancelTask = Task.Delay(Timeout.Infinite, _cancellationTokenSource.Token);

                var completedTask = await Task.WhenAny(actionTask, cancelTask);

                if (completedTask == cancelTask)
                {
                    wasCanceled = true;
                    _cancellationTokenSource.Cancel();
                    throw new OperationCanceledException(_cancellationTokenSource.Token);
                }

                await actionTask;
            }
            catch (OperationCanceledException)
            {
                wasCanceled = true;
            }
            finally
            {
                dialog.Dispose();
            }

            return wasCanceled;
        }

        /// <summary>
        /// Shows a loading dialog with cancellation token support
        /// </summary>
        public static async Task<bool> ShowCancelableAsync(string label, Func<CancellationToken, Task> action)
        {
            var dialog = new LoadingDialog(label, true);
            bool wasCanceled = false;

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                await MopupService.Instance.PushAsync(dialog);

                var actionTask = action(_cancellationTokenSource.Token);
                var cancelTask = Task.Delay(Timeout.Infinite, _cancellationTokenSource.Token);

                var completedTask = await Task.WhenAny(actionTask, cancelTask);

                if (completedTask == cancelTask)
                {
                    wasCanceled = true;
                    _cancellationTokenSource.Cancel();
                    throw new OperationCanceledException(_cancellationTokenSource.Token);
                }

                await actionTask;
            }
            catch (OperationCanceledException)
            {
                wasCanceled = true;
            }
            finally
            {
                dialog.Dispose();
            }

            return wasCanceled;
        }

        /// <summary>
        /// Shows a loading dialog without automatic dismissal
        /// </summary>
        public static async Task<LoadingDialog> ShowAsync(string? label = null, bool canCancel = false)
        {
            var dialog = new LoadingDialog(label, canCancel);
            await MopupService.Instance.PushAsync(dialog);
            return dialog;
        }

        /// <summary>
        /// Hides the current loading dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is LoadingDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
                (dialog as IDisposable)?.Dispose();
            }
        }

        protected override bool HandleBackButton()
        {
            // Prevent back button from dismissing loading dialog
            return true;
        }

        protected override void OnThemeApplied(DialogTheme theme)
        {
            base.OnThemeApplied(theme);
            _activityIndicator.Color = theme.ButtonBackgroundColor;
        }

        private void OnCancelClicked(object? sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            MopupService.Instance.PopAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    try
                    {
                        MopupService.Instance.PopAsync();
                    }
                    catch
                    {
                        // Ignore errors during disposal
                    }
                }
                _isDisposed = true;
            }
        }

        ~LoadingDialog()
        {
            Dispose(false);
        }
    }
}