using MarketAlly.Dialogs.Maui.Core;
using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;

namespace Test.Pages.Demos;

public partial class AdvancedDemoPage : ContentPage
{
    private readonly DialogService _dialogService;
    private bool _isDarkTheme = false;

    public AdvancedDemoPage()
    {
        InitializeComponent();
        _dialogService = DialogService.Instance;
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
        _dialogService.RegisterCustomIcon(DialogType.Custom, "custom_icon_light.png", "custom_icon_dark.png");

        await AlertDialog.ShowAsync(
            "Custom Dialog",
            "This is a custom dialog with special styling options.",
            "OK",
            DialogType.Custom);
        ResultLabel.Text = "Custom dialog shown";
    }

    private async void OnTitleMaxLinesClicked(object sender, EventArgs e)
    {
        var customTheme = _dialogService.CurrentTheme.Clone();
        customTheme.TitleMaxLines = 1;
        customTheme.TitleLineBreakMode = LineBreakMode.TailTruncation;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        await AlertDialog.ShowAsync(
            "This is a very long title that would normally wrap to multiple lines but now gets truncated to a single line with ellipsis",
            "Notice how the title is limited to 1 line with ellipsis. This demonstrates the TitleMaxLines feature.",
            "OK",
            DialogType.Info);

        _dialogService.CurrentThemeOverride = originalTheme;
        ResultLabel.Text = "Title MaxLines demo shown (1 line with tail truncation)";
    }

    private async void OnTitleLineBreakModeClicked(object sender, EventArgs e)
    {
        var modeOptions = new List<ActionItem>
        {
            new ActionItem("TailTruncation", "Text ends with ... (default)", 0),
            new ActionItem("HeadTruncation", "Text starts with ...", 1),
            new ActionItem("MiddleTruncation", "Text has ... in the middle", 2),
            new ActionItem("WordWrap", "Wraps at word boundaries", 3)
        };

        var modeChoice = await ActionListDialog.ShowAsync("Choose Title LineBreakMode", modeOptions, "Cancel");

        if (modeChoice == -1)
        {
            ResultLabel.Text = "Title LineBreakMode demo cancelled";
            return;
        }

        var lineBreakMode = modeChoice switch
        {
            0 => LineBreakMode.TailTruncation,
            1 => LineBreakMode.HeadTruncation,
            2 => LineBreakMode.MiddleTruncation,
            3 => LineBreakMode.WordWrap,
            _ => LineBreakMode.TailTruncation
        };

        var customTheme = _dialogService.CurrentTheme.Clone();
        customTheme.TitleMaxLines = 1;
        customTheme.TitleLineBreakMode = lineBreakMode;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        await AlertDialog.ShowAsync(
            "This is an extremely long title that will demonstrate the LineBreakMode truncation behavior clearly",
            $"Selected mode: {modeOptions[modeChoice].Name}",
            "OK",
            DialogType.Info);

        _dialogService.CurrentThemeOverride = originalTheme;
        ResultLabel.Text = $"Title LineBreakMode: {modeOptions[modeChoice].Name}";
    }

    private async void OnHtmlDescriptionClicked(object sender, EventArgs e)
    {
        var customTheme = _dialogService.CurrentTheme.Clone();
        customTheme.DescriptionTextType = TextType.Html;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        await AlertDialog.ShowAsync(
            "HTML Formatting Enabled",
            "This description uses <b>bold text</b>, <i>italic text</i>, and <u>underlined text</u>.<br/><br/>" +
            "Perfect for <b>formatted error messages</b> and <i>important notices</i>!",
            "Got It",
            DialogType.Success);

        _dialogService.CurrentThemeOverride = originalTheme;
        ResultLabel.Text = "HTML Description demo shown";
    }

    private async void OnCombinedFeaturesClicked(object sender, EventArgs e)
    {
        var customTheme = _dialogService.CurrentTheme.Clone();
        customTheme.TitleMaxLines = 2;
        customTheme.TitleLineBreakMode = LineBreakMode.TailTruncation;
        customTheme.DescriptionTextType = TextType.Html;

        var originalTheme = _dialogService.CurrentThemeOverride;
        _dialogService.CurrentThemeOverride = customTheme;

        await AlertDialog.ShowAsync(
            "Combined Features Demo: This Long Title Shows TitleMaxLines and LineBreakMode Working Together",
            "<b>Version 1.2.0 Features:</b><br/><br/>" +
            "- <b>TitleMaxLines</b>: Limits title to 2 lines<br/>" +
            "- <b>TitleLineBreakMode</b>: TailTruncation<br/>" +
            "- <b>DescriptionTextType</b>: HTML enabled<br/><br/>" +
            "<i>All features work together!</i>",
            "Awesome!",
            DialogType.Success);

        _dialogService.CurrentThemeOverride = originalTheme;
        ResultLabel.Text = "Combined v1.2.0 features demo shown";
    }
}
