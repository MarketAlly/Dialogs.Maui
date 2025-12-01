using Test.Pages.Demos;

namespace Test.Pages;

public partial class DialogDemoPage : ContentPage
{
    public DialogDemoPage()
    {
        InitializeComponent();
    }

    private async void OnAlertsCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlertsDemoPage());
    }

    private async void OnConfirmationsCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfirmationsDemoPage());
    }

    private async void OnPromptsCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PromptsDemoPage());
    }

    private async void OnDateTimeCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DateTimeDemoPage());
    }

    private async void OnActionListsCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ActionListsDemoPage());
    }

    private async void OnColorPickerCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ColorPickerDemoPage());
    }

    private async void OnLoadingCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoadingDemoPage());
    }

    private async void OnToastSnackbarCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ToastSnackbarDemoPage());
    }

    private async void OnThemePresetsCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ThemePresetsDemoPage());
    }

    private async void OnMvvmCommandsCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MvvmCommandsDemoPage());
    }

    private async void OnAdvancedCategoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdvancedDemoPage());
    }
}
