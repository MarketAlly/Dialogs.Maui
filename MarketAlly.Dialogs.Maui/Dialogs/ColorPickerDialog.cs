using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a color picker dialog for selecting colors
    /// </summary>
    public class ColorPickerDialog : BaseDialog
    {
        private readonly TaskCompletionSource<Color?> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly Label _descriptionLabel;
        private readonly Button _okButton;
        private readonly Button _cancelButton;
        private readonly Image _iconImage;
        private readonly BoxView _colorPreview;
        private readonly Label _hexLabel;
        private readonly Entry _hexEntry;
        private readonly Slider _redSlider;
        private readonly Slider _greenSlider;
        private readonly Slider _blueSlider;
        private readonly Slider _alphaSlider;
        private readonly Label _redValueLabel;
        private readonly Label _greenValueLabel;
        private readonly Label _blueValueLabel;
        private readonly Label _alphaValueLabel;
        private readonly Grid _presetColorsGrid;

        private Color _selectedColor = Colors.White;
        private bool _isUpdating = false;

        // Preset colors for quick selection
        private readonly Color[] _presetColors = new[]
        {
            Colors.Red, Colors.Pink, Colors.Purple, Colors.MediumPurple,
            Colors.Indigo, Colors.Blue, Colors.LightBlue, Colors.Cyan,
            Colors.Teal, Colors.Green, Colors.LightGreen, Colors.LawnGreen,
            Colors.Yellow, Colors.Gold, Colors.Orange, Colors.DarkOrange,
            Colors.Brown, Colors.Gray, Colors.SlateGray, Colors.Black
        };

        public ColorPickerDialog(
            string title,
            string? description = null,
            Color? initialColor = null,
            string? okText = null,
            string? cancelText = null,
            bool showAlpha = true,
            bool showPresets = true)
        {
            DialogType = DialogType.None;
            _selectedColor = initialColor ?? Colors.White;

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
                _descriptionLabel.Margin = new Thickness(0, 5);
            }
            else
            {
                _descriptionLabel = new Label { IsVisible = false };
            }

            var theme = CurrentTheme;

            // Color preview (smaller, for inline display)
            _colorPreview = new BoxView
            {
                HeightRequest = 35,
                WidthRequest = 60,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                CornerRadius = 5,
                Color = _selectedColor
            };

            // Hex value display and entry
            _hexLabel = new Label
            {
                Text = DialogService.Localization.HexLabel,
                TextColor = theme.DescriptionTextColor,
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            _hexEntry = new Entry
            {
                Text = ColorToHex(_selectedColor),
                TextColor = theme.DescriptionTextColor,
                PlaceholderColor = theme.DescriptionTextColor.WithAlpha(0.5f),
                FontSize = 14,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Keyboard = Keyboard.Text,
                MaxLength = 9 // #AARRGGBB
            };
            _hexEntry.TextChanged += OnHexTextChanged;

            // RGB Sliders
            _redSlider = CreateColorSlider(0, 255, _selectedColor.Red * 255);
            _greenSlider = CreateColorSlider(0, 255, _selectedColor.Green * 255);
            _blueSlider = CreateColorSlider(0, 255, _selectedColor.Blue * 255);
            _alphaSlider = CreateColorSlider(0, 255, _selectedColor.Alpha * 255);

            _redValueLabel = CreateValueLabel($"R: {(int)(_selectedColor.Red * 255)}");
            _greenValueLabel = CreateValueLabel($"G: {(int)(_selectedColor.Green * 255)}");
            _blueValueLabel = CreateValueLabel($"B: {(int)(_selectedColor.Blue * 255)}");
            _alphaValueLabel = CreateValueLabel($"A: {(int)(_selectedColor.Alpha * 255)}");

            _redSlider.ValueChanged += (s, e) => UpdateColorFromSliders();
            _greenSlider.ValueChanged += (s, e) => UpdateColorFromSliders();
            _blueSlider.ValueChanged += (s, e) => UpdateColorFromSliders();
            _alphaSlider.ValueChanged += (s, e) => UpdateColorFromSliders();

            // Preset colors grid
            _presetColorsGrid = new Grid
            {
                RowSpacing = 5,
                ColumnSpacing = 5,
                Margin = new Thickness(0, 10),
                IsVisible = showPresets
            };

            if (showPresets)
            {
                SetupPresetColors();
            }

            _okButton = CreatePrimaryButton(
                okText ?? DialogService.Localization.OkButtonText,
                OnOkClicked);

            _cancelButton = CreateSecondaryButton(
                cancelText ?? DialogService.Localization.CancelButtonText,
                OnCancelClicked);

            // Build layout
            var scrollView = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var contentStack = new VerticalStackLayout
            {
                Spacing = 10,
                Padding = new Thickness(10)
            };

            // Add title
            contentStack.Children.Add(_titleLabel);

            // Add description if provided
            if (!string.IsNullOrEmpty(description))
            {
                contentStack.Children.Add(_descriptionLabel);
            }

            // Add separator
            contentStack.Children.Add(CreateSeparator());

            // Add hex input with inline color preview
            var hexGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                },
                ColumnSpacing = 10,
                Margin = new Thickness(0, 10, 0, 10)
            };
            hexGrid.Add(_hexLabel, 0, 0);
            hexGrid.Add(_hexEntry, 1, 0);
            hexGrid.Add(_colorPreview, 2, 0);
            contentStack.Children.Add(hexGrid);

            // Add RGB sliders
            contentStack.Children.Add(CreateSliderRow(DialogService.Localization.RedLabel, _redSlider, _redValueLabel));
            contentStack.Children.Add(CreateSliderRow(DialogService.Localization.GreenLabel, _greenSlider, _greenValueLabel));
            contentStack.Children.Add(CreateSliderRow(DialogService.Localization.BlueLabel, _blueSlider, _blueValueLabel));

            if (showAlpha)
            {
                contentStack.Children.Add(CreateSliderRow(DialogService.Localization.AlphaLabel, _alphaSlider, _alphaValueLabel));
            }
            else
            {
                _alphaSlider.IsVisible = false;
            }

            // Add preset colors if enabled
            if (showPresets)
            {
                contentStack.Children.Add(new Label
                {
                    Text = DialogService.Localization.PresetColorsLabel,
                    TextColor = theme.TitleTextColor,
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 10, 0, 5)
                });
                contentStack.Children.Add(_presetColorsGrid);
            }

            scrollView.Content = contentStack;

            // Main grid with buttons at bottom
            var mainGrid = new Grid
            {
                Padding = 10,
                BackgroundColor = Colors.Transparent,
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Star),
                    new RowDefinition(GridLength.Auto)
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Star)
                }
            };

            mainGrid.Add(scrollView, 0, 0);
            Grid.SetColumnSpan(scrollView, 2);

            mainGrid.Add(_cancelButton, 0, 1);
            mainGrid.Add(_okButton, 1, 1);

            // Create frame with dynamic sizing
            var frame = CreateThemedFrame(mainGrid);
            frame.MinimumHeightRequest = 400;
            frame.MaximumHeightRequest = 600;
            frame.MinimumWidthRequest = 300;

            // Set content
            Content = frame;
        }

        private void SetupPresetColors()
        {
            int columns = 5;
            int rows = (_presetColors.Length + columns - 1) / columns;

            for (int i = 0; i < columns; i++)
            {
                _presetColorsGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            }

            for (int i = 0; i < rows; i++)
            {
                _presetColorsGrid.RowDefinitions.Add(new RowDefinition(40));
            }

            for (int i = 0; i < _presetColors.Length; i++)
            {
                var color = _presetColors[i];

                // Create a BoxView with the color
                var colorBox = new BoxView
                {
                    Color = color,
                    CornerRadius = 5,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                // Wrap it in a Border for the stroke
                var borderWrapper = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(5) },
                    Stroke = new SolidColorBrush(Colors.LightGray),
                    StrokeThickness = 1,
                    Padding = 0,
                    Content = colorBox,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) => SelectPresetColor(color);
                borderWrapper.GestureRecognizers.Add(tapGesture);

                int row = i / columns;
                int column = i % columns;
                _presetColorsGrid.Add(borderWrapper, column, row);
            }
        }

        private void SelectPresetColor(Color color)
        {
            _selectedColor = color;
            UpdateUIFromColor();
        }

        private Grid CreateSliderRow(string label, Slider slider, Label valueLabel)
        {
            var theme = CurrentTheme;
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(60),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(50)
                },
                ColumnSpacing = 5
            };

            var labelControl = new Label
            {
                Text = label,
                TextColor = theme.DescriptionTextColor,
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center
            };

            grid.Add(labelControl, 0, 0);
            grid.Add(slider, 1, 0);
            grid.Add(valueLabel, 2, 0);

            return grid;
        }

        private Slider CreateColorSlider(double min, double max, double value)
        {
            var theme = CurrentTheme;
            return new Slider
            {
                Minimum = min,
                Maximum = max,
                Value = value,
                MinimumTrackColor = theme.ButtonBackgroundColor,
                MaximumTrackColor = theme.BorderColor,
                ThumbColor = theme.ButtonBackgroundColor,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
        }

        private Label CreateValueLabel(string text)
        {
            var theme = CurrentTheme;
            return new Label
            {
                Text = text,
                TextColor = theme.DescriptionTextColor,
                FontSize = 12,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End
            };
        }

        private void UpdateColorFromSliders()
        {
            if (_isUpdating) return;

            _isUpdating = true;

            _selectedColor = new Color(
                (float)(_redSlider.Value / 255.0),
                (float)(_greenSlider.Value / 255.0),
                (float)(_blueSlider.Value / 255.0),
                (float)(_alphaSlider.Value / 255.0));

            _colorPreview.Color = _selectedColor;
            _hexEntry.Text = ColorToHex(_selectedColor);

            _redValueLabel.Text = $"R: {(int)_redSlider.Value}";
            _greenValueLabel.Text = $"G: {(int)_greenSlider.Value}";
            _blueValueLabel.Text = $"B: {(int)_blueSlider.Value}";
            _alphaValueLabel.Text = $"A: {(int)_alphaSlider.Value}";

            _isUpdating = false;
        }

        private void UpdateUIFromColor()
        {
            if (_isUpdating) return;

            _isUpdating = true;

            _colorPreview.Color = _selectedColor;
            _hexEntry.Text = ColorToHex(_selectedColor);

            _redSlider.Value = _selectedColor.Red * 255;
            _greenSlider.Value = _selectedColor.Green * 255;
            _blueSlider.Value = _selectedColor.Blue * 255;
            _alphaSlider.Value = _selectedColor.Alpha * 255;

            _redValueLabel.Text = $"R: {(int)(_selectedColor.Red * 255)}";
            _greenValueLabel.Text = $"G: {(int)(_selectedColor.Green * 255)}";
            _blueValueLabel.Text = $"B: {(int)(_selectedColor.Blue * 255)}";
            _alphaValueLabel.Text = $"A: {(int)(_selectedColor.Alpha * 255)}";

            _isUpdating = false;
        }

        private void OnHexTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;

            if (TryParseHex(e.NewTextValue, out var color))
            {
                _selectedColor = color;
                UpdateUIFromColor();
            }
        }

        private string ColorToHex(Color color)
        {
            if (color.Alpha < 1.0f)
            {
                return $"#{(int)(color.Alpha * 255):X2}{(int)(color.Red * 255):X2}{(int)(color.Green * 255):X2}{(int)(color.Blue * 255):X2}";
            }
            else
            {
                return $"#{(int)(color.Red * 255):X2}{(int)(color.Green * 255):X2}{(int)(color.Blue * 255):X2}";
            }
        }

        private bool TryParseHex(string? hex, out Color color)
        {
            color = Colors.White;
            if (string.IsNullOrEmpty(hex)) return false;

            hex = hex.Trim();
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            try
            {
                if (hex.Length == 6) // RGB
                {
                    int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                    int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                    int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                    color = new Color(r / 255f, g / 255f, b / 255f);
                    return true;
                }
                else if (hex.Length == 8) // ARGB
                {
                    int a = Convert.ToInt32(hex.Substring(0, 2), 16);
                    int r = Convert.ToInt32(hex.Substring(2, 2), 16);
                    int g = Convert.ToInt32(hex.Substring(4, 2), 16);
                    int b = Convert.ToInt32(hex.Substring(6, 2), 16);
                    color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Gets or sets the selected color
        /// </summary>
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                UpdateUIFromColor();
            }
        }

        /// <summary>
        /// Gets the selected color as a hex string
        /// </summary>
        public string GetHexColor()
        {
            return _hexEntry.Text ?? ColorToHex(_selectedColor);
        }

        /// <summary>
        /// Shows a color picker dialog with default settings
        /// </summary>
        public static async Task<Color?> ShowAsync(
            string title,
            string? description = null,
            Color? initialColor = null)
        {
            return await ShowAsync(title, description, initialColor, null, null, true, true);
        }

        /// <summary>
        /// Shows a color picker dialog with custom settings
        /// </summary>
        public static async Task<Color?> ShowAsync(
            string title,
            string? description,
            Color? initialColor,
            string? okText,
            string? cancelText,
            bool showAlpha = true,
            bool showPresets = true)
        {
            // Check if a color picker dialog is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is ColorPickerDialog))
                return null;

            var dialog = new ColorPickerDialog(title, description, initialColor, okText, cancelText, showAlpha, showPresets);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Shows this instance of the dialog
        /// </summary>
        public async Task<Color?> ShowAsync()
        {
            await MopupService.Instance.PushAsync(this);
            return await _taskCompletionSource.Task;
        }

        /// <summary>
        /// Hides the current color picker dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is ColorPickerDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
            }
        }

        protected override bool HandleBackButton()
        {
            // Back button acts as cancel
            _okButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(null);
            return true;
        }

        protected override void OnThemeApplied(DialogTheme theme)
        {
            base.OnThemeApplied(theme);

            // Update colors for theme change
            _hexLabel.TextColor = theme.DescriptionTextColor;
            _hexEntry.TextColor = theme.DescriptionTextColor;
            _hexEntry.PlaceholderColor = theme.DescriptionTextColor.WithAlpha(0.5f);

            // Update slider colors
            _redSlider.MinimumTrackColor = theme.ButtonBackgroundColor;
            _redSlider.MaximumTrackColor = theme.BorderColor;
            _redSlider.ThumbColor = theme.ButtonBackgroundColor;

            _greenSlider.MinimumTrackColor = theme.ButtonBackgroundColor;
            _greenSlider.MaximumTrackColor = theme.BorderColor;
            _greenSlider.ThumbColor = theme.ButtonBackgroundColor;

            _blueSlider.MinimumTrackColor = theme.ButtonBackgroundColor;
            _blueSlider.MaximumTrackColor = theme.BorderColor;
            _blueSlider.ThumbColor = theme.ButtonBackgroundColor;

            _alphaSlider.MinimumTrackColor = theme.ButtonBackgroundColor;
            _alphaSlider.MaximumTrackColor = theme.BorderColor;
            _alphaSlider.ThumbColor = theme.ButtonBackgroundColor;
        }

        private async void OnOkClicked(object? sender, EventArgs e)
        {
            _okButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(_selectedColor);
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            _okButton.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(null);
        }
    }
}