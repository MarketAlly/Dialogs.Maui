using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a date picker dialog for selecting dates
    /// </summary>
    public class DatePickerDialog : BaseDialog
    {
        private readonly TaskCompletionSource<DateTime?> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly Label? _descriptionLabel;
        private readonly Button _okButton;
        private readonly Button _cancelButton;
        private readonly DatePicker _datePicker;
        private readonly ImageButton? _todayButton;
        private readonly ImageButton? _clearButton;
        private readonly Grid _mainGrid;

        private DateTime? _selectedDate;

        /// <summary>
        /// Gets or sets the selected date
        /// </summary>
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                if (value.HasValue)
                {
                    _datePicker.Date = value.Value;
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
        /// Gets or sets the visibility of the Today button
        /// </summary>
        public bool ShowTodayButton
        {
            get => _todayButton?.IsVisible ?? false;
            set
            {
                if (_todayButton != null)
                    _todayButton.IsVisible = value;
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
        /// Creates a new DatePickerDialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="description">Optional description text</param>
        /// <param name="initialDate">Initial date to display</param>
        /// <param name="minDate">Minimum selectable date</param>
        /// <param name="maxDate">Maximum selectable date</param>
        /// <param name="okText">OK button text</param>
        /// <param name="cancelText">Cancel button text</param>
        /// <param name="showTodayButton">Whether to show the Today button</param>
        /// <param name="showClearButton">Whether to show the Clear button</param>
        /// <param name="dialogType">Dialog type for styling</param>
        public DatePickerDialog(
            string title,
            string? description = null,
            DateTime? initialDate = null,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string? okText = null,
            string? cancelText = null,
            bool showTodayButton = true,
            bool showClearButton = false,
            DialogType dialogType = DialogType.None)
        {
            DialogType = dialogType;
            _selectedDate = initialDate ?? DateTime.Today;

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
                Date = _selectedDate.Value,
                MinimumDate = minDate ?? DateTime.MinValue,
                MaximumDate = maxDate ?? DateTime.MaxValue,
                TextColor = theme.DescriptionTextColor,
                BackgroundColor = Colors.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 18
            };
            _datePicker.DateSelected += OnDateSelected;

            // Create quick action buttons as icons
            if (showTodayButton)
            {
                _todayButton = new ImageButton
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
                _todayButton.Clicked += OnTodayClicked;
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

            // Row 3: Date picker with icons
            currentRow = 3;
            var pickerRow = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Auto)
                },
                ColumnSpacing = 5,
                VerticalOptions = LayoutOptions.Center
            };

            var datePickerBorder = new Border
            {
                BackgroundColor = theme.IsDarkMode
                    ? theme.BackgroundColor.AddLuminosity(0.05f)
                    : theme.BackgroundColor.AddLuminosity(-0.05f),
                Stroke = theme.BorderColor,
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Padding = new Thickness(10, 8),
                Content = _datePicker
            };
            pickerRow.Add(datePickerBorder, 0, 0);

            if (_todayButton != null)
            {
                pickerRow.Add(_todayButton, 1, 0);
            }

            if (_clearButton != null)
            {
                pickerRow.Add(_clearButton, 2, 0);
            }

            _mainGrid.Add(pickerRow, 0, currentRow);
            Grid.SetColumnSpan(pickerRow, 2);

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
            _selectedDate = e.NewDate;
        }

        private void OnTodayClicked(object? sender, EventArgs e)
        {
            var today = DateTime.Today;
            if (today >= _datePicker.MinimumDate && today <= _datePicker.MaximumDate)
            {
                _datePicker.Date = today;
                _selectedDate = today;
            }
        }

        private void OnClearClicked(object? sender, EventArgs e)
        {
            _selectedDate = null;
        }

        private async void OnOkClicked(object? sender, EventArgs e)
        {
            _okButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(_selectedDate);
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

            if (_todayButton?.Source is FontImageSource todaySource)
            {
                todaySource.Color = theme.ButtonBackgroundColor;
            }

            if (_clearButton?.Source is FontImageSource clearSource)
            {
                clearSource.Color = theme.SecondaryButtonTextColor;
            }
        }

        /// <summary>
        /// Shows a date picker dialog with default settings
        /// </summary>
        public static async Task<DateTime?> ShowAsync(
            string title,
            string? description = null,
            DateTime? initialDate = null)
        {
            return await ShowAsync(title, description, initialDate, null, null, null, null, true, false, DialogType.None);
        }

        /// <summary>
        /// Shows a date picker dialog with date constraints
        /// </summary>
        public static async Task<DateTime?> ShowAsync(
            string title,
            string? description,
            DateTime? initialDate,
            DateTime? minDate,
            DateTime? maxDate)
        {
            return await ShowAsync(title, description, initialDate, minDate, maxDate, null, null, true, false, DialogType.None);
        }

        /// <summary>
        /// Shows a date picker dialog with full customization
        /// </summary>
        public static async Task<DateTime?> ShowAsync(
            string title,
            string? description,
            DateTime? initialDate,
            DateTime? minDate,
            DateTime? maxDate,
            string? okText,
            string? cancelText,
            bool showTodayButton = true,
            bool showClearButton = false,
            DialogType dialogType = DialogType.None)
        {
            // Check if a date picker dialog is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is DatePickerDialog))
                return null;

            var dialog = new DatePickerDialog(
                title, description, initialDate, minDate, maxDate,
                okText, cancelText, showTodayButton, showClearButton, dialogType);

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
        /// Hides the current date picker dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is DatePickerDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
            }
        }
    }
}
