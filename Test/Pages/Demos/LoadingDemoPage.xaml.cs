using MarketAlly.Dialogs.Maui.Dialogs;
using Mopups.Services;

namespace Test.Pages.Demos;

public partial class LoadingDemoPage : ContentPage
{
    private LoadingDialog? _currentLoadingDialog;

    public LoadingDemoPage()
    {
        InitializeComponent();
    }

    private async void OnLoadingClicked(object sender, EventArgs e)
    {
        _currentLoadingDialog = new LoadingDialog("Processing...");
        await MopupService.Instance.PushAsync(_currentLoadingDialog);
        ResultLabel.Text = "Loading dialog shown...";

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
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(500);
                }
            });

        ResultLabel.Text = wasCanceled ? "Loading was canceled by user" : "Loading completed successfully";
    }
}
