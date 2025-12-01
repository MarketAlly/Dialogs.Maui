using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;

namespace Test.Pages.Demos;

public partial class ConfirmationsDemoPage : ContentPage
{
    public ConfirmationsDemoPage()
    {
        InitializeComponent();
    }

    private async void OnYesNoConfirmationClicked(object sender, EventArgs e)
    {
        var result = await ConfirmDialog.ShowAsync(
            "Confirm Action",
            "Are you sure you want to proceed with this action?",
            "Yes",
            "No",
            DialogType.Help);

        ResultLabel.Text = $"Confirmation result: {(result ? "Yes" : "No")}";
    }

    private async void OnDeleteConfirmationClicked(object sender, EventArgs e)
    {
        var result = await ConfirmDialog.ShowAsync(
            "Delete Item",
            "This will permanently delete the selected item. This action cannot be undone.",
            "Delete",
            "Cancel",
            DialogType.Warning);

        ResultLabel.Text = $"Delete confirmation result: {(result ? "Deleted" : "Cancelled")}";
    }
}
