using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Pages;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays an actionable snackbar notification with optional action button
    /// </summary>
    public class Snackbar : PopupPage
    {
        private static readonly List<Snackbar> _activeSnackbars = new();
        private static readonly object _lock = new();
        private static readonly SemaphoreSlim _showSemaphore = new(1, 1);
        private static SnackbarStackBehavior _stackBehavior = SnackbarStackBehavior.Stack;
        private static int _maxVisibleSnackbars = 3;

        private readonly TaskCompletionSource<SnackbarResult> _taskCompletionSource = new();
        private readonly Label _messageLabel;
        private readonly Button? _actionButton;
        private readonly Image? _iconImage;
        private readonly Border _container;
        private readonly ToastPosition _position;
        private readonly Action? _actionCallback;
        private CancellationTokenSource? _autoDismissCts;
        private bool _isDisposed;

        #region Static Configuration Properties

        /// <summary>
        /// Gets or sets the default stack behavior for snackbars
        /// </summary>
        public static SnackbarStackBehavior DefaultStackBehavior
        {
            get => _stackBehavior;
            set => _stackBehavior = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of visible snackbars when stacking (default: 3)
        /// </summary>
        public static int MaxVisibleSnackbars
        {
            get => _maxVisibleSnackbars;
            set => _maxVisibleSnackbars = Math.Max(1, value);
        }

        /// <summary>
        /// Gets or sets the background color for snackbars (default: #1F1F1F)
        /// </summary>
        public static Color BackgroundColor { get; set; } = Color.FromRgba("#1F1F1F");

        /// <summary>
        /// Gets or sets the text color for snackbar messages (default: White)
        /// </summary>
        public static Color TextColor { get; set; } = Colors.White;

        /// <summary>
        /// Gets or sets the action button text color (default: #BB86FC - Material purple)
        /// </summary>
        public static Color ActionTextColor { get; set; } = Color.FromRgba("#BB86FC");

        /// <summary>
        /// Gets or sets the font size for snackbar messages (default: 14)
        /// </summary>
        public static double FontSize { get; set; } = 14;

        /// <summary>
        /// Gets or sets the corner radius for snackbars (default: 6)
        /// </summary>
        public static double CornerRadius { get; set; } = 6;

        /// <summary>
        /// Gets or sets the horizontal padding inside snackbars (default: 14)
        /// </summary>
        public static double PaddingHorizontal { get; set; } = 14;

        /// <summary>
        /// Gets or sets the vertical padding inside snackbars (default: 10)
        /// </summary>
        public static double PaddingVertical { get; set; } = 10;

        /// <summary>
        /// Gets or sets the icon size for snackbars (default: 20)
        /// </summary>
        public static double IconSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the margin from screen edge (default: 80)
        /// </summary>
        public static double ScreenEdgeMargin { get; set; } = 80;

        /// <summary>
        /// Gets or sets the horizontal margin from screen sides (default: 16)
        /// </summary>
        public static double SideMargin { get; set; } = 16;

        /// <summary>
        /// Gets or sets the spacing between stacked snackbars (default: 55)
        /// </summary>
        public static double StackSpacing { get; set; } = 55;

        /// <summary>
        /// Resets all configuration to default values
        /// </summary>
        public static void ResetConfiguration()
        {
            DefaultStackBehavior = SnackbarStackBehavior.Stack;
            MaxVisibleSnackbars = 3;
            BackgroundColor = Color.FromRgba("#1F1F1F");
            TextColor = Colors.White;
            ActionTextColor = Color.FromRgba("#BB86FC");
            FontSize = 14;
            CornerRadius = 6;
            PaddingHorizontal = 14;
            PaddingVertical = 10;
            IconSize = 20;
            ScreenEdgeMargin = 80;
            SideMargin = 16;
            StackSpacing = 55;
        }

        #endregion

        public Snackbar(
            string message,
            string? actionText = null,
            Action? actionCallback = null,
            DialogType iconType = DialogType.None,
            ToastPosition position = ToastPosition.Bottom)
        {
            _position = position;
            _actionCallback = actionCallback;

            // Configure popup page
            BackgroundInputTransparent = true;
            CloseWhenBackgroundIsClicked = true;
            HasSystemPadding = true;
            base.BackgroundColor = Colors.Transparent;

            // Disable Android accessibility handling to allow multiple Snackbar instances
            // Without this, Mopups uses Type as a dictionary key which prevents multiple snackbars
            DisableAndroidAccessibilityHandling = true;

            // Note: Custom animations have rendering issues on some platforms.
            // Using Mopups default fade animation for reliability.

            var theme = DialogService.Instance.CurrentTheme;

            // Create icon if specified
            if (iconType != DialogType.None)
            {
                _iconImage = new Image
                {
                    HeightRequest = IconSize,
                    WidthRequest = IconSize,
                    VerticalOptions = LayoutOptions.Center,
                    Source = GetIconSource(iconType, theme.IsDarkMode)
                };
            }

            // Create message label
            _messageLabel = new Label
            {
                Text = message,
                TextColor = TextColor,
                FontSize = FontSize,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                MaxLines = 2,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            // Create action button if specified
            if (!string.IsNullOrEmpty(actionText))
            {
                _actionButton = new Button
                {
                    Text = actionText.ToUpperInvariant(),
                    TextColor = ActionTextColor,
                    BackgroundColor = Colors.Transparent,
                    FontSize = FontSize,
                    FontAttributes = FontAttributes.Bold,
                    Padding = new Thickness(8, 0),
                    VerticalOptions = LayoutOptions.Center,
                    MinimumWidthRequest = 60
                };
                _actionButton.Clicked += OnActionClicked;
            }

            // Build horizontal layout - compact padding
            var contentLayout = new Grid
            {
                Padding = new Thickness(PaddingHorizontal, PaddingVertical),
                ColumnSpacing = 8,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };

            int columnIndex = 0;

            // Add icon column if present
            if (_iconImage != null)
            {
                contentLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                contentLayout.Add(_iconImage, columnIndex++, 0);
            }

            // Add message column (fills available space)
            contentLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            contentLayout.Add(_messageLabel, columnIndex++, 0);

            // Add action button column if present
            if (_actionButton != null)
            {
                contentLayout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                contentLayout.Add(_actionButton, columnIndex, 0);
            }

            // Create container with rounded corners
            _container = new Border
            {
                BackgroundColor = BackgroundColor,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new Microsoft.Maui.CornerRadius(CornerRadius)
                },
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = position == ToastPosition.Bottom ? LayoutOptions.End : LayoutOptions.Start,
                Margin = new Thickness(SideMargin, position == ToastPosition.Top ? ScreenEdgeMargin : SideMargin, SideMargin, position == ToastPosition.Bottom ? ScreenEdgeMargin : SideMargin),
                Shadow = new Shadow
                {
                    Brush = new SolidColorBrush(Colors.Black),
                    Offset = new Point(0, 2),
                    Radius = 6,
                    Opacity = 0.4f
                },
                Content = contentLayout
            };

            // Add swipe gesture for dismissal
            var swipeGesture = new SwipeGestureRecognizer
            {
                Direction = position == ToastPosition.Bottom ? SwipeDirection.Down : SwipeDirection.Up
            };
            swipeGesture.Swiped += OnSwiped;
            _container.GestureRecognizers.Add(swipeGesture);

            Content = _container;
        }

        #region Static Show Methods

        /// <summary>
        /// Shows a snackbar with just a message (no action)
        /// </summary>
        public static Task<SnackbarResult> ShowAsync(string message)
        {
            return ShowAsync(message, null, null, DialogType.None, SnackbarDuration.Short, ToastPosition.Bottom);
        }

        /// <summary>
        /// Shows a snackbar with message and action button
        /// </summary>
        public static Task<SnackbarResult> ShowAsync(string message, string actionText)
        {
            return ShowAsync(message, actionText, null, DialogType.None, SnackbarDuration.Short, ToastPosition.Bottom);
        }

        /// <summary>
        /// Shows a snackbar with message, action, and callback
        /// </summary>
        public static Task<SnackbarResult> ShowAsync(string message, string actionText, Action actionCallback)
        {
            return ShowAsync(message, actionText, actionCallback, DialogType.None, SnackbarDuration.Short, ToastPosition.Bottom);
        }

        /// <summary>
        /// Shows a snackbar with message, action, callback, and duration
        /// </summary>
        public static Task<SnackbarResult> ShowAsync(
            string message,
            string? actionText,
            Action? actionCallback,
            SnackbarDuration duration)
        {
            return ShowAsync(message, actionText, actionCallback, DialogType.None, duration, ToastPosition.Bottom);
        }

        /// <summary>
        /// Shows a snackbar with full customization
        /// </summary>
        public static async Task<SnackbarResult> ShowAsync(
            string message,
            string? actionText,
            Action? actionCallback,
            DialogType iconType,
            SnackbarDuration duration,
            ToastPosition position)
        {
            int? durationMs = duration switch
            {
                SnackbarDuration.Short => 4000,
                SnackbarDuration.Long => 7000,
                SnackbarDuration.Indefinite => null,
                _ => 4000
            };

            return await ShowAsync(message, actionText, actionCallback, iconType, durationMs, position);
        }

        /// <summary>
        /// Shows a snackbar with custom duration in milliseconds (null for indefinite)
        /// </summary>
        public static async Task<SnackbarResult> ShowAsync(
            string message,
            string? actionText,
            Action? actionCallback,
            DialogType iconType,
            int? durationMs,
            ToastPosition position)
        {
            Snackbar snackbar;

            // Use semaphore to prevent race conditions when showing multiple snackbars rapidly
            await _showSemaphore.WaitAsync();
            try
            {
                await HandleStackBehavior();

                snackbar = new Snackbar(message, actionText, actionCallback, iconType, position);

                lock (_lock)
                {
                    _activeSnackbars.Add(snackbar);
                    UpdateSnackbarPositions();
                }

                // Check if already in stack (shouldn't happen, but safety check)
                if (!MopupService.Instance.PopupStack.Contains(snackbar))
                {
                    await MopupService.Instance.PushAsync(snackbar, animate: true);
                }
            }
            finally
            {
                _showSemaphore.Release();
            }

            // Start auto-dismiss timer if duration is specified (outside semaphore)
            if (durationMs.HasValue)
            {
                snackbar._autoDismissCts = new CancellationTokenSource();
                _ = snackbar.StartAutoDismissTimer(durationMs.Value);
            }

            return await snackbar._taskCompletionSource.Task;
        }

        #endregion

        #region Dismiss Methods

        /// <summary>
        /// Dismisses all active snackbars
        /// </summary>
        public static async Task DismissAllAsync()
        {
            List<Snackbar> snackbarsCopy;
            lock (_lock)
            {
                snackbarsCopy = _activeSnackbars.ToList();
            }

            foreach (var snackbar in snackbarsCopy)
            {
                await snackbar.DismissAsync(SnackbarResult.Dismissed);
            }
        }

        /// <summary>
        /// Dismisses this snackbar with a specific result
        /// </summary>
        public async Task DismissAsync(SnackbarResult result)
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _autoDismissCts?.Cancel();

            lock (_lock)
            {
                _activeSnackbars.Remove(this);
                UpdateSnackbarPositions();
            }

            try
            {
                if (MopupService.Instance.PopupStack.Contains(this))
                {
                    await MopupService.Instance.RemovePageAsync(this, animate: true);
                }
            }
            catch
            {
                // Ignore errors during dismissal
            }

            _taskCompletionSource.TrySetResult(result);
        }

        #endregion

        #region Private Methods

        private async Task StartAutoDismissTimer(int durationMs)
        {
            try
            {
                await Task.Delay(durationMs, _autoDismissCts!.Token);
                await DismissAsync(SnackbarResult.TimedOut);
            }
            catch (TaskCanceledException)
            {
                // Timer was cancelled (user interacted)
            }
        }

        private static async Task HandleStackBehavior()
        {
            lock (_lock)
            {
                switch (_stackBehavior)
                {
                    case SnackbarStackBehavior.Replace:
                        // Will dismiss all after releasing lock
                        break;

                    case SnackbarStackBehavior.Queue:
                        // Queue behavior: wait for existing snackbars (handled by caller)
                        break;

                    case SnackbarStackBehavior.Stack:
                        // Remove oldest if at max
                        while (_activeSnackbars.Count >= _maxVisibleSnackbars)
                        {
                            var oldest = _activeSnackbars.FirstOrDefault();
                            if (oldest != null)
                            {
                                _activeSnackbars.RemoveAt(0);
                                // Dismiss async without blocking
                                _ = oldest.DismissInternalAsync(SnackbarResult.Dismissed);
                            }
                        }
                        return;
                }
            }

            if (_stackBehavior == SnackbarStackBehavior.Replace)
            {
                await DismissAllAsync();
            }
        }

        private async Task DismissInternalAsync(SnackbarResult result)
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _autoDismissCts?.Cancel();

            try
            {
                if (MopupService.Instance.PopupStack.Contains(this))
                {
                    await MopupService.Instance.RemovePageAsync(this, animate: true);
                }
            }
            catch
            {
                // Ignore errors during dismissal
            }

            _taskCompletionSource.TrySetResult(result);
        }

        private static void UpdateSnackbarPositions()
        {
            // Update margins for stacked snackbars using configurable spacing
            var bottomSnackbars = _activeSnackbars.Where(s => s._position == ToastPosition.Bottom).ToList();
            var topSnackbars = _activeSnackbars.Where(s => s._position == ToastPosition.Top).ToList();

            for (int i = 0; i < bottomSnackbars.Count; i++)
            {
                var offset = (bottomSnackbars.Count - 1 - i) * StackSpacing;
                bottomSnackbars[i]._container.Margin = new Thickness(SideMargin, SideMargin, SideMargin, ScreenEdgeMargin + offset);
            }

            for (int i = 0; i < topSnackbars.Count; i++)
            {
                var offset = (topSnackbars.Count - 1 - i) * StackSpacing;
                topSnackbars[i]._container.Margin = new Thickness(SideMargin, ScreenEdgeMargin + offset, SideMargin, SideMargin);
            }
        }

        private ImageSource? GetIconSource(DialogType dialogType, bool isDarkTheme)
        {
            var iconSource = DialogService.Instance.GetDialogIcon(dialogType, isDarkTheme);
            if (string.IsNullOrEmpty(iconSource))
                return null;

            var cacheKey = $"snackbar_{iconSource}_{isDarkTheme}";

            return ImageCache.GetOrCreate(cacheKey, () =>
            {
                var pngFileName = System.IO.Path.GetFileName(iconSource);
                var resourceName = $"MarketAlly.Dialogs.Maui.Resources.Images.{pngFileName}";

                if (ImageCache.ResourceExists(resourceName))
                {
                    var buffer = ImageCache.GetResourceBytes(resourceName);
                    if (buffer != null)
                    {
                        return ImageSource.FromStream(() => new System.IO.MemoryStream(buffer));
                    }
                }

                return null;
            });
        }

        #endregion

        #region Event Handlers

        private async void OnActionClicked(object? sender, EventArgs e)
        {
            if (_actionButton != null)
                _actionButton.IsEnabled = false;

            _actionCallback?.Invoke();
            await DismissAsync(SnackbarResult.ActionClicked);
        }

        private async void OnSwiped(object? sender, SwipedEventArgs e)
        {
            await DismissAsync(SnackbarResult.Dismissed);
        }

        protected override bool OnBackgroundClicked()
        {
            // Dismiss on background tap
            _ = DismissAsync(SnackbarResult.Dismissed);
            return true;
        }

        protected override bool OnBackButtonPressed()
        {
            // Allow back button to pass through (snackbar is non-blocking)
            // but also dismiss the snackbar
            _ = DismissAsync(SnackbarResult.Dismissed);
            return false;
        }

        #endregion
    }

    /// <summary>
    /// Defines how multiple snackbars are handled
    /// </summary>
    public enum SnackbarStackBehavior
    {
        /// <summary>
        /// New snackbar replaces existing snackbars
        /// </summary>
        Replace,

        /// <summary>
        /// Snackbars are queued and shown one at a time
        /// </summary>
        Queue,

        /// <summary>
        /// Multiple snackbars are stacked vertically (default)
        /// </summary>
        Stack
    }
}
