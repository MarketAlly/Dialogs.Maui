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
            "Please enter your full name below:",
            "Your name here",
            null,
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
            "Please enter your password to continue:",
            "Enter your password",
            null,
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

    private async void OnLargeActionListClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Create New Project", "Initialize a new project workspace", 0, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Open Existing", "Open an existing project file", 1, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Save Current", "Save the current project", 2, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Save As...", "Save with a different name or location", 3, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Export to PDF", "Export the project as a PDF document", 4, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Share Online", "Share the project via cloud services", 5, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Print Preview", "Preview before printing", 6, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Settings", "Configure project settings and preferences", 7, "help_outline_black_48dp", "help_outline_white_48dp"),
            new ActionItem("Delete Project", "Permanently remove this project", 8, "error_outline_black_48dp", "error_outline_white_48dp"),
            new ActionItem("Help & Support", "View documentation and get help", 9, "help_outline_black_48dp", "help_outline_white_48dp")
        };

        var dialog = new ActionListDialog(
            "Project Actions",
            actions,
            "Cancel");

        var result = await dialog.ShowAsync();

        if (result >= 0 && result < actions.Count)
        {
            ResultLabel.Text = $"Selected: {actions[result].Name} - {actions[result].Detail}";
        }
        else
        {
            ResultLabel.Text = "Large action list cancelled";
        }
    }

    private async void OnMultiLineActionListClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Cloud Sync", "Automatically sync your files across all devices in real-time. Changes are instantly reflected everywhere you work.", 0, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Team Collaboration", "Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.", 1, "help_outline_black_48dp", "help_outline_white_48dp"),
            new ActionItem("Advanced Security", "Enable two-factor authentication, encrypted backups, and secure sharing with granular permission controls.", 2, "warning_amber_black_48dp", "warning_amber_white_48dp"),
            new ActionItem("Version History", "Access complete revision history for all your files. Restore previous versions or compare changes side-by-side.", 3, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Smart Notifications", "Receive intelligent alerts about important updates, deadlines, and team activities that matter to you.", 4, "info_black_48dp", "info_white_48dp")
        };

        var dialog = new ActionListDialog(
            "Premium Features",
            actions,
            "Cancel",
            customHeight: null,
            descriptionMaxLines: 2,
            descriptionLineBreakMode: LineBreakMode.TailTruncation);

        var result = await dialog.ShowAsync();

        if (result >= 0 && result < actions.Count)
        {
            ResultLabel.Text = $"Selected premium feature: {actions[result].Name}";
        }
        else
        {
            ResultLabel.Text = "Multi-line action list cancelled";
        }
    }

    private async void OnDescriptionVariationsClicked(object sender, EventArgs e)
    {
        // First, let the user choose which variation to test
        var variationOptions = new List<ActionItem>
        {
            new ActionItem("Single Line (Truncated)", "Default behavior - long text gets truncated Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.", 0, "info_black_48dp", "info_white_48dp"),
            new ActionItem("2 Lines (Word Wrap)", "Text wraps to 2 lines maximum Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.", 1, "info_black_48dp", "info_white_48dp"),
            new ActionItem("3 Lines (Word Wrap)", "Text wraps to 3 lines maximum Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.", 2, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Tail Truncation", "Truncates at end with ellipsis Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.", 3, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Head Truncation", "Truncates at beginning with ellipsis Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.", 4, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Middle Truncation", "Truncates in the middle with ellipsis Invite team members to collaborate on projects. Share workspaces, assign tasks, and track progress together.", 5, "info_black_48dp", "info_white_48dp")
        };

        var variationDialog = new ActionListDialog(
            "Choose Description Style",
            variationOptions,
            "Cancel");

        var variationChoice = await variationDialog.ShowAsync();

        if (variationChoice == -1)
        {
            ResultLabel.Text = "Description variations demo cancelled";
            return;
        }

        // Now show the example with the chosen variation
        var exampleActions = new List<ActionItem>
        {
            new ActionItem("Advanced AI Processing", "Our cutting-edge artificial intelligence algorithms analyze your data in real-time to provide intelligent insights, predictions, and automated recommendations that help you make better decisions faster.", 0, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Enterprise Cloud Storage", "Store unlimited files with military-grade encryption, automatic versioning, instant synchronization across all devices, and seamless collaboration tools for teams of any size.", 1, "info_black_48dp", "info_white_48dp"),
            new ActionItem("Real-Time Analytics Dashboard", "Monitor your business metrics with interactive charts, customizable widgets, live data updates, and comprehensive reporting tools that provide actionable insights at a glance.", 2, "help_outline_black_48dp", "help_outline_white_48dp"),
            new ActionItem("Automated Workflow Engine", "Design and deploy complex business workflows with our intuitive drag-and-drop interface, conditional logic, integrations with hundreds of third-party services, and powerful automation capabilities.", 3, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("24/7 Premium Support", "Get immediate assistance from our expert support team via live chat, email, or phone. Guaranteed response times, dedicated account managers, and comprehensive training resources included.", 4, "help_outline_black_48dp", "help_outline_white_48dp")
        };

        ActionListDialog exampleDialog;

        switch (variationChoice)
        {
            case 0: // Single Line (Truncated)
                exampleDialog = new ActionListDialog(
                    "Single Line Truncation",
                    exampleActions,
                    "Cancel",
                    customHeight: null,
                    descriptionMaxLines: 1,
                    descriptionLineBreakMode: LineBreakMode.TailTruncation);
                ResultLabel.Text = "Showing: Single line with tail truncation (default)";
                break;

            case 1: // 2 Lines (Word Wrap)
                exampleDialog = new ActionListDialog(
                    "2 Lines Word Wrap",
                    exampleActions,
                    "Cancel",
                    customHeight: null,
                    descriptionMaxLines: 2,
                    descriptionLineBreakMode: LineBreakMode.TailTruncation);
                ResultLabel.Text = "Showing: 2 lines with tail truncation (wraps + ellipsis)";
                break;

            case 2: // 3 Lines (Word Wrap)
                exampleDialog = new ActionListDialog(
                    "3 Lines Word Wrap",
                    exampleActions,
                    "Cancel",
                    customHeight: null,
                    descriptionMaxLines: 3,
                    descriptionLineBreakMode: LineBreakMode.TailTruncation);
                ResultLabel.Text = "Showing: 3 lines with tail truncation (wraps + ellipsis)";
                break;

            case 3: // Tail Truncation
                exampleDialog = new ActionListDialog(
                    "Tail Truncation",
                    exampleActions,
                    "Cancel",
                    customHeight: null,
                    descriptionMaxLines: 1,
                    descriptionLineBreakMode: LineBreakMode.TailTruncation);
                ResultLabel.Text = "Showing: Tail truncation (ends with ...)";
                break;

            case 4: // Head Truncation
                exampleDialog = new ActionListDialog(
                    "Head Truncation",
                    exampleActions,
                    "Cancel",
                    customHeight: null,
                    descriptionMaxLines: 1,
                    descriptionLineBreakMode: LineBreakMode.HeadTruncation);
                ResultLabel.Text = "Showing: Head truncation (starts with ...)";
                break;

            case 5: // Middle Truncation
                exampleDialog = new ActionListDialog(
                    "Middle Truncation",
                    exampleActions,
                    "Cancel",
                    customHeight: null,
                    descriptionMaxLines: 1,
                    descriptionLineBreakMode: LineBreakMode.MiddleTruncation);
                ResultLabel.Text = "Showing: Middle truncation (... in middle)";
                break;

            default:
                return;
        }

        var result = await exampleDialog.ShowAsync();

        if (result >= 0 && result < exampleActions.Count)
        {
            ResultLabel.Text = $"Selected: {exampleActions[result].Name} (Variation: {variationOptions[variationChoice].Name})";
        }
        else
        {
            ResultLabel.Text = $"Cancelled after viewing: {variationOptions[variationChoice].Name}";
        }
    }

    private async void OnChainedDialogsClicked(object sender, EventArgs e)
    {
        ResultLabel.Text = "Starting chained dialogs test...";

        // Step 1: Show ActionList Dialog
        var actions = new List<ActionItem>
        {
            new ActionItem("Create New Item", "Start a new creation process", 0, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Delete Item", "Remove an existing item", 1, "error_outline_black_48dp", "error_outline_white_48dp"),
            new ActionItem("View Info", "Display information", 2, "info_black_48dp", "info_white_48dp")
        };

        var actionDialog = new ActionListDialog(
            "Select an Action",
            actions,
            "Cancel");

        var actionResult = await actionDialog.ShowAsync();

        if (actionResult == -1)
        {
            ResultLabel.Text = "Chained dialog test cancelled at step 1 (ActionList)";
            return;
        }

        var selectedAction = actions[actionResult].Name;
        ResultLabel.Text = $"Step 1 complete: Selected '{selectedAction}'";

        // Step 2: Show Confirmation Dialog based on selection
        string confirmTitle = selectedAction switch
        {
            "Create New Item" => "Confirm Creation",
            "Delete Item" => "Confirm Deletion",
            "View Info" => "Confirm View",
            _ => "Confirm Action"
        };

        string confirmMessage = selectedAction switch
        {
            "Create New Item" => "Are you sure you want to create a new item? This will initialize a new workspace.",
            "Delete Item" => "Are you sure you want to delete this item? This action cannot be undone!",
            "View Info" => "Would you like to view detailed information about this item?",
            _ => "Do you want to proceed with this action?"
        };

        var confirmDialog = new ConfirmDialog(
            confirmTitle,
            confirmMessage,
            "Yes, Proceed",
            "No, Cancel",
            selectedAction == "Delete Item" ? DialogType.Warning : DialogType.Help);

        var confirmResult = await confirmDialog.ShowAsync();

        if (!confirmResult)
        {
            ResultLabel.Text = $"Chained dialog test cancelled at step 2 (Confirmation for '{selectedAction}')";
            return;
        }

        ResultLabel.Text = $"Step 2 complete: Confirmed '{selectedAction}'";

        // Step 3: Show Alert Dialog with final result
        string alertTitle = selectedAction switch
        {
            "Create New Item" => "Success!",
            "Delete Item" => "Deleted",
            "View Info" => "Information",
            _ => "Complete"
        };

        string alertMessage = selectedAction switch
        {
            "Create New Item" => "Your new item has been created successfully and is ready to use!",
            "Delete Item" => "The item has been permanently deleted from the system.",
            "View Info" => "Detailed information: This is a sample item with various properties and metadata attached to it.",
            _ => "The action has been completed successfully."
        };

        var alertType = selectedAction switch
        {
            "Create New Item" => DialogType.Success,
            "Delete Item" => DialogType.Warning,
            "View Info" => DialogType.Info,
            _ => DialogType.None
        };

        await AlertDialog.ShowAsync(
            alertTitle,
            alertMessage,
            "OK",
            alertType);

        ResultLabel.Text = $"Chained dialog test complete! Flow: ActionList → Confirm → Alert (Action: '{selectedAction}')";
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

    private async void OnHierarchicalMenuClicked(object sender, EventArgs e)
    {
        // Create hierarchical menu structure
        var actions = new List<ActionItem>
        {
            new ActionItem("File", "File operations", 0, "task_alt_black_48dp", "task_alt_white_48dp")
            {
                SubItems = new List<ActionItem>
                {
                    new ActionItem("New", "Create a new document", 100),
                    new ActionItem("Open", "Open an existing document", 101),
                    new ActionItem("Save", "Save the current document", 102),
                    new ActionItem("Export", "Export to various formats", 103)
                    {
                        SubItems = new List<ActionItem>
                        {
                            new ActionItem("PDF", "Export as PDF", 1000),
                            new ActionItem("Word", "Export as Word document", 1001),
                            new ActionItem("Excel", "Export as Excel spreadsheet", 1002),
                            new ActionItem("HTML", "Export as HTML", 1003)
                        }
                    },
                    new ActionItem("Print", "Print the document", 104)
                }
            },
            new ActionItem("Edit", "Edit operations", 1, "info_black_48dp", "info_white_48dp")
            {
                SubItems = new List<ActionItem>
                {
                    new ActionItem("Undo", "Undo last action", 200),
                    new ActionItem("Redo", "Redo last action", 201),
                    new ActionItem("Cut", "Cut selection", 202),
                    new ActionItem("Copy", "Copy selection", 203),
                    new ActionItem("Paste", "Paste from clipboard", 204)
                }
            },
            new ActionItem("View", "View options", 2, "help_outline_black_48dp", "help_outline_white_48dp")
            {
                SubItems = new List<ActionItem>
                {
                    new ActionItem("Zoom In", "Increase zoom level", 300),
                    new ActionItem("Zoom Out", "Decrease zoom level", 301),
                    new ActionItem("Full Screen", "Toggle full screen mode", 302),
                    new ActionItem("Themes", "Choose a theme", 303)
                    {
                        SubItems = new List<ActionItem>
                        {
                            new ActionItem("Light", "Light theme", 3000),
                            new ActionItem("Dark", "Dark theme", 3001),
                            new ActionItem("Blue", "Blue theme", 3002),
                            new ActionItem("Green", "Green theme", 3003)
                        }
                    }
                }
            },
            new ActionItem("Help", "Get help", 3, "help_outline_black_48dp", "help_outline_white_48dp")
            {
                SubItems = new List<ActionItem>
                {
                    new ActionItem("Documentation", "View documentation", 400),
                    new ActionItem("Tutorials", "Watch tutorials", 401),
                    new ActionItem("About", "About this app", 402)
                }
            }
        };

        var dialog = new ActionListDialog(
            "Main Menu",
            actions,
            "Cancel");

        var result = await dialog.ShowAsync();

        if (result >= 0)
        {
            ResultLabel.Text = $"Selected menu item with value: {result}";
        }
        else
        {
            ResultLabel.Text = "Hierarchical menu cancelled";
        }
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

    // New Features v1.2.0 Event Handlers

    private async void OnTitleMaxLinesClicked(object sender, EventArgs e)
    {
        // Create a custom theme with TitleMaxLines = 1 to show truncation
        var customTheme = _dialogService.CurrentTheme.Clone();
        customTheme.TitleMaxLines = 1;
        customTheme.TitleLineBreakMode = LineBreakMode.TailTruncation;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        var dialog = new AlertDialog(
            "This is a very long title that would normally wrap to multiple lines but now gets truncated to a single line with ellipsis",
            "Notice how the title is limited to 1 line with ellipsis (...). This demonstrates the TitleMaxLines feature.",
            "OK",
            DialogType.Info);

        await MopupService.Instance.PushAsync(dialog);

        // Restore original theme
        _dialogService.CurrentThemeOverride = originalTheme;

        ResultLabel.Text = "Title MaxLines demo shown (1 line with tail truncation)";
    }

    private async void OnTitleLineBreakModeClicked(object sender, EventArgs e)
    {
        // Let user choose which LineBreakMode to test
        var modeOptions = new List<ActionItem>
        {
            new ActionItem("TailTruncation", "Text ends with ... (default)", 0),
            new ActionItem("HeadTruncation", "Text starts with ...", 1),
            new ActionItem("MiddleTruncation", "Text has ... in the middle", 2),
            new ActionItem("WordWrap", "Wraps at word boundaries", 3),
            new ActionItem("CharacterWrap", "Wraps at any character", 4),
            new ActionItem("NoWrap", "No wrapping (may overflow)", 5)
        };

        var modeDialog = new ActionListDialog(
            "Choose Title LineBreakMode",
            modeOptions,
            "Cancel");

        var modeChoice = await modeDialog.ShowAsync();

        if (modeChoice == -1)
        {
            ResultLabel.Text = "Title LineBreakMode demo cancelled";
            return;
        }

        // Map selection to LineBreakMode
        var lineBreakMode = modeChoice switch
        {
            0 => LineBreakMode.TailTruncation,
            1 => LineBreakMode.HeadTruncation,
            2 => LineBreakMode.MiddleTruncation,
            3 => LineBreakMode.WordWrap,
            4 => LineBreakMode.CharacterWrap,
            5 => LineBreakMode.NoWrap,
            _ => LineBreakMode.TailTruncation
        };

        var customTheme = _dialogService.CurrentTheme.Clone();
        // Use 1 line for all modes to make truncation differences obvious
        customTheme.TitleMaxLines = 1;
        customTheme.TitleLineBreakMode = lineBreakMode;
        // Make dialog narrower to force truncation
        customTheme.DialogWidth = 280;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        var platformNote = (modeChoice == 1 || modeChoice == 2)
            ? "\n\n⚠️ NOTE: HeadTruncation and MiddleTruncation may not work on Windows due to MAUI platform limitations."
            : "";

        var dialog = new AlertDialog(
            "This is an extremely long title that will definitely demonstrate the LineBreakMode truncation behavior clearly",
            $"Selected mode: {modeOptions[modeChoice].Name}\n\nTail truncation adds ... at the end.\nHead truncation adds ... at the start.\nMiddle truncation adds ... in the middle.{platformNote}",
            "OK",
            DialogType.Info);

        await MopupService.Instance.PushAsync(dialog);

        // Explicitly set the LineBreakMode after dialog is displayed to ensure it's applied
        await Task.Delay(50);
        dialog.SetTitleLineBreakMode(lineBreakMode);

        // Verify what's actually set
        var actualMode = dialog.GetTitleLineBreakMode();

        _dialogService.CurrentThemeOverride = originalTheme;

        ResultLabel.Text = $"Title LineBreakMode: {modeOptions[modeChoice].Name} (Actual: {actualMode})";
    }

    private async void OnHtmlDescriptionClicked(object sender, EventArgs e)
    {
        // Create a custom theme with HTML support enabled
        var customTheme = _dialogService.CurrentTheme.Clone();
        customTheme.DescriptionTextType = TextType.Html;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        var dialog = new AlertDialog(
            "HTML Formatting Enabled",
            "This description uses <b>bold text</b>, <i>italic text</i>, and <u>underlined text</u>.<br/><br/>" +
            "You can use <strong>strong emphasis</strong> and <em>emphasized text</em>.<br/><br/>" +
            "Perfect for <b>formatted error messages</b>, <i>important notices</i>, or <u>highlighted information</u>!",
            "Got It",
            DialogType.Success);

        await MopupService.Instance.PushAsync(dialog);

        _dialogService.CurrentThemeOverride = originalTheme;

        ResultLabel.Text = "HTML Description demo shown (TextType.Html enabled)";
    }

    private async void OnCombinedFeaturesClicked(object sender, EventArgs e)
    {
        // Create a theme that demonstrates all new features together
        var customTheme = _dialogService.CurrentTheme.Clone();
        customTheme.TitleMaxLines = 2;
        customTheme.TitleLineBreakMode = LineBreakMode.TailTruncation;
        customTheme.DescriptionTextType = TextType.Html;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        var dialog = new AlertDialog(
            "Combined Features Demo: This Long Title Shows TitleMaxLines and LineBreakMode Working Together",
            "<b>Version 1.2.0 Features:</b><br/><br/>" +
            "• <b>TitleMaxLines</b>: Limits title to 2 lines<br/>" +
            "• <b>TitleLineBreakMode</b>: TailTruncation with ...<br/>" +
            "• <b>DescriptionTextType</b>: HTML formatting enabled<br/><br/>" +
            "<i>All features work together seamlessly!</i>",
            "Awesome!",
            DialogType.Success);

        await MopupService.Instance.PushAsync(dialog);

        _dialogService.CurrentThemeOverride = originalTheme;

        ResultLabel.Text = "Combined features demo shown (all v1.2.0 features together)";
    }
}