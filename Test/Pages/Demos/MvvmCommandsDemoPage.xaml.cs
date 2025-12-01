using System.Windows.Input;
using MarketAlly.Dialogs.Maui.Commands;
using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;

namespace Test.Pages.Demos;

public partial class MvvmCommandsDemoPage : ContentPage
{
    public ICommand AlertCommand { get; }
    public ICommand ConfirmCommand { get; }
    public ICommand PromptCommand { get; }
    public ICommand DatePickerCommand { get; }
    public ICommand ActionListCommand { get; }

    public MvvmCommandsDemoPage()
    {
        InitializeComponent();

        AlertCommand = DialogCommands.CreateAlertCommand(
            () => "MVVM Alert",
            () => "This alert was triggered via an ICommand binding!",
            "Got It",
            DialogType.Info,
            () => ResultLabel.Text = "MVVM Alert command executed");

        ConfirmCommand = DialogCommands.CreateConfirmCommand(
            () => "MVVM Confirm",
            () => "Do you want to proceed with this action?",
            () => ResultLabel.Text = "MVVM Confirm: User confirmed",
            () => ResultLabel.Text = "MVVM Confirm: User cancelled",
            "Yes", "No",
            DialogType.Help);

        PromptCommand = DialogCommands.CreatePromptCommand(
            () => "MVVM Prompt",
            result => ResultLabel.Text = result != null ? $"MVVM Prompt result: {result}" : "MVVM Prompt cancelled",
            "Enter something...",
            null, "Submit", "Cancel");

        DatePickerCommand = DialogCommands.CreateDatePickerCommand(
            () => "MVVM Date Picker",
            result => ResultLabel.Text = result.HasValue ? $"MVVM Date: {result:d}" : "MVVM Date picker cancelled",
            DateTime.Today);

        ActionListCommand = DialogCommands.CreateActionListCommand(
            () => "MVVM Action List",
            () => new List<ActionItem>
            {
                new ActionItem("Option A", "First option", 0),
                new ActionItem("Option B", "Second option", 1),
                new ActionItem("Option C", "Third option", 2)
            },
            result => ResultLabel.Text = result != null ? $"MVVM Action: {result.Name}" : "MVVM Action list cancelled");

        BindingContext = this;
    }
}
