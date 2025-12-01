using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;

namespace Test.Pages.Demos;

public partial class ToastSnackbarDemoPage : ContentPage
{
    public ToastSnackbarDemoPage()
    {
        InitializeComponent();
    }

    // Toast handlers
    private async void OnSimpleToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("Message sent successfully");
        ResultLabel.Text = "Simple toast shown";
    }

    private async void OnSuccessToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("Operation completed!", DialogType.Success);
        ResultLabel.Text = "Success toast shown";
    }

    private async void OnErrorToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("Something went wrong", DialogType.Error);
        ResultLabel.Text = "Error toast shown";
    }

    private async void OnTopToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("This toast appears at the top", DialogType.Info, ToastDuration.Short, ToastPosition.Top);
        ResultLabel.Text = "Top-positioned toast shown";
    }

    private async void OnLongToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("This toast stays longer (3.5 seconds)", DialogType.Info, ToastDuration.Long);
        ResultLabel.Text = "Long duration toast shown";
    }

    private async void OnStackedToastsClicked(object sender, EventArgs e)
    {
        ResultLabel.Text = "Showing multiple stacked toasts...";

        _ = Toast.ShowAsync("First toast message", DialogType.Info);
        await Task.Delay(300);
        _ = Toast.ShowAsync("Second toast message", DialogType.Success);
        await Task.Delay(300);
        _ = Toast.ShowAsync("Third toast message", DialogType.Warning);

        ResultLabel.Text = "Multiple stacked toasts shown";
    }

    private async void OnBottomLeftToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("Bottom Left!", DialogType.Info, ToastDuration.Short, ToastPosition.Bottom, ToastHorizontalPosition.Left);
        ResultLabel.Text = "Bottom left toast shown";
    }

    private async void OnBottomRightToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("Bottom Right!", DialogType.Success, ToastDuration.Short, ToastPosition.Bottom, ToastHorizontalPosition.Right);
        ResultLabel.Text = "Bottom right toast shown";
    }

    private async void OnTopLeftToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("Top Left!", DialogType.Warning, ToastDuration.Short, ToastPosition.Top, ToastHorizontalPosition.Left);
        ResultLabel.Text = "Top left toast shown";
    }

    private async void OnTopRightToastClicked(object sender, EventArgs e)
    {
        await Toast.ShowAsync("Top Right!", DialogType.Error, ToastDuration.Short, ToastPosition.Top, ToastHorizontalPosition.Right);
        ResultLabel.Text = "Top right toast shown";
    }

    // Snackbar handlers
    private async void OnSimpleSnackbarClicked(object sender, EventArgs e)
    {
        var result = await Snackbar.ShowAsync("File saved to documents");
        ResultLabel.Text = $"Simple snackbar result: {result}";
    }

    private async void OnUndoSnackbarClicked(object sender, EventArgs e)
    {
        var result = await Snackbar.ShowAsync(
            "Item deleted",
            "UNDO",
            () => ResultLabel.Text = "Undo action triggered!");

        ResultLabel.Text = result == SnackbarResult.ActionClicked
            ? "Item restored via UNDO"
            : $"Snackbar dismissed ({result})";
    }

    private async void OnRetrySnackbarClicked(object sender, EventArgs e)
    {
        var result = await Snackbar.ShowAsync(
            "Connection failed",
            "RETRY",
            () => ResultLabel.Text = "Retrying connection...",
            DialogType.Error,
            SnackbarDuration.Long,
            ToastPosition.Bottom);

        ResultLabel.Text = result == SnackbarResult.ActionClicked
            ? "Retry initiated"
            : $"Snackbar dismissed ({result})";
    }

    private async void OnIconSnackbarClicked(object sender, EventArgs e)
    {
        var result = await Snackbar.ShowAsync(
            "Upload complete",
            "VIEW",
            () => ResultLabel.Text = "Opening file...",
            DialogType.Success,
            SnackbarDuration.Short,
            ToastPosition.Bottom);

        ResultLabel.Text = $"Icon snackbar result: {result}";
    }

    private async void OnIndefiniteSnackbarClicked(object sender, EventArgs e)
    {
        ResultLabel.Text = "Showing indefinite snackbar (will stay until action)...";

        var result = await Snackbar.ShowAsync(
            "No internet connection",
            "RETRY",
            null,
            DialogType.Warning,
            SnackbarDuration.Indefinite,
            ToastPosition.Bottom);

        ResultLabel.Text = $"Indefinite snackbar result: {result}";
    }

    private async void OnStackedSnackbarsClicked(object sender, EventArgs e)
    {
        ResultLabel.Text = "Showing multiple stacked snackbars...";

        _ = Snackbar.ShowAsync("First action completed", "UNDO");
        await Task.Delay(500);
        _ = Snackbar.ShowAsync("Second action completed", "VIEW");
        await Task.Delay(500);
        _ = Snackbar.ShowAsync("Third action completed", "DISMISS");

        ResultLabel.Text = "Multiple stacked snackbars shown";
    }

    private async void OnTopSnackbarClicked(object sender, EventArgs e)
    {
        var result = await Snackbar.ShowAsync(
            "New message received",
            "VIEW",
            () => ResultLabel.Text = "Opening message...",
            DialogType.Info,
            SnackbarDuration.Short,
            ToastPosition.Top);

        ResultLabel.Text = $"Top snackbar result: {result}";
    }
}
