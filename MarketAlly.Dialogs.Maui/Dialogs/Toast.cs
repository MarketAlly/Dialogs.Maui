using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Pages;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a lightweight, non-interactive toast notification
    /// </summary>
    public class Toast : PopupPage
    {
        private static readonly List<Toast> _activeToasts = new();
        private static readonly object _lock = new();
        private static readonly SemaphoreSlim _showSemaphore = new(1, 1);
        private static ToastStackBehavior _stackBehavior = ToastStackBehavior.Stack;
        private static int _maxVisibleToasts = 3;

        private readonly Label _messageLabel;
        private readonly Image? _iconImage;
        private readonly Border _container;
        private readonly Grid _wrapperGrid;
        private readonly ToastPosition _position;
        private readonly ToastHorizontalPosition _horizontalPosition;
        private CancellationTokenSource? _autoDismissCts;
        private bool _isDisposed;

        #region Static Configuration Properties

        /// <summary>
        /// Gets or sets the default stack behavior for toasts
        /// </summary>
        public static ToastStackBehavior DefaultStackBehavior
        {
            get => _stackBehavior;
            set => _stackBehavior = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of visible toasts when stacking (default: 3)
        /// </summary>
        public static int MaxVisibleToasts
        {
            get => _maxVisibleToasts;
            set => _maxVisibleToasts = Math.Max(1, value);
        }

        /// <summary>
        /// Gets or sets the background color for toasts (default: #1F1F1F)
        /// </summary>
        public static Color BackgroundColor { get; set; } = Color.FromRgba("#1F1F1F");

        /// <summary>
        /// Gets or sets the text color for toasts (default: White)
        /// </summary>
        public static Color TextColor { get; set; } = Colors.White;

        /// <summary>
        /// Gets or sets the font size for toast messages (default: 14)
        /// </summary>
        public static double FontSize { get; set; } = 14;

        /// <summary>
        /// Gets or sets the corner radius for toasts (default: 16)
        /// </summary>
        public static double CornerRadius { get; set; } = 16;

        /// <summary>
        /// Gets or sets the horizontal padding inside toasts (default: 14)
        /// </summary>
        public static double PaddingHorizontal { get; set; } = 14;

        /// <summary>
        /// Gets or sets the vertical padding inside toasts (default: 8)
        /// </summary>
        public static double PaddingVertical { get; set; } = 8;

        /// <summary>
        /// Gets or sets the icon size for toasts (default: 20)
        /// </summary>
        public static double IconSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the maximum width for toasts (default: 350)
        /// </summary>
        public static double MaxWidth { get; set; } = 350;

        /// <summary>
        /// Gets or sets the margin from screen edge (default: 50)
        /// </summary>
        public static double ScreenEdgeMargin { get; set; } = 50;

        /// <summary>
        /// Gets or sets the spacing between stacked toasts (default: 52)
        /// </summary>
        public static double StackSpacing { get; set; } = 52;

        /// <summary>
        /// Resets all configuration to default values
        /// </summary>
        public static void ResetConfiguration()
        {
            DefaultStackBehavior = ToastStackBehavior.Stack;
            MaxVisibleToasts = 3;
            BackgroundColor = Color.FromRgba("#1F1F1F");
            TextColor = Colors.White;
            FontSize = 14;
            CornerRadius = 16;
            PaddingHorizontal = 14;
            PaddingVertical = 8;
            IconSize = 20;
            MaxWidth = 350;
            ScreenEdgeMargin = 50;
            StackSpacing = 52;
        }

        #endregion

        public Toast(
            string message,
            DialogType iconType = DialogType.None,
            ToastPosition position = ToastPosition.Bottom,
            ToastHorizontalPosition horizontalPosition = ToastHorizontalPosition.Center)
        {
            _position = position;
            _horizontalPosition = horizontalPosition;

            // Configure popup page
            BackgroundInputTransparent = true;
            CloseWhenBackgroundIsClicked = false;
            HasSystemPadding = true;
            base.BackgroundColor = Colors.Transparent;

            // Disable Android accessibility handling to allow multiple Toast instances
            // Without this, Mopups uses Type as a dictionary key which prevents multiple toasts
            DisableAndroidAccessibilityHandling = true;

            // Note: Custom animations (SlideFromLeft, etc.) have rendering issues on some platforms.
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
                LineHeight = 1.0,
                MaxLines = 2,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            // Build horizontal layout - compact padding for tight fit
            var contentLayout = new HorizontalStackLayout
            {
                Spacing = 8,
                Padding = new Thickness(PaddingHorizontal, PaddingVertical),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            if (_iconImage != null)
            {
                contentLayout.Children.Add(_iconImage);
            }
            contentLayout.Children.Add(_messageLabel);

            // Determine horizontal alignment
            var hOptions = horizontalPosition switch
            {
                ToastHorizontalPosition.Left => LayoutOptions.Start,
                ToastHorizontalPosition.Right => LayoutOptions.End,
                _ => LayoutOptions.Center
            };

            // Determine vertical alignment
            var vOptions = position == ToastPosition.Bottom ? LayoutOptions.End : LayoutOptions.Start;

            // Create container with rounded corners
            _container = new Border
            {
                BackgroundColor = BackgroundColor,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new Microsoft.Maui.CornerRadius(CornerRadius)
                },
                HorizontalOptions = hOptions,
                VerticalOptions = vOptions,
                MaximumWidthRequest = MaxWidth,
                Shadow = new Shadow
                {
                    Brush = new SolidColorBrush(Colors.Black),
                    Offset = new Point(0, 2),
                    Radius = 8,
                    Opacity = 0.3f
                },
                Content = contentLayout
            };

            // Create wrapper grid that fills the screen for proper positioning
            _wrapperGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Padding = new Thickness(20, position == ToastPosition.Top ? ScreenEdgeMargin : 20, 20, position == ToastPosition.Bottom ? ScreenEdgeMargin : 20)
            };

            _wrapperGrid.Children.Add(_container);

            Content = _wrapperGrid;
        }

        /// <summary>
        /// Shows a toast with a message
        /// </summary>
        public static Task ShowAsync(string message)
        {
            return ShowAsync(message, DialogType.None, ToastDuration.Short, ToastPosition.Bottom, ToastHorizontalPosition.Center);
        }

        /// <summary>
        /// Shows a toast with a message and icon
        /// </summary>
        public static Task ShowAsync(string message, DialogType iconType)
        {
            return ShowAsync(message, iconType, ToastDuration.Short, ToastPosition.Bottom, ToastHorizontalPosition.Center);
        }

        /// <summary>
        /// Shows a toast with a message, icon, and duration
        /// </summary>
        public static Task ShowAsync(string message, DialogType iconType, ToastDuration duration)
        {
            return ShowAsync(message, iconType, duration, ToastPosition.Bottom, ToastHorizontalPosition.Center);
        }

        /// <summary>
        /// Shows a toast with vertical position
        /// </summary>
        public static Task ShowAsync(
            string message,
            DialogType iconType,
            ToastDuration duration,
            ToastPosition position)
        {
            return ShowAsync(message, iconType, duration, position, ToastHorizontalPosition.Center);
        }

        /// <summary>
        /// Shows a toast with full position customization
        /// </summary>
        public static async Task ShowAsync(
            string message,
            DialogType iconType,
            ToastDuration duration,
            ToastPosition position,
            ToastHorizontalPosition horizontalPosition)
        {
            var durationMs = duration == ToastDuration.Short ? 2000 : 3500;
            await ShowAsync(message, iconType, durationMs, position, horizontalPosition);
        }

        /// <summary>
        /// Shows a toast with custom duration in milliseconds
        /// </summary>
        public static Task ShowAsync(
            string message,
            DialogType iconType,
            int durationMs,
            ToastPosition position)
        {
            return ShowAsync(message, iconType, durationMs, position, ToastHorizontalPosition.Center);
        }

        /// <summary>
        /// Shows a toast with custom duration and full position customization
        /// </summary>
        public static async Task ShowAsync(
            string message,
            DialogType iconType,
            int durationMs,
            ToastPosition position,
            ToastHorizontalPosition horizontalPosition)
        {
            Toast toast;

            // Use semaphore to prevent race conditions when showing multiple toasts rapidly
            await _showSemaphore.WaitAsync();
            try
            {
                await HandleStackBehavior();

                toast = new Toast(message, iconType, position, horizontalPosition);

                lock (_lock)
                {
                    _activeToasts.Add(toast);
                    UpdateToastPositions();
                }

                // Check if already in stack (shouldn't happen, but safety check)
                if (!MopupService.Instance.PopupStack.Contains(toast))
                {
                    // Disable animation for corner positions (Mopups default slides from center)
                    bool animate = horizontalPosition == ToastHorizontalPosition.Center;
                    await MopupService.Instance.PushAsync(toast, animate: animate);
                }
            }
            finally
            {
                _showSemaphore.Release();
            }

            // Start auto-dismiss timer (outside semaphore so it doesn't block other toasts)
            toast._autoDismissCts = new CancellationTokenSource();
            try
            {
                await Task.Delay(durationMs, toast._autoDismissCts.Token);
                await toast.DismissAsync();
            }
            catch (TaskCanceledException)
            {
                // Toast was dismissed manually
            }
        }

        /// <summary>
        /// Dismisses all active toasts
        /// </summary>
        public static async Task DismissAllAsync()
        {
            List<Toast> toastsCopy;
            lock (_lock)
            {
                toastsCopy = _activeToasts.ToList();
            }

            foreach (var toast in toastsCopy)
            {
                await toast.DismissAsync();
            }
        }

        /// <summary>
        /// Dismisses this toast
        /// </summary>
        public async Task DismissAsync()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _autoDismissCts?.Cancel();

            lock (_lock)
            {
                _activeToasts.Remove(this);
                UpdateToastPositions();
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
        }

        private static async Task HandleStackBehavior()
        {
            lock (_lock)
            {
                switch (_stackBehavior)
                {
                    case ToastStackBehavior.Replace:
                        // Will dismiss all after releasing lock
                        break;

                    case ToastStackBehavior.Queue:
                        // Queue behavior: wait for existing toasts (handled by caller)
                        break;

                    case ToastStackBehavior.Stack:
                        // Remove oldest if at max
                        while (_activeToasts.Count >= _maxVisibleToasts)
                        {
                            var oldest = _activeToasts.FirstOrDefault();
                            if (oldest != null)
                            {
                                _activeToasts.RemoveAt(0);
                                // Dismiss async without blocking
                                _ = oldest.DismissInternalAsync();
                            }
                        }
                        return;
                }
            }

            if (_stackBehavior == ToastStackBehavior.Replace)
            {
                await DismissAllAsync();
            }
        }

        private async Task DismissInternalAsync()
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
        }

        private static void UpdateToastPositions()
        {
            // Update padding for stacked toasts using configurable spacing
            var bottomToasts = _activeToasts.Where(t => t._position == ToastPosition.Bottom).ToList();
            var topToasts = _activeToasts.Where(t => t._position == ToastPosition.Top).ToList();

            for (int i = 0; i < bottomToasts.Count; i++)
            {
                var offset = (bottomToasts.Count - 1 - i) * StackSpacing;
                bottomToasts[i]._wrapperGrid.Padding = new Thickness(20, 20, 20, ScreenEdgeMargin + offset);
            }

            for (int i = 0; i < topToasts.Count; i++)
            {
                var offset = (topToasts.Count - 1 - i) * StackSpacing;
                topToasts[i]._wrapperGrid.Padding = new Thickness(20, ScreenEdgeMargin + offset, 20, 20);
            }
        }

        private ImageSource? GetIconSource(DialogType dialogType, bool isDarkTheme)
        {
            var iconSource = DialogService.Instance.GetDialogIcon(dialogType, isDarkTheme);
            if (string.IsNullOrEmpty(iconSource))
                return null;

            var cacheKey = $"toast_{iconSource}_{isDarkTheme}";

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

        protected override bool OnBackButtonPressed()
        {
            // Allow back button to pass through (toast is non-blocking)
            return false;
        }
    }

    /// <summary>
    /// Defines how multiple toasts are handled
    /// </summary>
    public enum ToastStackBehavior
    {
        /// <summary>
        /// New toast replaces existing toasts
        /// </summary>
        Replace,

        /// <summary>
        /// Toasts are queued and shown one at a time
        /// </summary>
        Queue,

        /// <summary>
        /// Multiple toasts are stacked vertically (default)
        /// </summary>
        Stack
    }
}
