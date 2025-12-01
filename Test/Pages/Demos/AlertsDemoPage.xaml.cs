using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;

namespace Test.Pages.Demos;

public partial class AlertsDemoPage : ContentPage
{
    public AlertsDemoPage()
    {
        InitializeComponent();
    }

    private async void OnSuccessAlertClicked(object sender, EventArgs e)
    {
        await AlertDialog.ShowAsync("Success!", "Operation completed successfully.", "OK", DialogType.Success);
        ResultLabel.Text = "Success alert shown";
    }

    private async void OnErrorAlertClicked(object sender, EventArgs e)
    {
        await AlertDialog.ShowAsync("Error", "An error occurred while processing your request.", "Close", DialogType.Error);
        ResultLabel.Text = "Error alert shown";
    }

    private async void OnWarningAlertClicked(object sender, EventArgs e)
    {
        await AlertDialog.ShowAsync("Warning", "This action may have unintended consequences.", "I Understand", DialogType.Warning);
        ResultLabel.Text = "Warning alert shown";
    }

    private async void OnInfoAlertClicked(object sender, EventArgs e)
    {
        await AlertDialog.ShowAsync("Information", "This is some important information for you to know.", "Got It", DialogType.Info);
        ResultLabel.Text = "Info alert shown";
    }

    private async void OnLongTitleAlertClicked(object sender, EventArgs e)
    {
        await AlertDialog.ShowAsync(
            "This is a very long title that should wrap to multiple lines to test how the dialog handles long titles",
            "Short description here.",
            "OK",
            DialogType.Info);
        ResultLabel.Text = "Long title alert shown";
    }

    private async void OnLongDescriptionAlertClicked(object sender, EventArgs e)
    {
        await AlertDialog.ShowAsync(
            "Important Notice",
            "This is a very long description that contains multiple sentences to test how the dialog handles lengthy content. " +
            "The dialog should automatically adjust its height to accommodate all the text without cutting off any content. " +
            "This helps ensure that important information is always visible to the user.",
            "I Understand",
            DialogType.Warning);
        ResultLabel.Text = "Long description alert shown";
    }
}
