# MarketAlly.Dialogs.Maui - API Reference

Complete API documentation for all public classes, methods, properties, and enumerations.

## Table of Contents

- [Dialogs](#dialogs)
  - [AlertDialog](#alertdialog)
  - [ConfirmDialog](#confirmdialog)
  - [PromptDialog](#promptdialog)
  - [EditorDialog](#editordialog)
  - [LoadingDialog](#loadingdialog)
  - [ActionListDialog](#actionlistdialog)
  - [ColorPickerDialog](#colorpickerdialog)
- [Notifications](#notifications)
  - [Toast](#toast)
  - [Snackbar](#snackbar)
- [Core Services](#core-services)
  - [DialogService](#dialogservice)
  - [BaseDialog](#basedialog)
- [Models](#models)
  - [DialogTheme](#dialogtheme)
  - [DialogType](#dialogtype)
  - [ActionItem](#actionitem)
- [Enumerations](#enumerations)
  - [ToastPosition](#toastposition)
  - [ToastHorizontalPosition](#toasthorizontalposition)
  - [ToastDuration](#toastduration)
  - [ToastStackBehavior](#toaststackbehavior)
  - [SnackbarDuration](#snackbarduration)
  - [SnackbarResult](#snackbarresult)
  - [SnackbarStackBehavior](#snackbarstackbehavior)
- [Interfaces](#interfaces)
  - [IDialogLocalization](#idialoglocalization)
- [Localization](#localization)
  - [DefaultDialogLocalization](#defaultdialoglocalization)

---

## Dialogs

### AlertDialog

Displays an informational dialog with a single button.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `BaseDialog` → `PopupPage`

#### Constructors

```csharp
public AlertDialog(
    string title,
    string description,
    string? okText = null,
    DialogType type = DialogType.None)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `title` | `string` | The dialog title |
| `description` | `string` | The dialog message/description |
| `okText` | `string?` | Custom OK button text (default: localized "OK") |
| `type` | `DialogType` | Icon type to display |

#### Static Methods

```csharp
public static Task<bool> ShowAsync(string title, DialogType type = DialogType.None)
```
Shows an alert with title only.

```csharp
public static Task<bool> ShowAsync(string title, string description, DialogType type = DialogType.None)
```
Shows an alert with title and description.

```csharp
public static Task<bool> ShowAsync(
    string title,
    string description,
    string? okText,
    DialogType type = DialogType.None)
```
Shows an alert with custom button text.

```csharp
public static Task<bool> ShowAsync(
    string title,
    string description,
    int maxLines,
    DialogType type = DialogType.None)
```
Shows an alert with limited description lines.

```csharp
public static Task HideAsync()
```
Hides the currently displayed alert dialog.

#### Instance Methods

```csharp
public void SetTitle(string title)
```
Updates the dialog title.

```csharp
public void SetDescription(string description)
```
Updates the dialog description.

```csharp
public void SetDescriptionMaxLines(int maxLines)
```
Sets the maximum number of lines for the description.

```csharp
public void SetTitleLineBreakMode(LineBreakMode mode)
```
Sets how the title handles overflow.

```csharp
public LineBreakMode GetTitleLineBreakMode()
```
Gets the current title line break mode.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `DescriptionPadding` | `Thickness` | Padding around the description (inherited from BaseDialog) |
| `CustomLightIcon` | `string?` | Custom icon for light theme (inherited) |
| `CustomDarkIcon` | `string?` | Custom icon for dark theme (inherited) |

#### Examples

```csharp
// Simple alert
await AlertDialog.ShowAsync("Success!", DialogType.Success);

// With description
await AlertDialog.ShowAsync(
    "Error",
    "Failed to save the file. Please try again.",
    DialogType.Error);

// Custom button text
await AlertDialog.ShowAsync(
    "Notice",
    "Your session will expire in 5 minutes.",
    "Got it",
    DialogType.Warning);

// Instance with customization
var dialog = new AlertDialog(
    "Important",
    "This is a very long description that might need special handling...",
    "Acknowledge",
    DialogType.Info);
dialog.SetDescriptionMaxLines(3);
dialog.DescriptionPadding = new Thickness(20, 10);
await MopupService.Instance.PushAsync(dialog);
```

---

### ConfirmDialog

Displays a confirmation dialog with two buttons (confirm/cancel).

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `BaseDialog` → `PopupPage`

#### Constructors

```csharp
public ConfirmDialog(
    string title,
    string description,
    string? confirmText = null,
    string? cancelText = null,
    DialogType type = DialogType.None,
    double? dialogHeight = null)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `title` | `string` | The dialog title |
| `description` | `string` | The dialog message/description |
| `confirmText` | `string?` | Custom confirm button text (default: localized "Yes") |
| `cancelText` | `string?` | Custom cancel button text (default: localized "No") |
| `type` | `DialogType` | Icon type to display |
| `dialogHeight` | `double?` | Custom dialog height |

#### Static Methods

```csharp
public static Task<bool> ShowAsync(
    string title,
    string description,
    DialogType type = DialogType.None)
```
Shows a confirmation dialog. Returns `true` if confirmed, `false` if cancelled.

```csharp
public static Task<bool> ShowAsync(
    string title,
    string description,
    string? confirmText,
    string? cancelText,
    DialogType type = DialogType.None,
    double? dialogHeight = null)
```
Shows a confirmation dialog with full customization.

```csharp
public static Task HideAsync()
```
Hides the currently displayed confirm dialog.

#### Instance Methods

```csharp
public async Task<bool> ShowAsync()
```
Displays the dialog and returns the result.

```csharp
public void SetTitle(string title)
```
Updates the dialog title.

```csharp
public void SetDescription(string description)
```
Updates the dialog description.

```csharp
public void SetDescriptionMaxLines(int maxLines)
```
Sets the maximum number of lines for the description.

#### Examples

```csharp
// Simple confirmation
bool confirmed = await ConfirmDialog.ShowAsync(
    "Delete Item",
    "Are you sure you want to delete this item?",
    DialogType.Warning);

if (confirmed)
{
    await DeleteItem();
}

// Custom buttons
bool result = await ConfirmDialog.ShowAsync(
    "Save Changes",
    "Do you want to save your changes before closing?",
    "Save",
    "Discard",
    DialogType.Decide);

// Instance usage
var dialog = new ConfirmDialog(
    "Confirm Action",
    "This action cannot be undone.",
    "Proceed",
    "Cancel",
    DialogType.Warning,
    dialogHeight: 200);

bool result = await dialog.ShowAsync();
```

---

### PromptDialog

Displays a dialog with a single-line text input field.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `BaseDialog` → `PopupPage`

#### Constructors

```csharp
public PromptDialog(
    string title,
    string? description = null,
    string? placeholder = null,
    string? defaultValue = null,
    string? okText = null,
    string? cancelText = null,
    DialogType type = DialogType.None,
    Keyboard? keyboard = null,
    bool isPassword = false)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `title` | `string` | The dialog title |
| `description` | `string?` | Optional description text |
| `placeholder` | `string?` | Placeholder text for the input field |
| `defaultValue` | `string?` | Pre-filled value in the input |
| `okText` | `string?` | Custom OK button text |
| `cancelText` | `string?` | Custom cancel button text |
| `type` | `DialogType` | Icon type to display |
| `keyboard` | `Keyboard?` | Keyboard type (Text, Numeric, Email, etc.) |
| `isPassword` | `bool` | Whether to mask input as password |

#### Static Methods

```csharp
public static Task<string?> ShowAsync(
    string title,
    string? description = null,
    string? placeholder = null,
    DialogType type = DialogType.None)
```
Shows a basic prompt dialog. Returns the entered text or `null` if cancelled.

```csharp
public static Task<string?> ShowAsync(
    string title,
    string? description,
    string? placeholder,
    string? defaultValue,
    string? okText,
    string? cancelText,
    DialogType type = DialogType.None)
```
Shows a prompt dialog with custom button text.

```csharp
public static Task<string?> ShowAsync(
    string title,
    string? description,
    string? placeholder,
    string? defaultValue,
    Keyboard keyboard,
    DialogType type = DialogType.None)
```
Shows a prompt dialog with specific keyboard type.

```csharp
public static Task HideAsync()
```
Hides the currently displayed prompt dialog.

#### Instance Methods

```csharp
public async Task<string?> ShowAsync()
```
Displays the dialog and returns the entered text.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Text` | `string` | Gets or sets the current input text |
| `Description` | `string?` | Gets or sets the description |
| `Placeholder` | `string?` | Gets or sets the placeholder text |
| `Keyboard` | `Keyboard` | Gets or sets the keyboard type |
| `IsPassword` | `bool` | Gets or sets password mode (shows/hides toggle) |

#### Examples

```csharp
// Simple text input
string? name = await PromptDialog.ShowAsync(
    "Enter Name",
    "Please enter your full name",
    "John Doe");

// Email input with validation keyboard
string? email = await PromptDialog.ShowAsync(
    "Email Address",
    "Enter your email",
    "user@example.com",
    null,
    Keyboard.Email,
    DialogType.Info);

// Password input with visibility toggle
var dialog = new PromptDialog(
    "Enter Password",
    "Your password is required",
    "••••••••",
    null,
    "Login",
    "Cancel",
    DialogType.None,
    Keyboard.Text,
    isPassword: true);

string? password = await dialog.ShowAsync();

// Numeric input
string? amount = await PromptDialog.ShowAsync(
    "Enter Amount",
    "How much would you like to transfer?",
    "0.00",
    null,
    Keyboard.Numeric,
    DialogType.None);
```

---

### EditorDialog

Displays a dialog with a multi-line text editor.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `BaseDialog` → `PopupPage`

#### Constructors

```csharp
public EditorDialog(
    string title,
    string? description = null,
    string? placeholder = null,
    string? okText = null,
    string? cancelText = null,
    DialogType type = DialogType.None,
    Keyboard? keyboard = null,
    int minLines = 3,
    int maxLines = 10)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `title` | `string` | The dialog title |
| `description` | `string?` | Optional description text |
| `placeholder` | `string?` | Placeholder text for the editor |
| `okText` | `string?` | Custom OK button text |
| `cancelText` | `string?` | Custom cancel button text |
| `type` | `DialogType` | Icon type to display |
| `keyboard` | `Keyboard?` | Keyboard type |
| `minLines` | `int` | Minimum visible lines (default: 3) |
| `maxLines` | `int` | Maximum visible lines (default: 10) |

#### Static Methods

```csharp
public static Task<string?> ShowAsync(
    string title,
    string? description = null,
    string? placeholder = null,
    DialogType type = DialogType.None)
```
Shows a basic editor dialog. Returns the entered text or `null` if cancelled.

```csharp
public static Task<string?> ShowAsync(
    string title,
    string? description,
    string? placeholder,
    string? okText,
    string? cancelText,
    DialogType type = DialogType.None,
    int minLines = 3,
    int maxLines = 10)
```
Shows an editor dialog with full customization.

```csharp
public static Task HideAsync()
```
Hides the currently displayed editor dialog.

#### Instance Methods

```csharp
public async Task<string?> ShowAsync()
```
Displays the dialog and returns the entered text.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Text` | `string` | Gets or sets the current editor text |
| `Placeholder` | `string?` | Gets or sets the placeholder text |
| `Keyboard` | `Keyboard` | Gets or sets the keyboard type |
| `IsSpellCheckEnabled` | `bool` | Enables/disables spell checking |
| `IsTextPredictionEnabled` | `bool` | Enables/disables text prediction |

#### Examples

```csharp
// Simple notes editor
string? notes = await EditorDialog.ShowAsync(
    "Add Notes",
    "Enter your notes below",
    "Type here...",
    DialogType.None);

// Feedback form with spell check
var dialog = new EditorDialog(
    "Feedback",
    "Please share your thoughts",
    "Your feedback helps us improve...",
    "Submit",
    "Cancel",
    DialogType.Help,
    Keyboard.Text,
    minLines: 5,
    maxLines: 15);

dialog.IsSpellCheckEnabled = true;
dialog.IsTextPredictionEnabled = true;

string? feedback = await dialog.ShowAsync();
```

---

### LoadingDialog

Displays a loading indicator with optional cancellation.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `BaseDialog` → `PopupPage`

**Implements:** `IDisposable`

#### Constructors

```csharp
public LoadingDialog(string? label = null, bool canCancel = false)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `label` | `string?` | Text to display below the spinner |
| `canCancel` | `bool` | Whether to show a cancel button |

#### Static Methods

```csharp
public static Task ShowAsync(string label, Func<Task> action)
```
Shows loading dialog, executes action, then auto-dismisses.

```csharp
public static Task<bool> ShowCancelableAsync(string label, Func<Task> action)
```
Shows cancelable loading dialog. Returns `true` if cancelled.

```csharp
public static Task<bool> ShowCancelableAsync(
    string label,
    Func<CancellationToken, Task> action)
```
Shows cancelable loading with CancellationToken support.

```csharp
public static Task<LoadingDialog> ShowAsync(string? label = null, bool canCancel = false)
```
Shows loading dialog and returns the instance for manual control.

```csharp
public static Task HideAsync()
```
Hides the currently displayed loading dialog.

#### Instance Methods

```csharp
public void UpdateText(string text)
```
Updates the loading label text.

```csharp
public void Dispose()
```
Disposes the dialog resources.

#### Examples

```csharp
// Auto-dismiss after action completes
await LoadingDialog.ShowAsync("Saving...", async () =>
{
    await SaveDataAsync();
});

// Cancelable with progress updates
bool wasCanceled = await LoadingDialog.ShowCancelableAsync(
    "Downloading...",
    async (cancellationToken) =>
    {
        for (int i = 0; i < 100; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(100);
        }
    });

if (wasCanceled)
{
    await AlertDialog.ShowAsync("Download cancelled", DialogType.Info);
}

// Manual control
var loading = await LoadingDialog.ShowAsync("Processing...");
try
{
    await Step1();
    loading.UpdateText("Step 2 of 3...");
    await Step2();
    loading.UpdateText("Finishing up...");
    await Step3();
}
finally
{
    await LoadingDialog.HideAsync();
}
```

---

### ActionListDialog

Displays a scrollable list of selectable actions with optional icons and hierarchical sub-menus.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `BaseDialog` → `PopupPage`

#### Constructors

```csharp
public ActionListDialog(
    string title,
    List<ActionItem> items,
    string? cancelText = null,
    double? customHeight = null,
    int descriptionMaxLines = 1,
    LineBreakMode descriptionLineBreakMode = LineBreakMode.TailTruncation)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `title` | `string` | The dialog title |
| `items` | `List<ActionItem>` | List of selectable items |
| `cancelText` | `string?` | Custom cancel button text |
| `customHeight` | `double?` | Custom dialog height |
| `descriptionMaxLines` | `int` | Max lines for item descriptions (default: 1) |
| `descriptionLineBreakMode` | `LineBreakMode` | How descriptions handle overflow |

#### Static Methods

```csharp
public static Task<int> ShowAsync(
    string title,
    List<ActionItem> items,
    string? cancelText = null,
    double? customHeight = null,
    int descriptionMaxLines = 1,
    LineBreakMode descriptionLineBreakMode = LineBreakMode.TailTruncation)
```
Shows action list dialog. Returns selected item's `Value` or `-1` if cancelled.

```csharp
public static Task<bool> ShowWithActionsAsync(
    string title,
    List<ActionItem> items,
    string? cancelText = null)
```
Shows action list dialog with action callbacks. When an item with `Action` or `AsyncAction` is selected, the callback is automatically invoked. Returns `true` if an action was selected, `false` if cancelled.

```csharp
public static Task HideAsync()
```
Hides the currently displayed action list dialog.

#### Instance Methods

```csharp
public async Task<int> ShowAsync()
```
Displays the dialog and returns the selected value.

```csharp
public void UpdateItems(List<ActionItem> items)
```
Updates the list items dynamically.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `DescriptionMaxLines` | `int` | Gets or sets max description lines |
| `DescriptionLineBreakMode` | `LineBreakMode` | Gets or sets description overflow mode |

#### Examples

```csharp
// Basic action list
var actions = new List<ActionItem>
{
    new ActionItem("Edit", "Modify this item", 0),
    new ActionItem("Share", "Share with others", 1),
    new ActionItem("Delete", "Remove permanently", 2)
};

int result = await ActionListDialog.ShowAsync("Choose Action", actions);

switch (result)
{
    case 0: await EditItem(); break;
    case 1: await ShareItem(); break;
    case 2: await DeleteItem(); break;
    case -1: /* Cancelled */ break;
}

// With icons
var iconActions = new List<ActionItem>
{
    new ActionItem("Camera", "Take a photo", 0, "camera_dark.png", "camera_light.png"),
    new ActionItem("Gallery", "Choose from gallery", 1, "gallery_dark.png", "gallery_light.png")
};

// Hierarchical menu
var menu = new List<ActionItem>
{
    new ActionItem("File", "File operations", 0)
    {
        SubItems = new List<ActionItem>
        {
            new ActionItem("New", "Create new", 100),
            new ActionItem("Open", "Open existing", 101),
            new ActionItem("Save", "Save current", 102)
        }
    },
    new ActionItem("Edit", "Edit operations", 1)
    {
        SubItems = new List<ActionItem>
        {
            new ActionItem("Cut", "Cut selection", 200),
            new ActionItem("Copy", "Copy selection", 201),
            new ActionItem("Paste", "Paste clipboard", 202)
        }
    },
    new ActionItem("Settings", "Open settings", 2)
};

int selected = await ActionListDialog.ShowAsync("Main Menu", menu);

// Multi-line descriptions
var features = new List<ActionItem>
{
    new ActionItem("Premium",
        "Unlock all features including cloud sync, unlimited storage, and priority support.",
        0),
    new ActionItem("Basic",
        "Essential features for everyday use with limited storage.",
        1)
};

var dialog = new ActionListDialog(
    "Choose Plan",
    features,
    "Cancel",
    customHeight: 350,
    descriptionMaxLines: 3,
    descriptionLineBreakMode: LineBreakMode.WordWrap);

int choice = await dialog.ShowAsync();
```

---

### ColorPickerDialog

Displays a color picker with RGB sliders, hex input, and preset colors.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `BaseDialog` → `PopupPage`

#### Constructors

```csharp
public ColorPickerDialog(
    string title,
    string? description = null,
    Color? initialColor = null,
    string? okText = null,
    string? cancelText = null,
    bool showAlpha = false,
    bool showPresets = true)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `title` | `string` | The dialog title |
| `description` | `string?` | Optional description text |
| `initialColor` | `Color?` | Initially selected color |
| `okText` | `string?` | Custom OK button text |
| `cancelText` | `string?` | Custom cancel button text |
| `showAlpha` | `bool` | Show alpha/transparency slider |
| `showPresets` | `bool` | Show preset color grid |

#### Static Methods

```csharp
public static Task<Color?> ShowAsync(
    string title,
    string? description = null,
    Color? initialColor = null)
```
Shows color picker. Returns selected `Color` or `null` if cancelled.

```csharp
public static Task<Color?> ShowAsync(
    string title,
    string? description,
    Color? initialColor,
    string? okText,
    string? cancelText,
    bool showAlpha = false,
    bool showPresets = true)
```
Shows color picker with full customization.

```csharp
public static Task HideAsync()
```
Hides the currently displayed color picker.

#### Instance Methods

```csharp
public async Task<Color?> ShowAsync()
```
Displays the dialog and returns the selected color.

```csharp
public string GetHexColor()
```
Returns the current color as a hex string (#RRGGBB or #AARRGGBB).

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `SelectedColor` | `Color` | Gets or sets the currently selected color |

#### Preset Colors

The dialog includes 20 preset colors:
Red, Pink, Purple, Deep Purple, Indigo, Blue, Light Blue, Cyan, Teal, Green, Light Green, Lime, Yellow, Amber, Orange, Deep Orange, Brown, Grey, Blue Grey, Black

#### Examples

```csharp
// Basic color picker
Color? color = await ColorPickerDialog.ShowAsync(
    "Choose Color",
    "Select your preferred theme color",
    Colors.Blue);

if (color != null)
{
    ApplyThemeColor(color.Value);
}

// With alpha channel
Color? bgColor = await ColorPickerDialog.ShowAsync(
    "Background Color",
    "Choose color with transparency",
    Colors.White.WithAlpha(0.8f),
    "Apply",
    "Cancel",
    showAlpha: true,
    showPresets: true);

// Get hex value
var dialog = new ColorPickerDialog(
    "Brand Color",
    null,
    Color.FromRgb(255, 87, 34));

Color? result = await dialog.ShowAsync();
if (result != null)
{
    string hex = dialog.GetHexColor(); // e.g., "#FF5722"
    SaveBrandColor(hex);
}

// Without presets (custom colors only)
Color? customColor = await ColorPickerDialog.ShowAsync(
    "Custom Color",
    "Use sliders to create your color",
    Colors.Gray,
    "Select",
    "Cancel",
    showAlpha: false,
    showPresets: false);
```

---

## Notifications

### Toast

Displays lightweight, non-interactive toast notifications.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `PopupPage`

#### Static Configuration Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DefaultStackBehavior` | `ToastStackBehavior` | `Stack` | How multiple toasts are handled |
| `MaxVisibleToasts` | `int` | `3` | Maximum visible toasts when stacking |
| `BackgroundColor` | `Color` | `#1F1F1F` | Toast background color |
| `TextColor` | `Color` | `White` | Message text color |
| `FontSize` | `double` | `14` | Message font size |
| `CornerRadius` | `double` | `16` | Border corner radius |
| `PaddingHorizontal` | `double` | `14` | Horizontal padding inside toast |
| `PaddingVertical` | `double` | `8` | Vertical padding inside toast |
| `IconSize` | `double` | `20` | Icon dimensions |
| `MaxWidth` | `double` | `350` | Maximum toast width |
| `ScreenEdgeMargin` | `double` | `50` | Distance from screen edge |
| `StackSpacing` | `double` | `52` | Space between stacked toasts |

#### Static Methods

```csharp
public static Task ShowAsync(string message)
```
Shows a simple toast message.

```csharp
public static Task ShowAsync(string message, DialogType iconType)
```
Shows a toast with an icon.

```csharp
public static Task ShowAsync(
    string message,
    DialogType iconType,
    ToastDuration duration)
```
Shows a toast with icon and duration.

```csharp
public static Task ShowAsync(
    string message,
    DialogType iconType,
    ToastDuration duration,
    ToastPosition position)
```
Shows a toast with vertical position (horizontally centered).

```csharp
public static Task ShowAsync(
    string message,
    DialogType iconType,
    ToastDuration duration,
    ToastPosition position,
    ToastHorizontalPosition horizontalPosition)
```
Shows a toast with full position customization (corners, edges, center).

```csharp
public static Task ShowAsync(
    string message,
    DialogType iconType,
    int durationMs,
    ToastPosition position)
```
Shows a toast with custom duration in milliseconds (horizontally centered).

```csharp
public static Task ShowAsync(
    string message,
    DialogType iconType,
    int durationMs,
    ToastPosition position,
    ToastHorizontalPosition horizontalPosition)
```
Shows a toast with custom duration and full position customization.

```csharp
public static Task DismissAllAsync()
```
Dismisses all active toasts.

```csharp
public static void ResetConfiguration()
```
Resets all configuration properties to defaults.

#### Instance Methods

```csharp
public Task DismissAsync()
```
Dismisses this specific toast.

#### Examples

```csharp
// Simple toast
await Toast.ShowAsync("Message sent");

// With icon
await Toast.ShowAsync("Saved successfully", DialogType.Success);

// Long duration at top
await Toast.ShowAsync(
    "Processing complete",
    DialogType.Info,
    ToastDuration.Long,
    ToastPosition.Top);

// Custom duration (5 seconds)
await Toast.ShowAsync(
    "Custom timing",
    DialogType.None,
    5000,
    ToastPosition.Bottom);

// Corner positioning - bottom right
await Toast.ShowAsync(
    "Downloaded!",
    DialogType.Success,
    ToastDuration.Short,
    ToastPosition.Bottom,
    ToastHorizontalPosition.Right);

// Corner positioning - top left
await Toast.ShowAsync(
    "New message",
    DialogType.Info,
    ToastDuration.Short,
    ToastPosition.Top,
    ToastHorizontalPosition.Left);

// Bottom left corner
await Toast.ShowAsync(
    "File saved",
    DialogType.Success,
    ToastDuration.Long,
    ToastPosition.Bottom,
    ToastHorizontalPosition.Left);

// Top right corner
await Toast.ShowAsync(
    "Update available",
    DialogType.Warning,
    ToastDuration.Long,
    ToastPosition.Top,
    ToastHorizontalPosition.Right);

// Configure appearance
Toast.BackgroundColor = Color.FromRgba("#2196F3");
Toast.TextColor = Colors.White;
Toast.CornerRadius = 8;
Toast.PaddingVertical = 12;

// Configure stacking
Toast.DefaultStackBehavior = ToastStackBehavior.Replace;
Toast.MaxVisibleToasts = 5;

// Reset to defaults
Toast.ResetConfiguration();

// Dismiss all
await Toast.DismissAllAsync();
```

---

### Snackbar

Displays actionable snackbar notifications with optional buttons.

**Namespace:** `MarketAlly.Dialogs.Maui.Dialogs`

**Inheritance:** `PopupPage`

#### Static Configuration Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DefaultStackBehavior` | `SnackbarStackBehavior` | `Stack` | How multiple snackbars are handled |
| `MaxVisibleSnackbars` | `int` | `3` | Maximum visible snackbars when stacking |
| `BackgroundColor` | `Color` | `#1F1F1F` | Snackbar background color |
| `TextColor` | `Color` | `White` | Message text color |
| `ActionTextColor` | `Color` | `#BB86FC` | Action button text color |
| `FontSize` | `double` | `14` | Message and button font size |
| `CornerRadius` | `double` | `6` | Border corner radius |
| `PaddingHorizontal` | `double` | `14` | Horizontal padding |
| `PaddingVertical` | `double` | `10` | Vertical padding |
| `IconSize` | `double` | `20` | Icon dimensions |
| `ScreenEdgeMargin` | `double` | `80` | Distance from screen edge |
| `SideMargin` | `double` | `16` | Horizontal margin from sides |
| `StackSpacing` | `double` | `55` | Space between stacked snackbars |

#### Static Methods

```csharp
public static Task<SnackbarResult> ShowAsync(string message)
```
Shows a simple snackbar message.

```csharp
public static Task<SnackbarResult> ShowAsync(string message, string actionText)
```
Shows a snackbar with an action button.

```csharp
public static Task<SnackbarResult> ShowAsync(
    string message,
    string actionText,
    Action actionCallback)
```
Shows a snackbar with action button and callback.

```csharp
public static Task<SnackbarResult> ShowAsync(
    string message,
    string? actionText,
    Action? actionCallback,
    SnackbarDuration duration)
```
Shows a snackbar with custom duration.

```csharp
public static Task<SnackbarResult> ShowAsync(
    string message,
    string? actionText,
    Action? actionCallback,
    DialogType iconType,
    SnackbarDuration duration,
    ToastPosition position)
```
Shows a snackbar with full customization using enum duration.

```csharp
public static Task<SnackbarResult> ShowAsync(
    string message,
    string? actionText,
    Action? actionCallback,
    DialogType iconType,
    int? durationMs,
    ToastPosition position)
```
Shows a snackbar with custom duration in milliseconds (`null` for indefinite).

```csharp
public static Task DismissAllAsync()
```
Dismisses all active snackbars.

```csharp
public static void ResetConfiguration()
```
Resets all configuration properties to defaults.

#### Instance Methods

```csharp
public Task DismissAsync(SnackbarResult result)
```
Dismisses this snackbar with a specific result.

#### Examples

```csharp
// Simple snackbar
var result = await Snackbar.ShowAsync("File saved");

// With action button
var result = await Snackbar.ShowAsync("Item deleted", "UNDO");
if (result == SnackbarResult.ActionClicked)
{
    await RestoreItem();
}

// With callback
await Snackbar.ShowAsync(
    "Message archived",
    "UNDO",
    () => UnarchiveMessage());

// Full customization
var result = await Snackbar.ShowAsync(
    "Connection lost",
    "RETRY",
    () => Reconnect(),
    DialogType.Error,
    SnackbarDuration.Long,
    ToastPosition.Bottom);

// Indefinite (stays until interaction)
var result = await Snackbar.ShowAsync(
    "No internet connection",
    "RETRY",
    null,
    DialogType.Warning,
    SnackbarDuration.Indefinite,
    ToastPosition.Bottom);

// Configure appearance
Snackbar.BackgroundColor = Color.FromRgba("#424242");
Snackbar.ActionTextColor = Colors.Cyan;
Snackbar.CornerRadius = 4;

// Configure stacking
Snackbar.DefaultStackBehavior = SnackbarStackBehavior.Replace;

// Reset to defaults
Snackbar.ResetConfiguration();
```

---

## Core Services

### DialogService

Singleton service for managing themes, localization, and global dialog settings.

**Namespace:** `MarketAlly.Dialogs.Maui.Core`

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Instance` | `DialogService` | Singleton instance (read-only) |
| `LightTheme` | `DialogTheme` | Light theme configuration |
| `DarkTheme` | `DialogTheme` | Dark theme configuration |
| `CurrentTheme` | `DialogTheme` | Currently active theme (computed) |
| `CurrentThemeOverride` | `DialogTheme?` | Manual theme override (null = auto) |
| `UseSystemTheme` | `bool` | Whether to follow system theme (default: true) |
| `Localization` | `IDialogLocalization` | Current localization provider |
| `CustomIcons` | `Dictionary<DialogType, DialogIconMapping>` | Custom icon mappings |

#### Methods

```csharp
public void Initialize(
    DialogTheme? lightTheme = null,
    DialogTheme? darkTheme = null)
```
Initializes the service with optional custom themes.

```csharp
public void SetLocalization(IDialogLocalization localization)
```
Sets the localization provider.

```csharp
public void SetOverlayEnabled(bool showOverlay)
```
Enables or disables the background overlay for all dialogs.

```csharp
public void SetOverlayColor(Color color)
```
Sets the overlay background color.

```csharp
public void RegisterCustomIcon(
    DialogType dialogType,
    string lightIcon,
    string darkIcon)
```
Registers custom icons for a dialog type.

```csharp
public string? GetDialogIcon(DialogType dialogType, bool isDarkTheme)
```
Gets the icon path for a dialog type and theme.

```csharp
public ResourceDictionary CreateThemedStyles()
```
Creates a resource dictionary with current theme styles.

```csharp
public void Reset()
```
Resets all settings to defaults.

#### Examples

```csharp
// Initialize with custom themes
var lightTheme = new DialogTheme
{
    BackgroundColor = Colors.White,
    TitleTextColor = Colors.Black,
    ButtonBackgroundColor = Color.FromRgba("#2196F3")
};

var darkTheme = new DialogTheme
{
    BackgroundColor = Color.FromRgba("#1E1E1E"),
    TitleTextColor = Colors.White,
    ButtonBackgroundColor = Color.FromRgba("#BB86FC")
};

DialogService.Instance.Initialize(lightTheme, darkTheme);

// Force dark theme
DialogService.Instance.CurrentThemeOverride = DialogService.Instance.DarkTheme;

// Return to system theme
DialogService.Instance.CurrentThemeOverride = null;

// Custom localization
DialogService.Instance.SetLocalization(new SpanishLocalization());

// Custom overlay
DialogService.Instance.SetOverlayEnabled(true);
DialogService.Instance.SetOverlayColor(Color.FromRgba("#CC000000"));

// Custom icons
DialogService.Instance.RegisterCustomIcon(
    DialogType.Custom,
    "custom_light.png",
    "custom_dark.png");
```

---

### BaseDialog

Abstract base class for all dialogs providing theming and helper methods.

**Namespace:** `MarketAlly.Dialogs.Maui.Core`

**Inheritance:** `PopupPage`

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `CustomLightIcon` | `string?` | Custom icon for light theme |
| `CustomDarkIcon` | `string?` | Custom icon for dark theme |
| `DescriptionPadding` | `Thickness` | Padding around description (default: 10,5) |

#### Protected Properties

| Property | Type | Description |
|----------|------|-------------|
| `DialogService` | `DialogService` | Reference to DialogService singleton |
| `CurrentTheme` | `DialogTheme` | Currently active theme |
| `DialogType` | `DialogType` | Type of dialog being displayed |

#### Protected Methods

```csharp
protected Border CreateThemedFrame(View content)
```
Creates a themed container frame for dialog content.

```csharp
protected Label CreateTitleLabel(string text)
```
Creates a styled title label.

```csharp
protected Label CreateDescriptionLabel(string text)
```
Creates a styled description label.

```csharp
protected Button CreatePrimaryButton(string text, EventHandler clicked)
```
Creates a primary (accent colored) button.

```csharp
protected Button CreateSecondaryButton(string text, EventHandler clicked)
```
Creates a secondary (subtle) button.

```csharp
protected BoxView CreateSeparator()
```
Creates a horizontal separator line.

```csharp
protected virtual void ApplyTheme()
```
Applies the current theme. Override for custom theming.

```csharp
protected virtual void OnThemeApplied()
```
Called after theme is applied. Override for custom handling.

```csharp
protected virtual bool HandleBackButton()
```
Handles back button press. Override for custom behavior.

#### Creating Custom Dialogs

```csharp
public class RatingDialog : BaseDialog
{
    private readonly TaskCompletionSource<int> _tcs = new();
    private int _rating;

    public RatingDialog(string title)
    {
        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            },
            Padding = 20
        };

        // Title
        grid.Add(CreateTitleLabel(title), 0, 0);

        // Star rating
        var stars = new HorizontalStackLayout { Spacing = 10 };
        for (int i = 1; i <= 5; i++)
        {
            int rating = i;
            var star = new Label { Text = "☆", FontSize = 30 };
            star.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SetRating(rating))
            });
            stars.Children.Add(star);
        }
        grid.Add(stars, 0, 1);

        // Buttons
        var buttons = new HorizontalStackLayout { Spacing = 10 };
        buttons.Children.Add(CreateSecondaryButton("Cancel", OnCancel));
        buttons.Children.Add(CreatePrimaryButton("Submit", OnSubmit));
        grid.Add(buttons, 0, 2);

        Content = CreateThemedFrame(grid);
    }

    private void SetRating(int rating) => _rating = rating;

    private async void OnSubmit(object s, EventArgs e)
    {
        _tcs.TrySetResult(_rating);
        await MopupService.Instance.PopAsync();
    }

    private async void OnCancel(object s, EventArgs e)
    {
        _tcs.TrySetResult(0);
        await MopupService.Instance.PopAsync();
    }

    public async Task<int> ShowAsync()
    {
        await MopupService.Instance.PushAsync(this);
        return await _tcs.Task;
    }
}
```

---

## Models

### DialogTheme

Complete theme configuration for dialogs.

**Namespace:** `MarketAlly.Dialogs.Maui.Models`

#### Properties

##### Colors

| Property | Type | Default (Light) | Description |
|----------|------|-----------------|-------------|
| `BackgroundColor` | `Color` | `#FFFFFF` | Dialog background |
| `OverlayColor` | `Color` | `#80000000` | Semi-transparent overlay |
| `BorderColor` | `Color` | `#E0E0E0` | Dialog border color |
| `ShowOverlay` | `bool` | `true` | Whether to show overlay |

##### Text Colors

| Property | Type | Default (Light) | Description |
|----------|------|-----------------|-------------|
| `TitleTextColor` | `Color` | `#212121` | Title text color |
| `DescriptionTextColor` | `Color` | `#757575` | Description text color |
| `ButtonTextColor` | `Color` | `#FFFFFF` | Primary button text |
| `SecondaryButtonTextColor` | `Color` | `#212121` | Secondary button text |

##### Button Colors

| Property | Type | Default (Light) | Description |
|----------|------|-----------------|-------------|
| `ButtonBackgroundColor` | `Color` | `#2196F3` | Primary button background |
| `ButtonBorderColor` | `Color` | `#2196F3` | Primary button border |
| `SecondaryButtonBackgroundColor` | `Color` | `#F5F5F5` | Secondary button background |
| `SecondaryButtonBorderColor` | `Color` | `#E0E0E0` | Secondary button border |

##### Typography

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `TitleFontFamily` | `string` | `null` | Title font family |
| `TitleFontSize` | `double` | `16` | Title font size |
| `TitleFontAttributes` | `FontAttributes` | `Bold` | Title font style |
| `TitleMaxLines` | `int` | `2` | Maximum title lines |
| `TitleLineBreakMode` | `LineBreakMode` | `TailTruncation` | Title overflow handling |
| `DescriptionFontFamily` | `string` | `null` | Description font family |
| `DescriptionFontSize` | `double` | `14` | Description font size |
| `DescriptionTextType` | `TextType` | `Text` | Text or HTML rendering |

##### Dimensions

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DialogWidth` | `double` | `300` | Dialog width |
| `DialogHeight` | `double` | `250` | Dialog height |
| `DialogCornerRadius` | `double` | `8` | Corner radius |
| `DialogPadding` | `double` | `20` | Internal padding |
| `ButtonHeight` | `double` | `44` | Button height |

##### Animation

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `AnimationDuration` | `uint` | `250` | Animation duration (ms) |
| `EnableAnimation` | `bool` | `true` | Enable/disable animations |

##### Visual Effects

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `HasShadow` | `bool` | `true` | Enable drop shadow |
| `IsDarkMode` | `bool` | `false` | Dark mode flag |

#### Static Properties

```csharp
public static DialogTheme LightTheme { get; }
```
Pre-configured light theme.

```csharp
public static DialogTheme DarkTheme { get; }
```
Pre-configured dark theme.

#### Methods

```csharp
public DialogTheme Clone()
```
Creates a deep copy of the theme.

#### Examples

```csharp
// Create custom theme based on light
var customTheme = DialogTheme.LightTheme.Clone();
customTheme.ButtonBackgroundColor = Color.FromRgba("#FF5722");
customTheme.DialogCornerRadius = 16;
customTheme.TitleFontSize = 20;

// Apply custom theme
DialogService.Instance.Initialize(customTheme, DialogTheme.DarkTheme);

// HTML descriptions
var htmlTheme = new DialogTheme
{
    DescriptionTextType = TextType.Html,
    // ... other properties
};

// Use with HTML content
await AlertDialog.ShowAsync(
    "Welcome",
    "This is <b>bold</b> and <i>italic</i> text.",
    DialogType.Info);
```

---

### DialogType

Enumeration of available dialog icon types.

**Namespace:** `MarketAlly.Dialogs.Maui.Models`

```csharp
public enum DialogType
{
    None,      // No icon
    Info,      // Information icon (blue circle with 'i')
    Success,   // Success icon (green checkmark)
    Warning,   // Warning icon (orange/yellow triangle)
    Error,     // Error icon (red circle with 'x')
    Help,      // Help icon (question mark)
    Decide,    // Decision icon (fork/branch)
    Stop,      // Stop icon (hand)
    Custom     // Custom user-defined icon
}
```

#### Icon Files

Each type has light and dark variants:
- `info_black_48dp.png` / `info_white_48dp.png`
- `task_alt_black_48dp.png` / `task_alt_white_48dp.png` (Success)
- `warning_amber_black_48dp.png` / `warning_amber_white_48dp.png`
- `error_outline_black_48dp.png` / `error_outline_white_48dp.png`
- `help_outline_black_48dp.png` / `help_outline_white_48dp.png`
- `fork_right_black_48dp.png` / `fork_right_white_48dp.png` (Decide)
- `pan_tool_black_48dp.png` / `pan_tool_white_48dp.png` (Stop)

---

### ActionItem

Represents a selectable item in ActionListDialog.

**Namespace:** `MarketAlly.Dialogs.Maui.Models`

#### Constructors

```csharp
public ActionItem()
```
Default constructor.

```csharp
public ActionItem(string name, int value, Guid? itemId = null)
```
Creates item with name and value.

```csharp
public ActionItem(string name, string? detail, int value, Guid? itemId = null)
```
Creates item with name, detail, and value.

```csharp
public ActionItem(
    string name,
    string? detail,
    int value,
    string? imageDark,
    string? imageLight,
    Guid? itemId = null)
```
Creates item with full customization.

```csharp
public ActionItem(string name, Action action, string? detail = null)
```
Creates item with synchronous action callback.

```csharp
public ActionItem(string name, Func<Task> asyncAction, string? detail = null)
```
Creates item with async action callback.

```csharp
public ActionItem(string name, Action action, string? detail, string? imageDark, string? imageLight)
```
Creates item with synchronous action callback and icons.

```csharp
public ActionItem(string name, Func<Task> asyncAction, string? detail, string? imageDark, string? imageLight)
```
Creates item with async action callback and icons.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Display name (required) |
| `Detail` | `string?` | Optional description text |
| `Value` | `int` | Return value when selected |
| `Action` | `Action?` | Synchronous callback invoked on selection |
| `AsyncAction` | `Func<Task>?` | Async callback invoked on selection (takes precedence) |
| `ImageDark` | `string?` | Icon path for dark theme |
| `ImageLight` | `string?` | Icon path for light theme |
| `ItemId` | `Guid` | Unique identifier |
| `SubItems` | `List<ActionItem>?` | Child items for hierarchical menus |

#### Computed Properties

| Property | Type | Description |
|----------|------|-------------|
| `ShowImage` | `bool` | `true` if any image is set |
| `HasDetail` | `bool` | `true` if Detail is not empty |
| `HasSubItems` | `bool` | `true` if SubItems has items |
| `HasAction` | `bool` | `true` if Action or AsyncAction is set |

#### Methods

```csharp
public async Task InvokeActionAsync()
```
Invokes the action associated with this item. AsyncAction takes precedence if both are set.

#### Examples

```csharp
// Simple item
var item = new ActionItem("Edit", 1);

// With description
var item = new ActionItem("Delete", "Remove this item permanently", 2);

// With icons
var item = new ActionItem(
    "Share",
    "Share with others",
    3,
    "share_dark.png",
    "share_light.png");

// With synchronous action callback
var syncItem = new ActionItem("Show Alert", () =>
{
    Console.WriteLine("Alert shown!");
}, "Displays an alert message");

// With async action callback
var asyncItem = new ActionItem("Load Data", async () =>
{
    await LoadDataAsync();
}, "Loads data from server");

// With action and icons
var actionWithIcons = new ActionItem("Save", async () =>
{
    await SaveFileAsync();
}, "Save to cloud", "save_dark.png", "save_light.png");

// Hierarchical menu
var fileMenu = new ActionItem("File", "File operations", 0)
{
    SubItems = new List<ActionItem>
    {
        new ActionItem("New", "Create new file", 100),
        new ActionItem("Open", "Open existing", 101),
        new ActionItem("Save", "Save changes", 102),
        new ActionItem("Export", "Export options", 103)
        {
            SubItems = new List<ActionItem>
            {
                new ActionItem("PDF", "Export as PDF", 1000),
                new ActionItem("Word", "Export as Word", 1001)
            }
        }
    }
};

// Using actions with ActionListDialog
var actions = new List<ActionItem>
{
    new ActionItem("Edit", () => EditItem(), "Modify this item"),
    new ActionItem("Delete", async () => await DeleteAsync(), "Remove permanently")
};

bool wasSelected = await ActionListDialog.ShowWithActionsAsync("Actions", actions);
// Actions are automatically invoked when selected!
```

---

## Enumerations

### ToastPosition

```csharp
public enum ToastPosition
{
    Bottom,  // Bottom of screen (default)
    Top      // Top of screen
}
```

### ToastHorizontalPosition

```csharp
public enum ToastHorizontalPosition
{
    Left,    // Left side of screen
    Center,  // Center of screen (default)
    Right    // Right side of screen
}
```

Combine with `ToastPosition` for corner positioning:
- Bottom + Left = Bottom-left corner
- Bottom + Center = Bottom center (default)
- Bottom + Right = Bottom-right corner
- Top + Left = Top-left corner
- Top + Center = Top center
- Top + Right = Top-right corner

### ToastDuration

```csharp
public enum ToastDuration
{
    Short,  // 2 seconds
    Long    // 3.5 seconds
}
```

### ToastStackBehavior

```csharp
public enum ToastStackBehavior
{
    Replace,  // New toast replaces existing
    Queue,    // Toasts shown one at a time
    Stack     // Multiple toasts stacked (default)
}
```

### SnackbarDuration

```csharp
public enum SnackbarDuration
{
    Short,      // 4 seconds
    Long,       // 7 seconds
    Indefinite  // Until user interaction
}
```

### SnackbarResult

```csharp
public enum SnackbarResult
{
    Dismissed,      // User dismissed (swipe, tap, back)
    ActionClicked,  // User clicked action button
    TimedOut        // Auto-dismissed after duration
}
```

### SnackbarStackBehavior

```csharp
public enum SnackbarStackBehavior
{
    Replace,  // New snackbar replaces existing
    Queue,    // Snackbars shown one at a time
    Stack     // Multiple snackbars stacked (default)
}
```

---

## Interfaces

### IDialogLocalization

Interface for providing localized strings.

**Namespace:** `MarketAlly.Dialogs.Maui.Interfaces`

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `OkButtonText` | `string` | OK button text |
| `CancelButtonText` | `string` | Cancel button text |
| `YesButtonText` | `string` | Yes button text |
| `NoButtonText` | `string` | No button text |
| `LoadingText` | `string` | Default loading text |
| `SelectPlaceholder` | `string` | Select placeholder text |
| `HexLabel` | `string` | "Hex:" label |
| `RedLabel` | `string` | "Red" label |
| `GreenLabel` | `string` | "Green" label |
| `BlueLabel` | `string` | "Blue" label |
| `AlphaLabel` | `string` | "Alpha" label |
| `PresetColorsLabel` | `string` | "Preset Colors" label |
| `ItemsScrollIndicator` | `string` | Scroll indicator format |
| `DismissText` | `string` | "DISMISS" action text |
| `UndoText` | `string` | "UNDO" action text |
| `RetryText` | `string` | "RETRY" action text |

#### Methods

```csharp
string GetString(string key)
```
Gets localized string by key.

```csharp
string GetString(string key, params object[] args)
```
Gets formatted localized string.

#### Implementation Example

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
    public string ItemsScrollIndicator => "{0}件 (スクロールで表示)";
    public string DismissText => "閉じる";
    public string UndoText => "元に戻す";
    public string RetryText => "再試行";

    public string GetString(string key) => key;

    public string GetString(string key, params object[] args)
        => string.Format(GetString(key), args);
}

// Usage
DialogService.Instance.SetLocalization(new JapaneseLocalization());
```

---

## Localization

### DefaultDialogLocalization

Built-in localization with support for English, Spanish, French, and German.

**Namespace:** `MarketAlly.Dialogs.Maui.Localization`

**Implements:** `IDialogLocalization`

#### Constructors

```csharp
public DefaultDialogLocalization()
```
Uses `CultureInfo.CurrentCulture` for language detection.

```csharp
public DefaultDialogLocalization(CultureInfo culture)
```
Uses specified culture.

#### Supported Languages

| Language | Culture Code | Example |
|----------|--------------|---------|
| English | `en`, `en-US`, etc. | "OK", "Cancel", "Yes", "No" |
| Spanish | `es`, `es-ES`, etc. | "OK", "Cancelar", "Sí", "No" |
| French | `fr`, `fr-FR`, etc. | "OK", "Annuler", "Oui", "Non" |
| German | `de`, `de-DE`, etc. | "OK", "Abbrechen", "Ja", "Nein" |

#### Examples

```csharp
// Auto-detect from system
var localization = new DefaultDialogLocalization();

// Force Spanish
var spanish = new DefaultDialogLocalization(new CultureInfo("es-ES"));
DialogService.Instance.SetLocalization(spanish);

// Force French
var french = new DefaultDialogLocalization(new CultureInfo("fr"));
DialogService.Instance.SetLocalization(french);
```

---

## Version History

| Version | Changes |
|---------|---------|
| 1.4.1 | Added ActionItem Action/AsyncAction callbacks, ShowWithActionsAsync, Toast horizontal positioning |
| 1.4.0 | Added Toast and Snackbar notifications |
| 1.3.0 | Added hierarchical menus, fixed duplicate key bug |
| 1.2.0 | Added TitleMaxLines, TitleLineBreakMode, HTML descriptions |
| 1.1.0 | Added multi-line descriptions, configurable line break modes |
| 1.0.0 | Initial release |

---

*Generated for MarketAlly.Dialogs.Maui v1.4.1*
