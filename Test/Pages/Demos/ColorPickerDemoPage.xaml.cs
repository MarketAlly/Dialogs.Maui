using MarketAlly.Dialogs.Maui.Dialogs;

namespace Test.Pages.Demos;

public partial class ColorPickerDemoPage : ContentPage
{
    public ColorPickerDemoPage()
    {
        InitializeComponent();
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
            ResultLabel.TextColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
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
            ResultLabel.TextColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
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
            ResultLabel.Text = $"Custom color: RGB({result.Red * 255:F0}, {result.Green * 255:F0}, {result.Blue * 255:F0})";
            ResultLabel.TextColor = result;
        }
        else
        {
            ResultLabel.Text = "Custom color picker cancelled";
            ResultLabel.TextColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black;
        }
    }
}
