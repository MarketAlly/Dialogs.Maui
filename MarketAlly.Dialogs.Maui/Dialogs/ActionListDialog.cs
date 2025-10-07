using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MarketAlly.Dialogs.Maui.Dialogs
{
    /// <summary>
    /// Displays a dialog with a list of selectable actions
    /// </summary>
    public class ActionListDialog : BaseDialog
    {
        private readonly TaskCompletionSource<int> _taskCompletionSource = new();
        private readonly Label _titleLabel;
        private readonly ListView _listView;
        private readonly Button _cancelButton;
        private readonly Border _containerFrame;
        private readonly ObservableCollection<ActionItem> _items;
        private int _descriptionMaxLines = 1;
        private LineBreakMode _descriptionLineBreakMode = LineBreakMode.TailTruncation;

        /// <summary>
        /// Gets or sets the maximum number of lines for item descriptions
        /// </summary>
        public int DescriptionMaxLines
        {
            get => _descriptionMaxLines;
            set
            {
                _descriptionMaxLines = value;
                RefreshListView();
            }
        }

        /// <summary>
        /// Gets or sets the line break mode for item descriptions
        /// </summary>
        public LineBreakMode DescriptionLineBreakMode
        {
            get => _descriptionLineBreakMode;
            set
            {
                _descriptionLineBreakMode = value;
                RefreshListView();
            }
        }

        public ActionListDialog(
            string title,
            List<ActionItem> items,
            string? cancelText = null,
            double? customHeight = null,
            int descriptionMaxLines = 1,
            LineBreakMode descriptionLineBreakMode = LineBreakMode.TailTruncation)
        {
            DialogType = DialogType.None;
            _items = new ObservableCollection<ActionItem>(items);
            _descriptionMaxLines = descriptionMaxLines;
            _descriptionLineBreakMode = descriptionLineBreakMode;

            // Create UI elements
            _titleLabel = CreateTitleLabel(title);

            _listView = new ListView(ListViewCachingStrategy.RetainElement)
            {
                ItemsSource = _items,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = CurrentTheme.BorderColor.WithAlpha(0.3f),
                HasUnevenRows = true,
                SelectionMode = ListViewSelectionMode.Single,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = ShouldShowScrollBar(items.Count) ? ScrollBarVisibility.Always : ScrollBarVisibility.Never,
                BackgroundColor = Colors.Transparent,
                ItemTemplate = CreateItemTemplate(),
                VerticalOptions = LayoutOptions.Fill,
                HeightRequest = 200, // Fixed height for scrolling
                MinimumHeightRequest = 200
            };

            _listView.ItemSelected += OnItemSelected;

            _cancelButton = CreateSecondaryButton(
                cancelText ?? DialogService.Localization.CancelButtonText,
                OnCancelClicked);

            // Add item count label if there are many items
            Label? itemCountLabel = null;
            if (items.Count > 6)
            {
                itemCountLabel = new Label
                {
                    Text = DialogService.Localization.GetString("items_scroll_indicator", items.Count),
                    TextColor = CurrentTheme.DescriptionTextColor.WithAlpha(0.6f),
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 5)
                };
            }

            // Build layout with better spacing
            var grid = new Grid
            {
                Padding = new Thickness(10, 10, 10, 10),
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Colors.Transparent,
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto), // Title
                    new RowDefinition(GridLength.Auto), // Item count (if needed)
                    new RowDefinition(GridLength.Auto), // Separator
                    new RowDefinition(GridLength.Star), // List
                    new RowDefinition(GridLength.Auto), // Separator
                    new RowDefinition(GridLength.Auto)  // Cancel button
                },
                RowSpacing = 5
            };

            // Add title with proper padding
            _titleLabel.Margin = new Thickness(0, 5, 0, 5);
            grid.Add(_titleLabel, 0, 0);

            // Add item count if needed
            int currentRow = 1;
            if (itemCountLabel != null)
            {
                grid.Add(itemCountLabel, 0, currentRow);
                currentRow++;
            }

            grid.Add(CreateSeparator(), 0, currentRow);
            grid.Add(_listView, 0, currentRow + 1);
            grid.Add(CreateSeparator(), 0, currentRow + 2);

            // Add cancel button with proper margin
            _cancelButton.Margin = new Thickness(0, 5, 0, 5);
            grid.Add(_cancelButton, 0, currentRow + 3);

            // Calculate height based on items count or use custom height
            double height = customHeight ?? CalculateDialogHeight(items.Count);

            // Create frame
            _containerFrame = CreateThemedFrame(grid);
            _containerFrame.HeightRequest = height;

            // Set content
            Content = _containerFrame;
        }

        private DataTemplate CreateItemTemplate()
        {
            return new DataTemplate(() =>
            {
                // Use Grid for better control over layout and text wrapping
                var grid = new Grid
                {
                    Padding = new Thickness(15, 10),
                    ColumnSpacing = 10,
                    RowSpacing = 2,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Auto),  // Icon column (fixed)
                        new ColumnDefinition(GridLength.Star)   // Text column (fills space)
                    },
                    RowDefinitions =
                    {
                        new RowDefinition(GridLength.Auto),  // Title row
                        new RowDefinition(GridLength.Auto)   // Detail row
                    }
                };

                // Icon
                var icon = new Image
                {
                    WidthRequest = 24,
                    HeightRequest = 24,
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 2, 0, 0) // Align with text baseline
                };
                icon.SetBinding(Image.IsVisibleProperty, nameof(ActionItem.ShowImage));

                // Custom converter to process icon names properly
                var converter = new IconSourceConverter();
                var isDarkTheme = DialogService.CurrentTheme == DialogService.DarkTheme;

                // Use light icons on dark theme, dark icons on light theme
                var binding = new Binding
                {
                    Path = isDarkTheme ? nameof(ActionItem.ImageLight) : nameof(ActionItem.ImageDark),
                    Converter = converter
                };
                icon.SetBinding(Image.SourceProperty, binding);

                // Add icon spanning both rows
                grid.Add(icon, 0, 0);
                Grid.SetRowSpan(icon, 2);

                // Title
                var titleLabel = new Label
                {
                    FontSize = 14,
                    TextColor = CurrentTheme.TitleTextColor,
                    VerticalOptions = LayoutOptions.Start,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    MaxLines = 1
                };
                titleLabel.SetBinding(Label.TextProperty, nameof(ActionItem.Name));
                grid.Add(titleLabel, 1, 0);

                // Detail
                var detailLabel = new Label
                {
                    FontSize = 12,
                    TextColor = CurrentTheme.DescriptionTextColor.WithAlpha(0.7f),
                    VerticalOptions = LayoutOptions.Start,
                    MaxLines = _descriptionMaxLines,
                    LineBreakMode = _descriptionLineBreakMode
                };
                detailLabel.SetBinding(Label.TextProperty, nameof(ActionItem.Detail));
                detailLabel.SetBinding(Label.IsVisibleProperty, nameof(ActionItem.HasDetail));
                grid.Add(detailLabel, 1, 1);

                var viewCell = new ViewCell
                {
                    View = grid
                };

                return viewCell;
            });
        }

        private bool ShouldShowScrollBar(int itemCount)
        {
            // Calculate if content will exceed fixed list height
            const double fixedListHeight = 200;
            const double titleLineHeight = 20;
            const double descriptionLineHeight = 18;
            const double itemPadding = 20;

            // Estimate item height based on description lines
            double estimatedItemHeight = titleLineHeight + (_descriptionMaxLines * descriptionLineHeight) + itemPadding;
            double estimatedTotalHeight = estimatedItemHeight * itemCount;

            return estimatedTotalHeight > fixedListHeight;
        }

        private double CalculateDialogHeight(int itemCount)
        {
            // Base heights - fixed components
            const double titleHeight = 50;      // Title with padding and margins (10 top + 10 bottom)
            const double buttonHeight = 44;     // Button height from theme
            const double buttonMargin = 10;     // Button margin (5 top + 5 bottom)
            const double separatorHeight = 10;  // Two separators (1px each + margins)
            const double itemCountHeight = 25;  // Item count label if shown
            const double gridPadding = 20;      // Grid padding (10 top + 10 bottom)
            const double rowSpacing = 25;       // Grid row spacing (5 rows * 5px)

            // Fixed list height - items beyond this will scroll
            // Using a reasonable fixed height that shows approximately 4-5 single-line items
            // or 2-3 multi-line items depending on descriptionMaxLines
            const double fixedListHeight = 200;

            // Calculate total height with fixed list height
            double totalHeight = titleHeight +
                                buttonHeight +
                                buttonMargin +
                                separatorHeight +
                                gridPadding +
                                rowSpacing +
                                fixedListHeight;

            // Add item count label height if showing
            if (itemCount > 6)
            {
                totalHeight += itemCountHeight;
            }

            return totalHeight;
        }

        /// <summary>
        /// Shows an action list dialog
        /// </summary>
        public static async Task<int> ShowAsync(
            string title,
            List<ActionItem> items,
            string? cancelText = null,
            double? customHeight = null,
            int descriptionMaxLines = 1,
            LineBreakMode descriptionLineBreakMode = LineBreakMode.TailTruncation)
        {
            // Check if an action list is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is ActionListDialog))
                return -1;

            var dialog = new ActionListDialog(title, items, cancelText, customHeight, descriptionMaxLines, descriptionLineBreakMode);
            await MopupService.Instance.PushAsync(dialog);
            return await dialog._taskCompletionSource.Task;
        }

        /// <summary>
        /// Shows this instance of the dialog
        /// </summary>
        public async Task<int> ShowAsync()
        {
            await MopupService.Instance.PushAsync(this);
            return await _taskCompletionSource.Task;
        }

        /// <summary>
        /// Hides the current action list dialog
        /// </summary>
        public static async Task HideAsync()
        {
            var dialog = MopupService.Instance.PopupStack.FirstOrDefault(p => p is ActionListDialog);
            if (dialog != null)
            {
                await MopupService.Instance.RemovePageAsync(dialog);
            }
        }

        /// <summary>
        /// Updates the items in the list
        /// </summary>
        public void UpdateItems(List<ActionItem> items)
        {
            _items.Clear();
            foreach (var item in items)
            {
                _items.Add(item);
            }

            // Recalculate height and scrollbar
            _containerFrame.HeightRequest = CalculateDialogHeight(items.Count);
            _listView.VerticalScrollBarVisibility = ShouldShowScrollBar(items.Count)
                ? ScrollBarVisibility.Always
                : ScrollBarVisibility.Never;
        }

        /// <summary>
        /// Refreshes the ListView to apply changes to description styling
        /// </summary>
        private void RefreshListView()
        {
            // Force ListView to recreate all cells with new template
            _listView.ItemTemplate = CreateItemTemplate();
            _listView.HasUnevenRows = false;
            _listView.HasUnevenRows = true;

            var currentItems = _items.ToList();
            _items.Clear();
            foreach (var item in currentItems)
            {
                _items.Add(item);
            }

            // Recalculate dialog height and scrollbar based on new line settings
            _containerFrame.HeightRequest = CalculateDialogHeight(currentItems.Count);
            _listView.VerticalScrollBarVisibility = ShouldShowScrollBar(currentItems.Count)
                ? ScrollBarVisibility.Always
                : ScrollBarVisibility.Never;
        }

        protected override bool HandleBackButton()
        {
            // Back button acts as cancel
            _listView.IsEnabled = false;
            _cancelButton.IsEnabled = false;
            MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(-1);
            return true;
        }

        protected override void OnThemeApplied(DialogTheme theme)
        {
            base.OnThemeApplied(theme);

            // Refresh the list view to apply new theme
            var currentItems = _items.ToList();
            _items.Clear();
            foreach (var item in currentItems)
            {
                _items.Add(item);
            }
        }

        private async void OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ActionItem item)
            {
                // Prevent further interaction
                _listView.IsEnabled = false;
                _cancelButton.IsEnabled = false;
                _listView.SelectedItem = null;

                // Dismiss with or without animation based on theme setting
                await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
                _taskCompletionSource.TrySetResult(item.Value);
            }
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            // Prevent further interaction
            _listView.IsEnabled = false;
            _cancelButton.IsEnabled = false;

            // Dismiss with or without animation based on theme setting
            await MopupService.Instance.PopAsync(!CurrentTheme.EnableAnimation);
            _taskCompletionSource.TrySetResult(-1);
        }
    }

    /// <summary>
    /// Converter to process icon source strings to proper ImageSource
    /// </summary>
    internal class IconSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string iconSource && !string.IsNullOrEmpty(iconSource))
            {
                // Use cache for performance
                var cacheKey = $"actionlist_{iconSource}";

                return Core.ImageCache.GetOrCreate(cacheKey, () =>
                {
                    // ALWAYS use embedded resources first for NuGet consumers
                    var pngFileName = System.IO.Path.GetFileName(iconSource);
                    var resourceName = $"MarketAlly.Dialogs.Maui.Resources.Images.{pngFileName}";

                    if (Core.ImageCache.ResourceExists(resourceName))
                    {
                        var buffer = Core.ImageCache.GetResourceBytes(resourceName);
                        if (buffer != null)
                        {
                            return ImageSource.FromStream(() => new System.IO.MemoryStream(buffer));
                        }
                    }

                    // Fallback for development only
                    var imageName = iconSource.Replace(".svg", "").Replace(".png", "");
                    try
                    {
#if WINDOWS
                        return ImageSource.FromFile(imageName + ".png");
#else
                        return ImageSource.FromFile(imageName);
#endif
                    }
                    catch
                    {
                        return null;
                    }
                });
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}