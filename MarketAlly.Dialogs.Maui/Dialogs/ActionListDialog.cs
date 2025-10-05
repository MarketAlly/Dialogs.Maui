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

        public ActionListDialog(
            string title,
            List<ActionItem> items,
            string? cancelText = null,
            double? customHeight = null)
        {
            DialogType = DialogType.None;
            _items = new ObservableCollection<ActionItem>(items);

            // Create UI elements
            _titleLabel = CreateTitleLabel(title);

            _listView = new ListView
            {
                ItemsSource = _items,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = CurrentTheme.BorderColor.WithAlpha(0.3f),
                HasUnevenRows = false,
                RowHeight = 50,
                SelectionMode = ListViewSelectionMode.Single,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = items.Count > 6 ? ScrollBarVisibility.Always : ScrollBarVisibility.Never,
                BackgroundColor = Colors.Transparent,
                ItemTemplate = CreateItemTemplate(),
                VerticalOptions = LayoutOptions.FillAndExpand,
                MinimumHeightRequest = 100
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
                var horizontalStack = new HorizontalStackLayout
                {
                    Padding = new Thickness(15, 10),
                    Spacing = 10
                };

                // Icon
                var icon = new Image
                {
                    WidthRequest = 24,
                    HeightRequest = 24,
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center
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

                // Text content stack
                var textStack = new VerticalStackLayout
                {
                    Spacing = 2,
                    VerticalOptions = LayoutOptions.Center
                };

                // Title
                var titleLabel = new Label
                {
                    FontSize = 14,
                    TextColor = CurrentTheme.TitleTextColor,
                    VerticalOptions = LayoutOptions.Start
                };
                titleLabel.SetBinding(Label.TextProperty, nameof(ActionItem.Name));

                // Detail
                var detailLabel = new Label
                {
                    FontSize = 12,
                    TextColor = CurrentTheme.DescriptionTextColor.WithAlpha(0.7f),
                    VerticalOptions = LayoutOptions.Start
                };
                detailLabel.SetBinding(Label.TextProperty, nameof(ActionItem.Detail));
                detailLabel.SetBinding(Label.IsVisibleProperty, nameof(ActionItem.HasDetail));

                textStack.Children.Add(titleLabel);
                textStack.Children.Add(detailLabel);

                horizontalStack.Children.Add(icon);
                horizontalStack.Children.Add(textStack);

                return new ViewCell { View = horizontalStack };
            });
        }

        private double CalculateDialogHeight(int itemCount)
        {
            const double itemHeight = 50;       // Match ListView RowHeight
            const double titleHeight = 50;      // Title with padding
            const double buttonHeight = 50;     // Button with padding
            const double separatorHeight = 10;  // Two separators
            const double itemCountHeight = 25;  // Item count label if shown
            const double padding = 20;          // Top and bottom padding
            const int maxVisibleItems = 6;      // Show up to 6 items before scrolling

            int visibleItems = Math.Min(itemCount, maxVisibleItems);
            double listHeight = itemHeight * visibleItems;

            // Calculate total height
            double totalHeight = titleHeight + buttonHeight + separatorHeight + padding + listHeight;

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
            double? customHeight = null)
        {
            // Check if an action list is already showing
            if (MopupService.Instance.PopupStack.Any(p => p is ActionListDialog))
                return -1;

            var dialog = new ActionListDialog(title, items, cancelText, customHeight);
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

            // Recalculate height
            _containerFrame.HeightRequest = CalculateDialogHeight(items.Count);
            _listView.VerticalScrollBarVisibility = items.Count > 6
                ? ScrollBarVisibility.Default
                : ScrollBarVisibility.Never;
        }

        protected override bool HandleBackButton()
        {
            // Back button acts as cancel
            _taskCompletionSource.TrySetResult(-1);
            MopupService.Instance.PopAsync();
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
                _listView.SelectedItem = null;
                _taskCompletionSource.TrySetResult(item.Value);
                await MopupService.Instance.PopAsync();
            }
        }

        private async void OnCancelClicked(object? sender, EventArgs e)
        {
            _taskCompletionSource.TrySetResult(-1);
            await MopupService.Instance.PopAsync();
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
                // For development, try file-based loading first
                var imageName = iconSource.Replace(".svg", "").Replace(".png", "");

                try
                {
#if WINDOWS
                    // Windows needs the .png extension
                    var fileSource = ImageSource.FromFile(imageName + ".png");
#else
                    // Android/iOS work with just the base name
                    var fileSource = ImageSource.FromFile(imageName);
#endif
                    if (fileSource != null)
                        return fileSource;
                }
                catch { }

                // Try to load from embedded resources (for NuGet package)
                try
                {
                    var assembly = typeof(ActionListDialog).Assembly;
                    var resourceName = System.IO.Path.GetFileName(iconSource);

                    var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream != null)
                    {
                        var memoryStream = new System.IO.MemoryStream();
                        stream.CopyTo(memoryStream);
                        memoryStream.Position = 0;
                        stream.Dispose();
                        return ImageSource.FromStream(() => memoryStream);
                    }
                }
                catch { }
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}