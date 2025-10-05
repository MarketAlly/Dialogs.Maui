using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Models;
using MarketAlly.Dialogs.Maui.Dialogs;
using Mopups.Services;

namespace Test.Pages;

public partial class DialogDemoPage : ContentPage
{
    private readonly DialogService _dialogService;
    private bool _isDarkTheme = false;
    private LoadingDialog? _currentLoadingDialog;

    public DialogDemoPage()
    {
        InitializeComponent();
        _dialogService = DialogService.Instance;

        // Initialize the dialog service if needed
        _dialogService.Initialize();
    }

    private async void OnSuccessAlertClicked(object sender, EventArgs e)
    {
        var dialog = new AlertDialog(
            "Success!",
            "Operation completed successfully.",
            "OK",
            DialogType.Success);

        await MopupService.Instance.PushAsync(dialog);
        ResultLabel.Text = "Success alert shown";
    }

    private async void OnErrorAlertClicked(object sender, EventArgs e)
    {
        var dialog = new AlertDialog(
            "Error",
            "An error occurred while processing your request.",
            "Close",
            DialogType.Error);

        await MopupService.Instance.PushAsync(dialog);
        ResultLabel.Text = "Error alert shown";
    }

    private async void OnWarningAlertClicked(object sender, EventArgs e)
    {
        var dialog = new AlertDialog(
            "Warning",
            "This action may have unintended consequences.",
            "I Understand",
            DialogType.Warning);

        await MopupService.Instance.PushAsync(dialog);
        ResultLabel.Text = "Warning alert shown";
    }

    private async void OnInfoAlertClicked(object sender, EventArgs e)
    {
        var dialog = new AlertDialog(
            "Information",
            "This is some important information for you to know.",
            "Got It",
            DialogType.Info);

        await MopupService.Instance.PushAsync(dialog);
        ResultLabel.Text = "Info alert shown";
    }

    private async void OnYesNoConfirmationClicked(object sender, EventArgs e)
    {
        var dialog = new ConfirmDialog(
            "Confirm Action",
            "Are you sure you want to proceed with this action?",
            "Yes",
            "No",
            DialogType.Help);

        var result = await dialog.ShowAsync();

        ResultLabel.Text = $"Confirmation result: {(result ? "Yes" : "No")}";
    }

    private async void OnDeleteConfirmationClicked(object sender, EventArgs e)
    {
        var dialog = new ConfirmDialog(
            "Delete Item",
            "This will permanently delete the selected item. This action cannot be undone.",
            "Delete",
            "Cancel",
            DialogType.Warning);

        var result = await dialog.ShowAsync();

        ResultLabel.Text = $"Delete confirmation result: {(result ? "Deleted" : "Cancelled")}";
    }

    private async void OnTextPromptClicked(object sender, EventArgs e)
    {
        var result = await PromptDialog.ShowAsync(
            "Enter Name",
            "Your name here",
            "OK",
            "Cancel",
            DialogType.None);

        ResultLabel.Text = string.IsNullOrEmpty(result)
            ? "Text prompt cancelled"
            : $"You entered: {result}";
    }

    private async void OnPasswordPromptClicked(object sender, EventArgs e)
    {
        // Create a password prompt with masked input
        var dialog = new PromptDialog(
            "Enter Password",
            "Enter your password",
            "Login",
            "Cancel",
            DialogType.None,
            Keyboard.Text,
            isPassword: true);

        await MopupService.Instance.PushAsync(dialog);
        await Task.Delay(100); // Small delay to ensure dialog is displayed

        // Wait for user input
        var tcs = new TaskCompletionSource<string?>();
        dialog.Disappearing += (s, args) =>
        {
            tcs.TrySetResult(dialog.Text);
        };

        var result = await tcs.Task;

        ResultLabel.Text = string.IsNullOrEmpty(result)
            ? "Password prompt cancelled"
            : $"Password entered (length: {result?.Length ?? 0} characters)";
    }

    private async void OnNotesEditorClicked(object sender, EventArgs e)
    {
        var dialog = new EditorDialog(
            "Add Notes",
            "Enter your notes below:",
            "Type your notes here...",
            "Save",
            "Cancel",
            DialogType.None,
            Keyboard.Text,
            minLines: 3,
            maxLines: 8);

        var result = await dialog.ShowAsync();

        ResultLabel.Text = string.IsNullOrEmpty(result)
            ? "Notes editor cancelled"
            : $"Notes saved ({result.Length} characters)";
    }

    private async void OnFeedbackEditorClicked(object sender, EventArgs e)
    {
        var dialog = new EditorDialog(
            "Send Feedback",
            "We'd love to hear your thoughts! Please provide detailed feedback below.",
            "Your feedback helps us improve...",
            "Send",
            "Cancel",
            DialogType.Help,
            Keyboard.Text,
            minLines: 5,
            maxLines: 10);

        dialog.IsSpellCheckEnabled = true;
        dialog.IsTextPredictionEnabled = true;

        var result = await dialog.ShowAsync();

        ResultLabel.Text = string.IsNullOrEmpty(result)
            ? "Feedback cancelled"
            : $"Feedback submitted ({result.Split('\n').Length} lines, {result.Length} characters)";
    }

    private async void OnActionListClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Share", "Share this item with others", 0),
            new ActionItem("Edit", "Modify the selected item", 1),
            new ActionItem("Delete", "Remove this item permanently", 2),
            new ActionItem("Archive", "Move to archive folder", 3),
            new ActionItem("Export", "Export as PDF or image", 4),
            new ActionItem("Copy", "Create a duplicate", 5),
            new ActionItem("Move", "Move to another location", 6),
            new ActionItem("Rename", "Change the name", 7),
            new ActionItem("Properties", "View item properties", 8),
            new ActionItem("Permissions", "Manage access permissions", 9)
        };

        var dialog = new ActionListDialog(
            "Choose an Action",
            actions,
            "Cancel");

        var result = await dialog.ShowAsync();

        if (result >= 0 && result < actions.Count)
        {
            ResultLabel.Text = $"Action selected: {actions[result].Name}";
        }
        else
        {
            ResultLabel.Text = "Action list cancelled";
        }
    }

    private async void OnActionListWithIconsClicked(object sender, EventArgs e)
    {
        // Use the actual icon file names from the library resources
        // ImageDark = dark icons for light theme, ImageLight = light icons for dark theme
        var actions = new List<ActionItem>
        {
            new ActionItem("Success", "Operation completed successfully", 0, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Warning", "Please review this carefully", 1, "warning_amber_black_48dp", "warning_amber_white_48dp"),
            new ActionItem("Error", "Something went wrong", 2, "error_outline_black_48dp", "error_outline_white_48dp"),
            new ActionItem("Information", "Helpful information", 3, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Help", "Get assistance", 4, "help_outline_black_48dp", "help_outline_white_48dp")
        };

        var dialog = new ActionListDialog(
            "Select Content Type",
            actions,
            "Cancel");

        var result = await dialog.ShowAsync();

        if (result >= 0 && result < actions.Count)
        {
            ResultLabel.Text = $"Content type selected: {actions[result].Name}";
        }
        else
        {
            ResultLabel.Text = "Action list with icons cancelled";
        }
    }

    private async void OnColorPickerClicked(object sender, EventArgs e)
    {
        var dialog = new ColorPickerDialog(
            "Choose a Color",
            "Select your preferred color",
            Colors.Blue,
            "Select",
            "Cancel");

        var result = await dialog.ShowAsync();

        if (result != null)
        {
            ResultLabel.Text = $"Color selected: {dialog.GetHexColor()}";
            ResultLabel.TextColor = result;
        }
        else
        {
            ResultLabel.Text = "Color picker cancelled";
            ResultLabel.TextColor = Colors.Black;
        }
    }

    private async void OnColorPickerWithAlphaClicked(object sender, EventArgs e)
    {
        var dialog = new ColorPickerDialog(
            "Choose a Color with Transparency",
            "Adjust the transparency slider to set opacity",
            Colors.Red.WithAlpha(0.5f),
            "Select",
            "Cancel",
            showAlpha: true,
            showPresets: true);

        var result = await dialog.ShowAsync();

        if (result != null)
        {
            ResultLabel.Text = $"Color selected: {dialog.GetHexColor()} (Alpha: {result.Alpha:F2})";
            ResultLabel.TextColor = result;
        }
        else
        {
            ResultLabel.Text = "Color picker with alpha cancelled";
            ResultLabel.TextColor = Colors.Black;
        }
    }

    private async void OnColorPickerNoPresetsClicked(object sender, EventArgs e)
    {
        var dialog = new ColorPickerDialog(
            "Custom Color Selection",
            "Use the RGB sliders to create a custom color",
            Colors.Green,
            "Apply",
            "Cancel",
            showAlpha: false,
            showPresets: false);

        var result = await dialog.ShowAsync();

        if (result != null)
        {
            ResultLabel.Text = $"Custom color selected: RGB({result.Red * 255:F0}, {result.Green * 255:F0}, {result.Blue * 255:F0})";
            ResultLabel.TextColor = result;
        }
        else
        {
            ResultLabel.Text = "Custom color picker cancelled";
            ResultLabel.TextColor = Colors.Black;
        }
    }

    private async void OnLoadingClicked(object sender, EventArgs e)
    {
        _currentLoadingDialog = new LoadingDialog("Processing...");
        await MopupService.Instance.PushAsync(_currentLoadingDialog);
        ResultLabel.Text = "Loading dialog shown...";

        // Simulate some work
        await Task.Delay(3000);

        if (_currentLoadingDialog != null && MopupService.Instance.PopupStack.Contains(_currentLoadingDialog))
        {
            await MopupService.Instance.RemovePageAsync(_currentLoadingDialog);
        }
        _currentLoadingDialog = null;
        ResultLabel.Text = "Loading dialog hidden after 3 seconds";
    }

    private async void OnLoadingWithCancelClicked(object sender, EventArgs e)
    {
        ResultLabel.Text = "Loading with cancel shown...";

        var wasCanceled = await LoadingDialog.ShowCancelableAsync(
            "Processing... Click Cancel to stop",
            async () =>
            {
                // Simulate some long-running work
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(500);
                }
            });

        ResultLabel.Text = wasCanceled ? "Loading was canceled by user" : "Loading completed successfully";
    }

    private void OnToggleThemeClicked(object sender, EventArgs e)
    {
        _isDarkTheme = !_isDarkTheme;

        if (_isDarkTheme)
        {
            _dialogService.CurrentThemeOverride = DialogService.Instance.DarkTheme;
            CurrentThemeLabel.Text = "Current Theme: Dark";
            ResultLabel.Text = "Switched to Dark theme";
        }
        else
        {
            _dialogService.CurrentThemeOverride = DialogService.Instance.LightTheme;
            CurrentThemeLabel.Text = "Current Theme: Light";
            ResultLabel.Text = "Switched to Light theme";
        }
    }

    private async void OnCustomDialogClicked(object sender, EventArgs e)
    {
        // Example of registering custom icons
        _dialogService.RegisterCustomIcon(DialogType.Custom, "custom_icon_light.png", "custom_icon_dark.png");

        var dialog = new AlertDialog(
            "Custom Dialog",
            "This is a custom dialog with special styling options.",
            "OK",
            DialogType.Custom);

        await MopupService.Instance.PushAsync(dialog);
        ResultLabel.Text = "Custom dialog shown";
    }

    private async void OnLongTitleAlertClicked(object sender, EventArgs e)
    {
        var dialog = new AlertDialog(
            "This is a very long title that should wrap to multiple lines to test how the dialog handles long titles that exceed the normal width",
            "Short description here.",
            "OK",
            DialogType.Info);

        await MopupService.Instance.PushAsync(dialog);
        ResultLabel.Text = "Long title alert shown";
    }

    private async void OnLongDescriptionAlertClicked(object sender, EventArgs e)
    {
        var dialog = new AlertDialog(
            "Important Notice",
            "This is a very long description that contains multiple sentences to test how the dialog handles lengthy content. " +
            "The dialog should automatically adjust its height to accommodate all the text without cutting off any content. " +
            "This helps ensure that important information is always visible to the user, regardless of how much text needs to be displayed. " +
            "The layout should be responsive and handle various screen sizes appropriately.",
            "I Understand",
            DialogType.Warning);

        // Demonstrate custom description padding
        dialog.DescriptionPadding = new Thickness(20, 10, 20, 10);

        await MopupService.Instance.PushAsync(dialog);
        ResultLabel.Text = "Long description alert shown with custom padding";
    }
}