# MarketAlly.Dialogs.Maui

[![NuGet Version](https://img.shields.io/nuget/v/MarketAlly.Dialogs.Maui.svg?style=flat)](https://www.nuget.org/packages/MarketAlly.Dialogs.Maui/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/download)
[![Platform](https://img.shields.io/badge/Platform-iOS%20%7C%20Android%20%7C%20Windows%20%7C%20macOS-lightgray)](https://dotnet.microsoft.com/apps/maui)

A comprehensive, production-ready dialog library for .NET MAUI applications with built-in theming, localization, and extensive customization options.

## 🎯 Features

- **🎨 Rich Dialog Collection**: Alert, Confirm, Prompt, Editor, Loading, Action List, and Color Picker dialogs
- **🌓 Adaptive Theming**: Automatic dark/light theme detection with full customization
- **🌍 Internationalization**: Built-in support for English, Spanish, French, and German with extensible localization framework
- **🎭 Custom Icons**: Support for custom icons with platform-specific optimizations
- **📱 Cross-Platform**: Consistent experience across iOS, Android, Windows, and macOS
- **⚡ Performance Optimized**: Efficient resource handling and platform-specific rendering
- **♿ Accessibility Ready**: Full support for screen readers and accessibility features
- **🔒 Type-Safe**: Strongly typed APIs with comprehensive IntelliSense support

## 📦 Installation

Install via NuGet Package Manager:

```bash
dotnet add package MarketAlly.Dialogs.Maui
```

Or via Package Manager Console:

```powershell
Install-Package MarketAlly.Dialogs.Maui
```

## 🚀 Quick Start

### 1. Configure in MauiProgram.cs

```csharp
using MarketAlly.Dialogs.Maui;
using Mopups.Hosting;

public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .ConfigureMopups() // Required for popup functionality
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

    return builder.Build();
}
```

### 2. Initialize (Optional)

```csharp
// In App.xaml.cs
protected override void OnStart()
{
    // Optional: Initialize with default settings
    DialogService.Instance.Initialize();

    // Optional: Set overlay preferences
    DialogService.Instance.SetOverlayEnabled(true);
    DialogService.Instance.SetOverlayColor(Color.FromRgba("#80000000"));
}
```

### 3. Show Your First Dialog

```csharp
using MarketAlly.Dialogs.Maui.Dialogs;

// Simple alert
await AlertDialog.ShowAsync("Welcome!", "Thanks for using our dialogs", DialogType.Success);
```

## 📖 Dialog Types

### Alert Dialog
Display informational messages with customizable icons and styling.

```csharp
// Simple alert
await AlertDialog.ShowAsync("Operation Complete", DialogType.Success);

// With description
await AlertDialog.ShowAsync(
    "Network Error",
    "Unable to connect to server. Please check your connection.",
    DialogType.Error
);

// Custom button and styling
var dialog = new AlertDialog(
    "Important Notice",
    "Please read the terms carefully",
    "I Understand",
    DialogType.Warning);
dialog.DescriptionPadding = new Thickness(20);
await dialog.ShowAsync();
```

### Confirmation Dialog
Get user confirmation with customizable buttons.

```csharp
// Simple yes/no
bool confirmed = await ConfirmDialog.ShowAsync(
    "Delete Item",
    "This action cannot be undone. Continue?"
);

// Custom buttons
var dialog = new ConfirmDialog(
    "Save Changes",
    "You have unsaved changes. What would you like to do?",
    "Save & Exit",
    "Discard",
    DialogType.Warning);

bool result = await dialog.ShowAsync();
```

### Prompt Dialog (Single Line)
Collect single-line text input with validation support.

```csharp
// Basic text input
string? name = await PromptDialog.ShowAsync(
    "Enter Name",
    "Your full name"
);

// Password input with visibility toggle
var dialog = new PromptDialog(
    "Enter Password",
    "Password",
    "Login",
    "Cancel",
    DialogType.None,
    Keyboard.Text,
    isPassword: true);

string? password = await dialog.ShowAsync();

// Email with specific keyboard
string? email = await PromptDialog.ShowAsync(
    "Email Address",
    "user@example.com",
    Keyboard.Email,
    DialogType.Info
);
```

### Editor Dialog (Multi-line)
Collect multi-line text with configurable constraints.

```csharp
// Basic multi-line input
string? notes = await EditorDialog.ShowAsync(
    "Add Notes",
    "Enter your notes here",
    "Type your notes...",
    DialogType.None
);

// Advanced configuration
var dialog = new EditorDialog(
    "Feedback",
    "Help us improve",
    "Your feedback...",
    "Submit",
    "Cancel",
    DialogType.Help,
    Keyboard.Text,
    minLines: 3,
    maxLines: 10);

dialog.IsSpellCheckEnabled = true;
dialog.IsTextPredictionEnabled = true;

string? feedback = await dialog.ShowAsync();
```

### Loading Dialog
Show progress indicators with optional cancellation.

```csharp
// Simple loading
await LoadingDialog.ShowAsync("Processing...", async () =>
{
    await ProcessDataAsync();
});

// With cancellation
bool wasCanceled = await LoadingDialog.ShowCancelableAsync(
    "Downloading... Click Cancel to stop",
    async () =>
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(100);
            // Check cancellation token
        }
    }
);

// Manual control
var loading = new LoadingDialog("Uploading...");
await MopupService.Instance.PushAsync(loading);
// ... do work
await MopupService.Instance.RemovePageAsync(loading);
```

### Action List Dialog
Present a list of actions with optional icons and descriptions. Supports multi-line descriptions with customizable truncation and wrapping behavior.

```csharp
// Basic action list
var actions = new List<ActionItem>
{
    new ActionItem("Share", "Share with others", 0),
    new ActionItem("Edit", "Modify the item", 1),
    new ActionItem("Delete", "Remove permanently", 2),
    new ActionItem("Archive", "Move to archive", 3)
};

var dialog = new ActionListDialog(
    "Choose Action",
    actions,
    "Cancel"
);

int result = await dialog.ShowAsync();

if (result >= 0)
{
    // Action selected (0-3)
    var selectedAction = actions[result];
    await HandleAction(selectedAction);
}

// With icons
var actionsWithIcons = new List<ActionItem>
{
    new ActionItem("Share", "Share with others", 0,
        "share_icon_dark.png", "share_icon_light.png"),
    new ActionItem("Edit", "Modify the item", 1,
        "edit_icon_dark.png", "edit_icon_light.png")
};

// Multi-line descriptions (NEW in v1.1.0)
var premiumActions = new List<ActionItem>
{
    new ActionItem("Cloud Sync",
        "Automatically sync your files across all devices in real-time. Changes are instantly reflected everywhere you work.",
        0, "sync_icon_dark.png", "sync_icon_light.png"),
    new ActionItem("Team Collaboration",
        "Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.",
        1, "team_icon_dark.png", "team_icon_light.png")
};

var multiLineDialog = new ActionListDialog(
    "Premium Features",
    premiumActions,
    "Cancel",
    customHeight: null,
    descriptionMaxLines: 2,  // Wrap to 2 lines with ellipsis
    descriptionLineBreakMode: LineBreakMode.TailTruncation
);

int selected = await multiLineDialog.ShowAsync();
```

### Color Picker Dialog
Advanced color selection with RGB sliders, hex input, and preset colors.

```csharp
// Basic color picker
var dialog = new ColorPickerDialog(
    "Choose Theme Color",
    "Select your preferred color",
    Colors.Blue,
    "Select",
    "Cancel"
);

Color? selectedColor = await dialog.ShowAsync();
if (selectedColor != null)
{
    string hexColor = dialog.GetHexColor();
    // Apply the color
}

// With alpha channel
var dialogWithAlpha = new ColorPickerDialog(
    "Background Color",
    "Choose color with transparency",
    Colors.Red.WithAlpha(0.5f),
    "Apply",
    "Cancel",
    showAlpha: true,
    showPresets: true
);

// Without preset colors
var customDialog = new ColorPickerDialog(
    "Custom Color",
    null,
    Colors.Green,
    "OK",
    "Cancel",
    showAlpha: false,
    showPresets: false
);
```

## 🎨 Theming

### Custom Theme Creation

```csharp
var customLightTheme = new DialogTheme
{
    // Colors
    BackgroundColor = Color.FromRgba("#FFFFFF"),
    OverlayColor = Color.FromRgba("#80000000"),
    BorderColor = Color.FromRgba("#E0E0E0"),
    ShowOverlay = true,

    // Text Colors
    TitleTextColor = Color.FromRgba("#212121"),
    DescriptionTextColor = Color.FromRgba("#757575"),

    // Button Colors
    ButtonBackgroundColor = Color.FromRgba("#2196F3"),
    ButtonTextColor = Color.FromRgba("#FFFFFF"),
    SecondaryButtonBackgroundColor = Color.FromRgba("#F5F5F5"),
    SecondaryButtonTextColor = Color.FromRgba("#212121"),

    // Typography
    TitleFontSize = 18,
    TitleFontAttributes = FontAttributes.Bold,
    TitleMaxLines = 2,
    TitleLineBreakMode = LineBreakMode.TailTruncation,
    DescriptionFontSize = 14,
    DescriptionTextType = TextType.Text,  // or TextType.Html for HTML support
    ButtonFontSize = 14,

    // Layout
    DialogWidth = 300,
    DialogHeight = 250,
    DialogCornerRadius = 12,
    DialogPadding = 20,
    ButtonHeight = 44,

    // Effects
    HasShadow = true,
    AnimationDuration = 250,
    EnableAnimation = true
};

DialogService.Instance.Initialize(customLightTheme, customDarkTheme);
```

### Dynamic Theme Switching

```csharp
// Force dark theme
DialogService.Instance.CurrentThemeOverride = DialogService.Instance.DarkTheme;

// Force light theme
DialogService.Instance.CurrentThemeOverride = DialogService.Instance.LightTheme;

// Return to system theme
DialogService.Instance.CurrentThemeOverride = null;

// Disable background overlay
DialogService.Instance.SetOverlayEnabled(false);

// Custom overlay color
DialogService.Instance.SetOverlayColor(Color.FromRgba("#CC000000"));
```

### Title Customization

Control how dialog titles are displayed across all dialog types:

```csharp
var theme = new DialogTheme
{
    TitleMaxLines = 2,                              // Maximum lines for title (default: 2)
    TitleLineBreakMode = LineBreakMode.TailTruncation, // How to truncate/wrap title text
    // ... other properties
};

// Available LineBreakMode options:
// - TailTruncation: "This is a very long..." (default) ✅ Works on all platforms
// - HeadTruncation: "...very long title" ⚠️ May not work on Windows
// - MiddleTruncation: "This is...title" ⚠️ May not work on Windows
// - WordWrap: Wraps at word boundaries ✅ Works on all platforms
// - CharacterWrap: Wraps at any character ✅ Works on all platforms
// - NoWrap: No wrapping, may overflow ✅ Works on all platforms

DialogService.Instance.Initialize(theme);
```

**Benefits:**
- Prevents layout issues with very long titles
- Maintains consistent dialog heights
- Works across all dialog types (Alert, Confirm, Prompt, etc.)
- Customizable per theme (light/dark can have different settings)

**Platform Notes:**
- `HeadTruncation` and `MiddleTruncation` may not render correctly on Windows due to MAUI framework limitations
- For cross-platform compatibility, use `TailTruncation` (default) or `WordWrap`

### Description Text Type (HTML Support)

Enable HTML formatting in dialog descriptions for rich text display:

```csharp
var theme = new DialogTheme
{
    DescriptionTextType = TextType.Html,  // Enable HTML formatting (default: Text)
    // ... other properties
};

DialogService.Instance.Initialize(theme);

// Now you can use HTML in descriptions
await AlertDialog.ShowAsync(
    "Welcome!",
    "This is <b>bold</b> and this is <i>italic</i>.<br/>New line here!",
    DialogType.Info
);
```

**Available TextType options:**
- `TextType.Text` - Plain text (default, no HTML parsing)
- `TextType.Html` - Renders basic HTML tags like `<b>`, `<i>`, `<u>`, `<br/>`, etc.

**Supported HTML tags:**
- `<b>`, `<strong>` - Bold text
- `<i>`, `<em>` - Italic text
- `<u>` - Underlined text
- `<br/>` - Line breaks
- Basic text formatting

**Use cases:**
- Formatted error messages with bold keywords
- Multi-line descriptions with proper line breaks
- Emphasized text within descriptions
- Rich informational dialogs

## 🌍 Localization

### Built-in Language Support

The library includes translations for:
- 🇬🇧 English (default)
- 🇪🇸 Spanish (es)
- 🇫🇷 French (fr)
- 🇩🇪 German (de)

### Custom Localization Implementation

```csharp
public class JapaneseLocalization : IDialogLocalization
{
    public string OkButtonText => "OK";
    public string CancelButtonText => "キャンセル";
    public string YesButtonText => "はい";
    public string NoButtonText => "いいえ";
    public string LoadingText => "読み込み中...";
    public string SelectPlaceholder => "選択してください";
    public string HexLabel => "16進数:";
    public string RedLabel => "赤";
    public string GreenLabel => "緑";
    public string BlueLabel => "青";
    public string AlphaLabel => "透明度";
    public string PresetColorsLabel => "プリセット色";
    public string ItemsScrollIndicator => "{0} 項目 (スクロールで表示)";

    public string GetString(string key) => key;
    public string GetString(string key, params object[] args)
        => string.Format(GetString(key), args);
}

// Apply the localization
DialogService.Instance.SetLocalization(new JapaneseLocalization());
```

### Culture-Based Automatic Localization

```csharp
// Automatically use device culture
var localization = new DefaultDialogLocalization(CultureInfo.CurrentCulture);
DialogService.Instance.SetLocalization(localization);

// Force specific culture
var spanishLocalization = new DefaultDialogLocalization(new CultureInfo("es-ES"));
DialogService.Instance.SetLocalization(spanishLocalization);
```

## 🎯 Advanced Features

### Custom Dialog Creation

```csharp
public class RatingDialog : BaseDialog
{
    private readonly TaskCompletionSource<int> _taskCompletionSource = new();
    private int _rating = 0;

    public RatingDialog(string title, string message)
    {
        var grid = new Grid
        {
            Padding = 20,
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            }
        };

        // Add title
        grid.Add(CreateTitleLabel(title), 0, 0);

        // Add message
        grid.Add(CreateDescriptionLabel(message), 0, 1);

        // Add star rating
        var starsLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 10
        };

        for (int i = 1; i <= 5; i++)
        {
            var star = new Label
            {
                Text = "⭐",
                FontSize = 30
            };

            int rating = i;
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => SetRating(rating);
            star.GestureRecognizers.Add(tap);

            starsLayout.Children.Add(star);
        }

        grid.Add(starsLayout, 0, 2);

        // Add buttons
        var buttonLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 10
        };

        var submitButton = CreatePrimaryButton("Submit", OnSubmit);
        var cancelButton = CreateSecondaryButton("Cancel", OnCancel);

        buttonLayout.Children.Add(cancelButton);
        buttonLayout.Children.Add(submitButton);

        grid.Add(buttonLayout, 0, 3);

        Content = CreateThemedFrame(grid);
    }

    private void SetRating(int rating)
    {
        _rating = rating;
        // Update star display
    }

    private async void OnSubmit(object sender, EventArgs e)
    {
        _taskCompletionSource.TrySetResult(_rating);
        await MopupService.Instance.PopAsync();
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        _taskCompletionSource.TrySetResult(0);
        await MopupService.Instance.PopAsync();
    }

    public async Task<int> ShowAsync()
    {
        await MopupService.Instance.PushAsync(this);
        return await _taskCompletionSource.Task;
    }
}
```

### Custom Icon Registration

```csharp
// Register icons for specific dialog types
DialogService.Instance.RegisterCustomIcon(
    DialogType.Custom,
    "custom_icon_light.svg",
    "custom_icon_dark.svg"
);

// Use in dialogs
await AlertDialog.ShowAsync(
    "Custom Alert",
    "This uses custom icons",
    DialogType.Custom
);

// Per-instance custom icons
var dialog = new AlertDialog("Title", "Message")
{
    CustomLightIcon = "special_light.png",
    CustomDarkIcon = "special_dark.png"
};
```

## 📋 Requirements

- **.NET 9.0** or higher
- **.NET MAUI**
- **Mopups** (automatically included as dependency)
- **Supported Platforms**: iOS 11.0+, Android 5.0 (API 21)+, Windows 10.0.17763.0+, macOS 10.15+

## ⚙️ Configuration Options

### Global Settings

```csharp
// Overlay configuration
DialogService.Instance.SetOverlayEnabled(true);
DialogService.Instance.SetOverlayColor(Color.FromRgba("#80000000"));

// Theme management
DialogService.Instance.CurrentThemeOverride = customTheme;

// Localization
DialogService.Instance.SetLocalization(customLocalization);

// Custom icons
DialogService.Instance.RegisterCustomIcon(type, lightIcon, darkIcon);
```

### Per-Dialog Settings

```csharp
var dialog = new AlertDialog("Title", "Message")
{
    // Custom padding for description
    DescriptionPadding = new Thickness(20, 10),

    // Custom icons
    CustomLightIcon = "icon_light.png",
    CustomDarkIcon = "icon_dark.png"
};
```

## 🐛 Troubleshooting

### Icons Not Displaying on Windows
The library handles Windows platform differences automatically. Icons are converted to platform-specific formats during build.

### Dialog Not Showing
Ensure you've called `.UseMopups()` in your MauiProgram.cs configuration.

### Theme Not Applying
Check that `CurrentThemeOverride` is not set if you want automatic theme detection.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 💬 Support

- 📧 Email: support@marketally.com
- 🐛 Issues: [GitHub Issues](https://github.com/MarketAlly/Dialogs.Maui/issues)

## 🙏 Acknowledgments

- Built with [.NET MAUI](https://github.com/dotnet/maui)
- Popup functionality powered by [Mopups](https://github.com/LuckyDucko/Mopups)
- Icons from Material Design

## 🆕 What's New in v1.1.0

### Enhanced Action List Dialog
- **Multi-line Description Support**: Descriptions can now wrap to 2, 3, or more lines
- **Configurable Line Break Modes**: Choose between tail truncation, word wrap, character wrap, head truncation, and middle truncation
- **Intelligent Scrolling**: Automatically shows scrollbar when content exceeds dialog height
- **Fixed Dialog Height**: Consistent dialog sizing with scrollable content area

### Improved Dialog Dismissal
- **Instant Dismissal**: Dialogs now respect the `EnableAnimation` theme setting
- **Disable Animations**: Set `DialogTheme.EnableAnimation = false` for instant dialog dismissal
- **Better UX**: Controls are disabled immediately on interaction to prevent double-taps

### API Enhancements
```csharp
// Configure description behavior
var dialog = new ActionListDialog(
    title: "Select Option",
    items: actions,
    cancelText: "Cancel",
    customHeight: null,
    descriptionMaxLines: 2,  // NEW: Control line wrapping
    descriptionLineBreakMode: LineBreakMode.TailTruncation  // NEW: Control truncation
);

// Dynamic updates
dialog.DescriptionMaxLines = 3;  // NEW: Change after creation
dialog.DescriptionLineBreakMode = LineBreakMode.WordWrap;  // NEW: Update behavior
```

## 📈 Roadmap

- [ ] Snackbar support
- [ ] Date/Time picker dialogs
- [ ] Custom animation effects
- [ ] More preset themes
- [ ] Additional language translations
- [ ] MVVM command binding support

---

**Made with ❤️ by MarketAlly**

*Building better user experiences, one dialog at a time.*