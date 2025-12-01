using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;

namespace Test.Pages.Demos;

public partial class ThemePresetsDemoPage : ContentPage
{
    public ThemePresetsDemoPage()
    {
        InitializeComponent();
    }

    private async void OnMaterialLightClicked(object sender, EventArgs e)
    {
        DialogThemePresets.ApplyPreset(DesignSystem.Material, isDark: false);
        CurrentThemeLabel.Text = "Material Design 3 Light";
        await ShowThemePreviewAlert("Material Design 3 Light");
    }

    private async void OnMaterialDarkClicked(object sender, EventArgs e)
    {
        DialogThemePresets.ApplyPreset(DesignSystem.Material, isDark: true);
        CurrentThemeLabel.Text = "Material Design 3 Dark";
        await ShowThemePreviewAlert("Material Design 3 Dark");
    }

    private async void OnFluentLightClicked(object sender, EventArgs e)
    {
        DialogThemePresets.ApplyPreset(DesignSystem.Fluent, isDark: false);
        CurrentThemeLabel.Text = "Microsoft Fluent Light";
        await ShowThemePreviewAlert("Microsoft Fluent Light");
    }

    private async void OnFluentDarkClicked(object sender, EventArgs e)
    {
        DialogThemePresets.ApplyPreset(DesignSystem.Fluent, isDark: true);
        CurrentThemeLabel.Text = "Microsoft Fluent Dark";
        await ShowThemePreviewAlert("Microsoft Fluent Dark");
    }

    private async void OnCupertinoLightClicked(object sender, EventArgs e)
    {
        DialogThemePresets.ApplyPreset(DesignSystem.Cupertino, isDark: false);
        CurrentThemeLabel.Text = "Apple Cupertino Light";
        await ShowThemePreviewAlert("Apple Cupertino Light");
    }

    private async void OnCupertinoDarkClicked(object sender, EventArgs e)
    {
        DialogThemePresets.ApplyPreset(DesignSystem.Cupertino, isDark: true);
        CurrentThemeLabel.Text = "Apple Cupertino Dark";
        await ShowThemePreviewAlert("Apple Cupertino Dark");
    }

    private void OnResetThemeClicked(object sender, EventArgs e)
    {
        DialogService.Instance.CurrentThemeOverride = null;
        CurrentThemeLabel.Text = "System Default";
    }

    private async Task ShowThemePreviewAlert(string themeName)
    {
        await AlertDialog.ShowAsync(
            themeName,
            $"This is how dialogs look with the {themeName} theme. Notice the different corner radius, colors, and typography.",
            "Nice!",
            DialogType.Info);
    }
}
