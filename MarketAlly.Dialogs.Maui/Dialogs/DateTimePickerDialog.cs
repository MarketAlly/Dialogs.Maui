using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a combined date and time picker dialog
    /// </summary>
    public class DateTimePickerDialog : BaseDialog
    {
        private readonly TaskCompletionSource<DateTime?> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly Label? _descriptionLabel;
        private readonly Button _okButton;
        private readonly Button _cancelButton;
        private readonly DatePicker _datePicker;
        private readonly TimePicker _timePicker;
        private readonly ImageButton? _nowButton;
        private readonly ImageButton? _clearButton;
        private readonly Grid _mainGrid;

        private DateTime? _selectedDateTime;

        /// <summary>
        /// Gets or sets the selected date
        /// </summary>
        public DateTime? SelectedDate
        {
            get => _selectedDateTime?.Date;
            set
            {
                if (value.HasValue)
                {
                    var time = _selectedDateTime?.TimeOfDay ?? TimeSpan.Zero;
                    _selectedDateTime = value.Value.Date + time;
                    _datePicker.Date = value.Value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected time
        /// </summary>
        public TimeSpan? SelectedTime
        {
            get => _selectedDateTime?.TimeOfDay;
            set
            {
                if (value.HasValue)
                {
                    var date = _selectedDateTime?.Date ?? DateTime.Today;
                    _selectedDateTime = date + value.Value;
                    _timePicker.Time = value.Value;
                }
            }
        }

        /// <summary>
        /// Gets the combined selected date and time
        /// </summary>
        public DateTime? SelectedDateTime
        {
            get => _selectedDateTime;
            set
            {
                _selectedDateTime = value;
                if (value.HasValue)
                {
                    _datePicker.Date = value.Value.Date;
                    _timePicker.Time = value.Value.TimeOfDay;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum selectable date
        /// </summary>
        public DateTime MinimumDate
        {
            get => _datePicker.MinimumDate;
            set => _datePicker.MinimumDate = value;
        }

        /// <summary>
        /// Gets or sets the maximum selectable date
        /// </summary>
        public DateTime MaximumDate
        {
            get => _datePicker.MaximumDate;
            set => _datePicker.MaximumDate = value;
        }

        /// <summary>
        /// Gets or sets the visibility of the Now button
        /// </summary>
        public bool ShowNowButton
        {
            get => _nowButton?.IsVisible ?? false;
            set
            {
                if (_nowButton != null)
                    _nowButton.IsVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the Clear button
        /// </summary>
        public bool ShowClearButton
        {
            get => _clearButton?.IsVisible ?? false;
            set
            {
                if (_clearButton != null)
                    _clearButton.IsVisible = value;
            }
        }

        /// <summary>
        /// Creates a new DateTimePickerDialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="description">Optional description text</param>
        /// <param name="initialDateTime">Initial date and time to display</param>
        /// <param name="minDate">Minimum selectable date</param>
        /// <param name="maxDate">Maximum selectable date</param>
        /// <param name="okText">OK button text</param>
        /// <param name="cancelText">Cancel button text</param>
        /// <param name="showNowButton">Whether to show the Now button</param>
        /// <param name="showClearButton">Whether to show the Clear button</param>
        /// <param name="dialogType">Dialog type for styling</param>
        public DateTimePickerDialog(
            string title,
            string? description = null,
            DateTime? initialDateTime = null,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string? okText = null,
            string? cancelText = null,
            bool showNowButton = true,
            bool showClearButton = false,
            DialogType dialogType = DialogType.None)
        {
            DialogType = dialogType;
            _selectedDateTime = initialDateTime ?? DateTime.Now;

            var theme = CurrentTheme;

            // Create title
            _titleLabel = CreateTitleLabel(title);

            // Create description if provided
            if (!string.IsNullOrEmpty(description))
            {
                _descriptionLabel = CreateDescriptionLabel(description);
                _descriptionLabel.Margin = new Thickness(0, 5);
            }

            // Create date picker
            _datePicker = new DatePicker
            {
                Date = _selectedDateTime.Value.Date,
                MinimumDate = minDate ?? DateTime.MinValue,
                MaximumDate = maxDate ?? DateTime.MaxValue,
                TextColor = theme.DescriptionTextColor,
                BackgroundColor = Colors.Transparent,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 16
            };
            _datePicker.DateSelected += OnDateSelected;

            // Create time picker
            _timePicker = new TimePicker
            {
                Time = _selectedDateTime.Value.TimeOfDay,
                TextColor = theme.DescriptionTextColor,
                BackgroundColor = Colors.Transparent,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 16
            };
            _timePicker.PropertyChanged += OnTimePickerPropertyChanged;

            // Create quick action buttons as icons
            if (showNowButton)
            {
                _nowButton = new ImageButton
                {
                    Source = new FontImageSource
                    {
                        Glyph = "\uf23c", // calendar_today_20_regular
                        FontFamily = "FluentUI",
                        Size = 20,
                        Color = theme.ButtonBackgroundColor
                    },
                    BackgroundColor = Colors.Transparent,
                    HeightRequest = 36,
                    WidthRequest = 36,
                    VerticalOptions = LayoutOptions.Center
                };
                _nowButton.Clicked += OnNowClicked;
            }

            if (showClearButton)
            {
                _clearButton = new ImageButton
                {
                    Source = new FontImageSource
                    {
                        Glyph = "\ue894", // Clear/dismiss icon
                        FontFamily = "FluentUI",
                        Size = 20,
                        Color = theme.SecondaryButtonTextColor
                    },
                    BackgroundColor = Colors.Transparent,
                    HeightRequest = 36,
                    WidthRequest = 36,
                    VerticalOptions = LayoutOptions.Center
                };
                _clearButton.Clicked += OnClearClicked;
            }

            // Create main buttons
            _okButton = CreatePrimaryButton(
                okText ?? DialogService.Localization.OkButtonText,
                OnOkClicked);

            _cancelButton = CreateSecondaryButton(
                cancelText ?? DialogService.Localization.CancelButtonText,
                OnCancelClicked);

            // Build main grid layout
            // Row 0: Title
            // Row 1: Description (optional)
            // Row 2: Separator
            // Row 3: Content (Star - takes remaining space)
            // Row 4: Buttons (fixed at bottom)
            _mainGrid = new Grid
            {
                Padding = new Thickness(20),
                RowSpacing = 10,
                ColumnSpacing = 10,
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),  // Title
                    new RowDefinition(GridLength.Auto),  // Description
                    new RowDefinition(GridLength.Auto),  // Separator
                    new RowDefinition(GridLength.Star),  // Content
                    new RowDefinition(GridLength.Auto)   // Buttons
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Star)
                }
            };

            // Row 0: Title
            _mainGrid.Add(_titleLabel, 0, 0);
            Grid.SetColumnSpan(_titleLabel, 2);

            // Row 1: Description
            int currentRow = 1;
            if (_descriptionLabel != null)
            {
                _mainGrid.Add(_descriptionLabel, 0, currentRow);
                Grid.SetColumnSpan(_descriptionLabel, 2);
            }

            // Row 2: Separator
            currentRow = 2;
            var separator = CreateSeparator();
            _mainGrid.Add(separator, 0, currentRow);
            Grid.SetColumnSpan(separator, 2);

            // Row 3: Date and Time pickers with icons
            currentRow = 3;
            var pickersContainer = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto)
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Auto)
                },
                RowSpacing = 10,
                ColumnSpacing = 5,
                VerticalOptions = LayoutOptions.Center
            };

            // Date picker row
            var dateBorder = new Border
            {
                BackgroundColor = theme.IsDarkMode
                    ? theme.BackgroundColor.AddLuminosity(0.05f)
                    : theme.BackgroundColor.AddLuminosity(-0.05f),
                Stroke = theme.BorderColor,
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Padding = new Thickness(10, 8),
                Content = _datePicker
            };
            pickersContainer.Add(dateBorder, 0, 0);

            if (_nowButton != null)
            {
                pickersContainer.Add(_nowButton, 1, 0);
            }

            if (_clearButton != null)
            {
                pickersContainer.Add(_clearButton, 2, 0);
            }

            // Time picker row
            var timeBorder = new Border
            {
                BackgroundColor = theme.IsDarkMode
                    ? theme.BackgroundColor.AddLuminosity(0.05f)
                    : theme.BackgroundColor.AddLuminosity(-0.05f),
                Stroke = theme.BorderColor,
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Padding = new Thickness(10, 8),
                Content = _timePicker
            };
            pickersContainer.Add(timeBorder, 0, 1);
            Grid.SetColumnSpan(timeBorder, 3);

            _mainGrid.Add(pickersContainer, 0, currentRow);
            Grid.SetColumnSpan(pickersContainer, 2);

            // Row 4: Buttons
            currentRow = 4;
            _mainGrid.Add(_cancelButton, 0, currentRow);
            _mainGrid.Add(_okButton, 1, currentRow);

            // Create frame with appropriate size
            var frame = CreateThemedFrame(_mainGrid);
            frame.MinimumWidthRequest = 360;
            frame.HeightRequest = -1; // Auto height based on content

            Content = frame;
        }

        private void OnDateSelected(object? sender, DateChangedEventArgs e)
        {
            var time = _selectedDateTime?.TimeOfDay ?? TimeSpan.Zero;
            _selectedDateTime = e.NewDate.Date + time;
        }

        private void OnTimePickerPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TimePicker.Time))
            {
                var date = _selectedDateTime?.Date ?? DateTime.Today;
                _selectedDateTime = date + _timePicker.Time;
            }
        }

        private void OnNowClicked(object? sender, EventArgs e)
        {
            var now = DateTime.Now;
            if (now.Date >= _datePicker.MinimumDate && now.Date <= _datePicker.MaximumDate)
            {
                _datePicker.Date = now.Date;
                _timePicker.Time = now.TimeOfDay;
                _selectedDateTime = now;
            }
        }

        private void OnClearClicked(object? sender, EventArgs e)
        {
            _selectedDateTime = null;
        }

        private async void OnOkClicked(object? sender, EventArgs e)
        {
            _okButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(_selectedDateTime);
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            _okButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(null);
        }

        protected override bool HandleBackButton()
        {
            HandleBackButtonAsync();
            return true;
        }

        private async void HandleBackButtonAsync()
        {
            _okButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(null);
        }

        protected override void OnThemeApplied(DialogTheme theme)
        {
            base.OnThemeApplied(theme);

            _datePicker.TextColor = theme.DescriptionTextColor;
            _timePicker.TextColor = theme.DescriptionTextColor;

            if (_nowButton?.Source is FontImageSource nowSource)
            {
                nowSource.Color = theme.ButtonBackgroundColor;
            }

            if (_clearButton?.Source is FontImageSource clearSource)
            {
                clearSource.Color = theme.SecondaryButtonTextColor;
            }
        }

        /// <summary>
        /// Shows a date/time picker dialog with default settings
        /// </summary>
        public static async Task<DateTime?> ShowAsync(
            string title,
            string? description = null,
            DateTime? initialDateTime = null)
        {
            return await ShowAsync(title, description, initialDateTime, null, null, null, null, true, false, DialogType.None);
        }

        /// <summary>
        /// Shows a date/time picker dialog with date constraints
        /// </summary>
        public static async Task<DateTime?> ShowAsync(
            string title,
            string? description,
            DateTime? initialDateTime,
            DateTime? minDate,
            DateTime? maxDate)
        {
            return await ShowAsync(title, description, initialDateTime, minDate, maxDate, null, null, true, false, DialogType.None);
        }

        /// <summary>
        /// Shows a date/time picker dialog with full customization
        /// </summary>
        public static async Task<DateTime?> ShowAsync(
            string title,
            string? description,
            DateTime? initialDateTime,
            DateTime? minDate,
            DateTime? maxDate,
            string? okText,
            string? cancelText,
            bool showNowButton = true,
            bool showClearButton = false,
            DialogType dialogType = DialogType.None)
        {
            // Check if a date/time picker dialog is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is DateTimePickerDialog))
                return null;

            var dialog = new DateTimePickerDialog(
                title, description, initialDateTime, minDate, maxDate,
                okText, cancelText, showNowButton, showClearButton, dialogType);

            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Shows this instance of the dialog
        /// </summary>
        public async Task<DateTime?> ShowAsync()
        {
            await MopupService.Instance.PushAsync(this);
            return await _taskCompletionSource.Task;
        }

        /// <summary>
        /// Hides the current date/time picker dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is DateTimePickerDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
            }
        }
    }
}
