using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;
using Mopups.Services;

namespace Test.Pages.Demos;

public partial class PromptsDemoPage : ContentPage
{
    public PromptsDemoPage()
    {
        InitializeComponent();
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

        var tcs = new TaskCompletionSource<string?>();
        dialog.Disappearing += (s, args) => tcs.TrySetResult(dialog.Text);
        var result = await tcs.Task;

        ResultLabel.Text = string.IsNullOrEmpty(result)
            ? "Password prompt cancelled"
            : $"Password entered (length: {result?.Length ?? 0} characters)";
    }

    private async void OnNotesEditorClicked(object sender, EventArgs e)
    {
        var result = await EditorDialog.ShowAsync(
            "Add Notes",
            "Type your notes here...",
            null,
            "Save",
            "Cancel");

        ResultLabel.Text = string.IsNullOrEmpty(result)
            ? "Notes editor cancelled"
            : $"Notes saved ({result.Length} characters)";
    }

    private async void OnFeedbackEditorClicked(object sender, EventArgs e)
    {
        var dialog = new EditorDialog(
            "Send Feedback",
            "We'd love to hear your thoughts!",
            "Your feedback helps us improve...",
            "Send",
            "Cancel",
            DialogType.Help,
            Keyboard.Text,
            minLines: 5,
            maxLines: 10);

        var result = await dialog.ShowAsync();

        ResultLabel.Text = string.IsNullOrEmpty(result)
            ? "Feedback cancelled"
            : $"Feedback submitted ({result.Split('\n').Length} lines, {result.Length} characters)";
    }
}
