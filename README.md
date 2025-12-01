# MarketAlly.Dialogs.Maui

[![NuGet Version](https://img.shields.io/nuget/v/MarketAlly.Dialogs.Maui.svg?style=flat)](https://www.nuget.org/packages/MarketAlly.Dialogs.Maui/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MarketAlly.Dialogs.Maui.svg)](https://www.nuget.org/packages/MarketAlly.Dialogs.Maui/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/download)
[![Platform](https://img.shields.io/badge/Platform-iOS%20%7C%20Android%20%7C%20Windows%20%7C%20macOS-lightgray)](https://dotnet.microsoft.com/apps/maui)

A comprehensive, production-ready dialog library for .NET MAUI applications with built-in theming, localization, hierarchical menus, and extensive customization options.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Dialog Types](#dialog-types)
- [Notifications](#notifications)
- [Theming](#theming)
- [Localization](#localization)
- [Advanced Features](#advanced-features)
- [API Reference](#api-reference) | [Full API Documentation](API_REFERENCE.md)
- [Requirements](#requirements)
- [Migration Guide](#migration-guide)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

## Features

### Core Capabilities

- **9 Dialog Types**: Alert, Confirm, Prompt, Editor, Loading, Action List, Color Picker, Toast, and Snackbar
- **Toast & Snackbar**: Lightweight notifications with optional actions and stacking (v1.4.0+)
- **Hierarchical Menus**: Multi-level action list navigation with automatic back navigation (v1.3.0+)
- **Adaptive Theming**: Automatic dark/light theme detection with full customization
- **Internationalization**: Built-in support for English, Spanish, French, and German
- **HTML Description Support**: Rich text formatting in dialog descriptions
- **Cross-Platform**: iOS 11.0+, Android API 21+, Windows 10 (17763+), macOS 10.15+

### Technical Highlights

- **Type-Safe APIs**: Strongly typed with comprehensive IntelliSense support
- **Async/Await Pattern**: Modern asynchronous dialog handling
- **Memory Efficient**: Intelligent image caching and resource management
- **Extensible Architecture**: Easy to create custom dialogs via `BaseDialog`
- **Thread-Safe**: Singleton service pattern with proper synchronization
- **Symbol Package Support**: Full debugging support with `.snupkg` packages

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package MarketAlly.Dialogs.Maui
```

Or via Package Manager Console:

```powershell
Install-Package MarketAlly.Dialogs.Maui
```

Or search for `MarketAlly.Dialogs.Maui` in the NuGet Package Manager UI.

## Quick Start

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

## Dialog Types

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

### Confirm Dialog

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

### Prompt Dialog

Collect single-line text input with validation support and password visibility toggle.

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

### Editor Dialog

Collect multi-line text with configurable constraints, spell check, and text prediction.

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

Show progress indicators with optional cancellation support.

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

Present a list of actions with optional icons, descriptions, and hierarchical sub-menus. Supports multi-line descriptions with customizable truncation and wrapping behavior.

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

// Hierarchical menus (NEW in v1.3.0)
var menuWithSubItems = new List<ActionItem>
{
    new ActionItem("File", "File operations", 0)
    {
        SubItems = new List<ActionItem>
        {
            new ActionItem("New", "Create new file", 10),
            new ActionItem("Open", "Open existing file", 11),
            new ActionItem("Save", "Save current file", 12)
        }
    },
    new ActionItem("Edit", "Edit operations", 1)
    {
        SubItems = new List<ActionItem>
        {
            new ActionItem("Cut", "Cut selection", 20),
            new ActionItem("Copy", "Copy selection", 21),
            new ActionItem("Paste", "Paste from clipboard", 22)
        }
    },
    new ActionItem("Settings", "Open settings", 2)
};

var hierarchicalDialog = new ActionListDialog(
    "Main Menu",
    menuWithSubItems,
    "Cancel"
);

// User can navigate through sub-menus
// Back button automatically handles navigation
int selected = await hierarchicalDialog.ShowAsync();

// Multi-line descriptions
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
    descriptionMaxLines: 2,
    descriptionLineBreakMode: LineBreakMode.TailTruncation
);

int selectedFeature = await multiLineDialog.ShowAsync();
```

### Color Picker Dialog

Advanced color selection with RGB sliders, hex input, alpha channel support, and preset colors.

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

## Notifications

### Toast

Lightweight, non-interactive "fire-and-forget" notifications for quick status updates.

```csharp
// Simple toast
await Toast.ShowAsync("Message sent");

// With icon
await Toast.ShowAsync("Operation complete!", DialogType.Success);

// With icon and duration
await Toast.ShowAsync("Something went wrong", DialogType.Error, ToastDuration.Long);

// Vertical position (top or bottom)
await Toast.ShowAsync(
    "This appears at the top",
    DialogType.Info,
    ToastDuration.Short,
    ToastPosition.Top
);

// Full position control (vertical + horizontal)
await Toast.ShowAsync(
    "Bottom right corner!",
    DialogType.Success,
    ToastDuration.Short,
    ToastPosition.Bottom,
    ToastHorizontalPosition.Right
);

// All corner positions
await Toast.ShowAsync("Top Left", DialogType.Info, ToastDuration.Short, ToastPosition.Top, ToastHorizontalPosition.Left);
await Toast.ShowAsync("Top Right", DialogType.Warning, ToastDuration.Short, ToastPosition.Top, ToastHorizontalPosition.Right);
await Toast.ShowAsync("Bottom Left", DialogType.Success, ToastDuration.Short, ToastPosition.Bottom, ToastHorizontalPosition.Left);
await Toast.ShowAsync("Bottom Right", DialogType.Error, ToastDuration.Short, ToastPosition.Bottom, ToastHorizontalPosition.Right);

// Custom duration in milliseconds
await Toast.ShowAsync("Custom timing", DialogType.None, 5000, ToastPosition.Bottom);

// Dismiss all toasts
await Toast.DismissAllAsync();
```

**Toast Features:**
- **Vertical Position**: Top or Bottom of screen (default: Bottom)
- **Horizontal Position**: Left, Center, or Right (default: Center)
- **Duration**: Short (2s) or Long (3.5s), or custom milliseconds
- **Icons**: Supports all DialogType icons
- **Stacking**: Multiple toasts can stack, replace, or queue
- **Non-blocking**: User can continue interacting with the app

**Stacking Behavior Configuration:**

```csharp
// Stack multiple toasts (default)
Toast.DefaultStackBehavior = ToastStackBehavior.Stack;
Toast.MaxVisibleToasts = 3;  // Maximum visible at once

// Replace existing toasts
Toast.DefaultStackBehavior = ToastStackBehavior.Replace;

// Queue toasts (show one at a time)
Toast.DefaultStackBehavior = ToastStackBehavior.Queue;
```

### Snackbar

Actionable notifications with optional buttons for quick user responses.

```csharp
// Simple snackbar
var result = await Snackbar.ShowAsync("File saved to documents");

// With action button
var result = await Snackbar.ShowAsync("Item deleted", "UNDO");
if (result == SnackbarResult.ActionClicked)
{
    // User clicked UNDO
    RestoreItem();
}

// With action callback
var result = await Snackbar.ShowAsync(
    "Message archived",
    "UNDO",
    () => UnarchiveMessage()  // Called when UNDO is clicked
);

// Full customization
var result = await Snackbar.ShowAsync(
    message: "Connection lost",
    actionText: "RETRY",
    actionCallback: () => Reconnect(),
    iconType: DialogType.Error,
    duration: SnackbarDuration.Long,
    position: ToastPosition.Bottom
);

// Indefinite snackbar (stays until user interaction)
var result = await Snackbar.ShowAsync(
    "No internet connection",
    "RETRY",
    null,
    DialogType.Warning,
    SnackbarDuration.Indefinite,
    ToastPosition.Bottom
);

// Dismiss all snackbars
await Snackbar.DismissAllAsync();
```

**Snackbar Features:**
- **Action Button**: Optional button with callback (UNDO, RETRY, VIEW, etc.)
- **Position**: Top or Bottom of screen (default: Bottom)
- **Duration**: Short (4s), Long (7s), or Indefinite
- **Icons**: Supports all DialogType icons
- **Swipe to Dismiss**: Users can swipe to dismiss
- **Stacking**: Multiple snackbars can stack, replace, or queue
- **Result Tracking**: Returns `SnackbarResult` (ActionClicked, Dismissed, TimedOut)

**Stacking Behavior Configuration:**

```csharp
// Stack multiple snackbars (default)
Snackbar.DefaultStackBehavior = SnackbarStackBehavior.Stack;
Snackbar.MaxVisibleSnackbars = 3;

// Replace existing snackbars
Snackbar.DefaultStackBehavior = SnackbarStackBehavior.Replace;

// Queue snackbars
Snackbar.DefaultStackBehavior = SnackbarStackBehavior.Queue;
```

**SnackbarResult Enumeration:**

```csharp
public enum SnackbarResult
{
    Dismissed,      // User dismissed (swipe, tap outside, back button)
    ActionClicked,  // User clicked the action button
    TimedOut        // Snackbar auto-dismissed after duration
}
```

### When to Use Toast vs Snackbar

| Use Case | Recommended |
|----------|-------------|
| "Saved" / "Copied" / "Sent" | Toast |
| "Item deleted" with UNDO | Snackbar |
| "Connection lost" with RETRY | Snackbar |
| "Settings updated" | Toast |
| "File uploaded" with VIEW | Snackbar |
| Quick status confirmation | Toast |
| Actions that might need reversal | Snackbar |

## Theming

The library provides comprehensive theming support with automatic dark/light mode detection.

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

## Localization

The library includes a complete localization framework with built-in translations and extensibility.

### Built-in Language Support

- **English** (default)
- **Spanish** (es)
- **French** (fr)
- **German** (de)

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

## Advanced Features

### Custom Dialog Creation

Extend `BaseDialog` to create custom dialogs with full theming support:

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

## API Reference

> **For comprehensive API documentation including all methods, properties, and examples, see [API_REFERENCE.md](API_REFERENCE.md)**

### DialogService (Singleton)

The central service for managing dialogs, themes, and localization.

```csharp
// Access the singleton instance
var service = DialogService.Instance;

// Initialize with custom themes
service.Initialize(lightTheme, darkTheme);

// Theme management
service.CurrentThemeOverride = customTheme;  // Override automatic detection
service.LightTheme;                           // Get light theme
service.DarkTheme;                            // Get dark theme

// Overlay settings
service.SetOverlayEnabled(true);
service.SetOverlayColor(Color.FromRgba("#80000000"));

// Localization
service.SetLocalization(new CustomLocalization());

// Custom icon registration
service.RegisterCustomIcon(DialogType.Custom, "light.png", "dark.png");
```

### DialogType Enumeration

```csharp
public enum DialogType
{
    None,       // No icon
    Info,       // Information icon (blue)
    Success,    // Success icon (green)
    Warning,    // Warning icon (orange)
    Error,      // Error icon (red)
    Question,   // Question icon (purple)
    Help,       // Help icon
    Custom      // Custom icon (user-defined)
}
```

### DialogTheme Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `BackgroundColor` | `Color` | White/Dark | Dialog background |
| `OverlayColor` | `Color` | #80000000 | Semi-transparent overlay |
| `ShowOverlay` | `bool` | true | Enable background overlay |
| `TitleTextColor` | `Color` | #212121 | Title text color |
| `TitleFontSize` | `double` | 18 | Title font size |
| `TitleMaxLines` | `int` | 2 | Maximum title lines |
| `TitleLineBreakMode` | `LineBreakMode` | TailTruncation | Title truncation mode |
| `DescriptionTextColor` | `Color` | #757575 | Description text color |
| `DescriptionFontSize` | `double` | 14 | Description font size |
| `DescriptionTextType` | `TextType` | Text | Text or HTML rendering |
| `ButtonBackgroundColor` | `Color` | #2196F3 | Primary button background |
| `ButtonTextColor` | `Color` | White | Primary button text |
| `SecondaryButtonBackgroundColor` | `Color` | #F5F5F5 | Secondary button background |
| `SecondaryButtonTextColor` | `Color` | #212121 | Secondary button text |
| `DialogWidth` | `double` | 300 | Dialog width |
| `DialogHeight` | `double` | 250 | Dialog height |
| `DialogCornerRadius` | `double` | 12 | Corner radius |
| `DialogPadding` | `double` | 20 | Internal padding |
| `ButtonHeight` | `double` | 44 | Button height |
| `HasShadow` | `bool` | true | Enable drop shadow |
| `AnimationDuration` | `int` | 250 | Animation duration (ms) |
| `EnableAnimation` | `bool` | true | Enable/disable animations |

### ActionItem Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Display name |
| `Detail` | `string?` | Optional description |
| `Value` | `int` | Return value when selected |
| `ImageDark` | `string?` | Dark theme icon path |
| `ImageLight` | `string?` | Light theme icon path |
| `SubItems` | `List<ActionItem>?` | Hierarchical sub-menu items |
| `ItemId` | `Guid` | Unique identifier |
| `ShowImage` | `bool` | Whether to show icon (computed) |
| `HasDetail` | `bool` | Whether detail text exists (computed) |
| `HasSubItems` | `bool` | Whether sub-items exist (computed) |

## Requirements

### Minimum Platform Versions

| Platform | Minimum Version |
|----------|-----------------|
| .NET | 9.0 |
| iOS | 11.0 |
| Android | API 21 (5.0 Lollipop) |
| Windows | 10.0.17763.0 (1809) |
| macOS | 13.1 (via Mac Catalyst) |

### Dependencies

- **Microsoft.Maui.Controls** (9.0.110+)
- **Microsoft.Maui.Controls.Compatibility** (9.0.110+)
- **Mopups** (1.3.4+) - Automatically included

## Migration Guide

### Upgrading from v1.3.x to v1.4.0

No breaking changes. New features are additive:

```csharp
// NEW: Toast notifications
await Toast.ShowAsync("Message sent", DialogType.Success);

// NEW: Snackbar with action
var result = await Snackbar.ShowAsync("Item deleted", "UNDO", () => RestoreItem());

// NEW: Configure stacking behavior
Toast.DefaultStackBehavior = ToastStackBehavior.Stack;
Snackbar.DefaultStackBehavior = SnackbarStackBehavior.Replace;
```

### Upgrading from v1.2.x to v1.3.0

No breaking changes. New features are additive:

```csharp
// NEW: Hierarchical menus
var item = new ActionItem("Menu", "Description", 0);
item.SubItems = new List<ActionItem> { /* sub-items */ };
```

### Upgrading from v1.1.x to v1.2.0

No breaking changes. New theme properties:

```csharp
// NEW: Title customization
theme.TitleMaxLines = 2;
theme.TitleLineBreakMode = LineBreakMode.TailTruncation;

// NEW: HTML support
theme.DescriptionTextType = TextType.Html;
```

### Upgrading from v1.0.x to v1.1.0

No breaking changes. New ActionListDialog features:

```csharp
// NEW: Multi-line descriptions
var dialog = new ActionListDialog(
    title, actions, cancelText,
    customHeight: null,
    descriptionMaxLines: 2,                    // NEW
    descriptionLineBreakMode: LineBreakMode.TailTruncation  // NEW
);

// NEW: Dynamic updates
dialog.DescriptionMaxLines = 3;
dialog.DescriptionLineBreakMode = LineBreakMode.WordWrap;
```

## Configuration Options

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

## Troubleshooting

### Common Issues

#### Dialog Not Showing

**Problem**: Dialog doesn't appear when calling `ShowAsync()`.

**Solution**: Ensure Mopups is configured in `MauiProgram.cs`:
```csharp
builder.ConfigureMopups();
```

#### Icons Not Displaying

**Problem**: Dialog icons appear blank or missing.

**Solution**:
1. Verify icon files are included in the project as `MauiImage`
2. Check that file names match exactly (case-sensitive)
3. For Windows, ensure PNG format is used

#### Theme Not Applying

**Problem**: Custom theme changes aren't reflected.

**Solution**:
```csharp
// Clear override to use automatic detection
DialogService.Instance.CurrentThemeOverride = null;

// Or explicitly set the theme
DialogService.Instance.CurrentThemeOverride = customTheme;
```

#### Memory Warnings on iOS

**Problem**: App receives memory warnings with many dialogs.

**Solution**: The library includes automatic image caching. Ensure you're not creating redundant dialog instances.

#### Title Truncation Issues on Windows

**Problem**: `HeadTruncation` or `MiddleTruncation` don't work on Windows.

**Solution**: Use platform-compatible modes:
```csharp
theme.TitleLineBreakMode = LineBreakMode.TailTruncation; // Recommended
// or
theme.TitleLineBreakMode = LineBreakMode.WordWrap;
```

#### ActionListDialog "Duplicate Key" Exception

**Problem**: Exception when showing dialog immediately after another dismisses.

**Solution**: This was fixed in v1.3.0. Upgrade to latest version or ensure proper async/await usage:
```csharp
var result = await actionListDialog.ShowAsync();
// Dialog is fully dismissed before this line executes
await nextDialog.ShowAsync(); // Safe to show next dialog
```

### Platform-Specific Notes

**iOS**
- Supports all features including gesture recognizers
- Memory-optimized image loading

**Android**
- Full material design integration
- Hardware back button support in hierarchical menus

**Windows**
- Some `LineBreakMode` options have limited support
- Recommend using `TailTruncation` or `WordWrap`

**macOS (Catalyst)**
- Keyboard navigation supported
- Dark mode follows system preferences

## Contributing

We welcome contributions from the community.

### Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/MarketAlly/Dialogs.Maui.git
   ```

2. Open solution in Visual Studio 2022 or JetBrains Rider

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Build the solution:
   ```bash
   dotnet build
   ```

### Contribution Guidelines

1. **Fork the Repository**: Create your own fork on GitHub

2. **Create a Feature Branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Follow Code Standards**:
   - Use consistent naming conventions
   - Add XML documentation for public APIs
   - Include unit tests for new features

4. **Commit with Clear Messages**:
   ```bash
   git commit -m "Add: Brief description of feature"
   ```

5. **Push and Create PR**:
   ```bash
   git push origin feature/your-feature-name
   ```
   Then open a Pull Request on GitHub

6. **PR Requirements**:
   - Clear description of changes
   - No breaking changes without discussion
   - All tests passing
   - Updated documentation if needed

### Code Style

- Use C# 12 features where appropriate
- Enable nullable reference types
- Follow .NET naming conventions
- Keep methods focused and small

## Support

### Getting Help

- **GitHub Issues**: [Report bugs or request features](https://github.com/MarketAlly/Dialogs.Maui/issues)
- **Documentation**: Full API documentation in this README
- **Email**: support@marketally.com

### Reporting Issues

When reporting bugs, please include:
1. .NET MAUI version
2. Target platform(s) affected
3. Minimal reproduction code
4. Expected vs. actual behavior
5. Stack trace (if applicable)

## License

This project is licensed under the **MIT License**.

```
MIT License

Copyright (c) 2025 MarketAlly

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Acknowledgments

- Built on [.NET MAUI](https://github.com/dotnet/maui) by Microsoft
- Popup infrastructure powered by [Mopups](https://github.com/LuckyDucko/Mopups) by LuckyDucko
- Inspired by Material Design principles

## Changelog

### Version 1.4.0 (Latest)

**New Features:**
- **Toast Notifications**: Lightweight, non-interactive notifications for quick status updates
  - Configurable vertical position (Top/Bottom)
  - Configurable horizontal position (Left/Center/Right) - show toasts in any corner
  - Short (2s) and Long (3.5s) durations, or custom milliseconds
  - Optional icons using existing DialogType
  - Configurable stacking behavior (Stack, Replace, Queue)
- **Snackbar Notifications**: Actionable notifications with optional buttons
  - Action button with callback support (UNDO, RETRY, VIEW, etc.)
  - Short (4s), Long (7s), or Indefinite duration
  - Swipe-to-dismiss support
  - Returns SnackbarResult (ActionClicked, Dismissed, TimedOut)
  - Configurable stacking behavior
- **New Localization Strings**: Added DISMISS, UNDO, RETRY translations for all 4 languages
- **ToastHorizontalPosition Enum**: Left, Center, Right positioning for toasts

**Improvements:**
- Non-blocking notifications allow continued user interaction
- Multiple notifications can stack vertically
- Consistent theming with existing dialog components
- Corner-positioned toasts appear instantly (no slide-from-center animation)

### Version 1.3.0

**New Features:**
- Hierarchical Action List Support with `ActionItem.SubItems`
- Automatic back navigation in sub-menus
- Unlimited nesting depth for menu hierarchies

**Bug Fixes:**
- Fixed critical "duplicate key" exception on rapid dialog transitions
- Fixed async race condition in `PopAsync` handling

### Version 1.2.0

**New Features:**
- Title MaxLines and LineBreakMode configuration
- HTML description support via `DescriptionTextType`

### Version 1.1.0

**New Features:**
- Multi-line description support in ActionListDialog
- Configurable line break modes
- Dynamic property updates after dialog creation
- Intelligent scrolling for content overflow

**Improvements:**
- Fixed dialog height with scrollable content
- Instant dismissal option via `EnableAnimation`
- Double-tap prevention

### Version 1.0.0

**Initial Release:**
- 7 dialog types (Alert, Confirm, Prompt, Editor, Loading, ActionList, ColorPicker)
- Dark/light theme support with automatic detection
- Internationalization (English, Spanish, French, German)
- Custom icon support
- Cross-platform support (iOS, Android, Windows, macOS)

## Roadmap

Planned features for future releases:

- [x] **Snackbar/Toast notifications** - Non-blocking notifications ✅ Added in v1.4.0
- [ ] **Date/Time picker dialogs** - Native date and time selection
- [ ] **Custom animation effects** - Slide, fade, scale transitions
- [ ] **Preset theme gallery** - Material, Fluent, Cupertino themes
- [ ] **Additional localizations** - Chinese, Japanese, Portuguese, Italian
- [ ] **MVVM command binding** - ICommand support for button actions
- [ ] **Input validation framework** - Built-in validators for Prompt/Editor
- [ ] **Accessibility improvements** - Enhanced screen reader support
- [ ] **Performance telemetry** - Optional analytics for dialog usage

---

**Built with precision by [MarketAlly](https://marketally.com)**

*Enterprise-grade dialog solutions for .NET MAUI applications.*