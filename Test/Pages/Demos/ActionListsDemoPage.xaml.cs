using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;

namespace Test.Pages.Demos;

public partial class ActionListsDemoPage : ContentPage
{
    public ActionListsDemoPage()
    {
        InitializeComponent();
    }

    private async void OnActionListClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Share", "Share this item with others", 0),
            new ActionItem("Edit", "Modify the selected item", 1),
            new ActionItem("Delete", "Remove this item permanently", 2),
            new ActionItem("Archive", "Move to archive folder", 3)
        };

        var result = await ActionListDialog.ShowAsync("Choose an Action", actions, "Cancel");

        ResultLabel.Text = result >= 0 && result < actions.Count
            ? $"Action selected: {actions[result].Name}"
            : "Action list cancelled";
    }

    private async void OnActionListWithIconsClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Success", "Operation completed successfully", 0, "task_alt_black_48dp", "task_alt_white_48dp"),
            new ActionItem("Warning", "Please review this carefully", 1, "warning_amber_black_48dp", "warning_amber_white_48dp"),
            new ActionItem("Error", "Something went wrong", 2, "error_outline_black_48dp", "error_outline_white_48dp"),
            new ActionItem("Information", "Helpful information", 3, "info_black_48dp", "info_white_48dp")
        };

        var result = await ActionListDialog.ShowAsync("Select Content Type", actions, "Cancel");

        ResultLabel.Text = result >= 0 && result < actions.Count
            ? $"Content type selected: {actions[result].Name}"
            : "Action list with icons cancelled";
    }

    private async void OnLargeActionListClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>();
        for (int i = 1; i <= 10; i++)
        {
            actions.Add(new ActionItem($"Option {i}", $"Description for option {i}", i));
        }

        var result = await ActionListDialog.ShowAsync("10 Item Action List", actions, "Cancel");

        ResultLabel.Text = result >= 0 && result < actions.Count
            ? $"Selected: {actions[result].Name}"
            : "Large action list cancelled";
    }

    private async void OnHierarchicalMenuClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("File", "File operations", 0)
            {
                SubItems = new List<ActionItem>
                {
                    new ActionItem("New", "Create a new document", 100),
                    new ActionItem("Open", "Open an existing document", 101),
                    new ActionItem("Save", "Save the current document", 102)
                }
            },
            new ActionItem("Edit", "Edit operations", 1)
            {
                SubItems = new List<ActionItem>
                {
                    new ActionItem("Undo", "Undo last action", 200),
                    new ActionItem("Redo", "Redo last action", 201),
                    new ActionItem("Copy", "Copy selection", 202)
                }
            }
        };

        var result = await ActionListDialog.ShowAsync("Main Menu", actions, "Cancel");
        ResultLabel.Text = result >= 0 ? $"Selected menu item with value: {result}" : "Hierarchical menu cancelled";
    }

    private async void OnChainedDialogsClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Create New Item", "Start a new creation process", 0),
            new ActionItem("Delete Item", "Remove an existing item", 1),
            new ActionItem("View Info", "Display information", 2)
        };

        var actionResult = await ActionListDialog.ShowAsync("Select an Action", actions, "Cancel");

        if (actionResult == -1)
        {
            ResultLabel.Text = "Chained dialog cancelled at step 1";
            return;
        }

        var selectedAction = actions[actionResult].Name;
        var confirmResult = await ConfirmDialog.ShowAsync(
            $"Confirm {selectedAction}",
            $"Are you sure you want to {selectedAction.ToLower()}?",
            DialogType.Help);

        if (!confirmResult)
        {
            ResultLabel.Text = $"Chained dialog cancelled at step 2";
            return;
        }

        await AlertDialog.ShowAsync("Success!", $"{selectedAction} completed.", "OK", DialogType.Success);
        ResultLabel.Text = $"Chained dialog complete! Flow: ActionList -> Confirm -> Alert";
    }

    private async void OnMultiLineActionListClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Cloud Sync", "Automatically sync your files across all devices in real-time.", 0),
            new ActionItem("Team Collaboration", "Invite team members to collaborate on projects.", 1),
            new ActionItem("Advanced Security", "Enable two-factor authentication and encryption.", 2)
        };

        var dialog = new ActionListDialog("Premium Features", actions, "Cancel",
            customHeight: null, descriptionMaxLines: 2, descriptionLineBreakMode: LineBreakMode.TailTruncation);

        var result = await dialog.ShowAsync();

        ResultLabel.Text = result >= 0 && result < actions.Count
            ? $"Selected: {actions[result].Name}"
            : "Multi-line action list cancelled";
    }

    private async void OnDescriptionVariationsClicked(object sender, EventArgs e)
    {
        var variationOptions = new List<ActionItem>
        {
            new ActionItem("Single Line", "Default behavior", 0),
            new ActionItem("2 Lines", "Text wraps to 2 lines", 1),
            new ActionItem("3 Lines", "Text wraps to 3 lines", 2)
        };

        var variationChoice = await ActionListDialog.ShowAsync("Choose Style", variationOptions, "Cancel");

        if (variationChoice == -1)
        {
            ResultLabel.Text = "Description variations cancelled";
            return;
        }

        var exampleActions = new List<ActionItem>
        {
            new ActionItem("Advanced AI", "Our cutting-edge AI algorithms analyze your data in real-time to provide intelligent insights and recommendations.", 0),
            new ActionItem("Cloud Storage", "Store unlimited files with encryption, versioning, and synchronization across all devices.", 1)
        };

        var maxLines = variationChoice + 1;
        var exampleDialog = new ActionListDialog($"{maxLines} Line(s)", exampleActions, "Cancel",
            customHeight: null, descriptionMaxLines: maxLines, descriptionLineBreakMode: LineBreakMode.TailTruncation);

        var result = await exampleDialog.ShowAsync();
        ResultLabel.Text = result >= 0 ? $"Selected: {exampleActions[result].Name}" : "Cancelled";
    }

    private async void OnActionCallbacksClicked(object sender, EventArgs e)
    {
        var actions = new List<ActionItem>
        {
            new ActionItem("Show Alert", () => ResultLabel.Text = "Alert action triggered!", "Triggers a synchronous action"),
            new ActionItem("Load Data", async () =>
            {
                ResultLabel.Text = "Loading data...";
                await Task.Delay(1000);
                ResultLabel.Text = "Data loaded successfully!";
            }, "Triggers an async action with delay"),
            new ActionItem("Show Toast", async () =>
            {
                await Toast.ShowAsync("Action completed!", DialogType.Success);
                ResultLabel.Text = "Toast was shown via action callback";
            }, "Displays a toast notification")
        };

        ResultLabel.Text = "Select an action - callback will execute automatically";

        bool wasSelected = await ActionListDialog.ShowWithActionsAsync("Action Callbacks Demo", actions, "Cancel");

        if (!wasSelected)
        {
            ResultLabel.Text = "Action callbacks demo cancelled";
        }
    }
}
