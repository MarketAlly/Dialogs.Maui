using MarketAlly.Dialogs.Maui.Dialogs;

namespace Test.Pages.Demos;

public partial class DateTimeDemoPage : ContentPage
{
    public DateTimeDemoPage()
    {
        InitializeComponent();
    }

    private async void OnDatePickerClicked(object sender, EventArgs e)
    {
        var result = await DatePickerDialog.ShowAsync(
            "Select Date",
            "Choose a date for your event");

        ResultLabel.Text = result.HasValue
            ? $"Date selected: {result:dddd, MMMM d, yyyy}"
            : "Date picker cancelled";
    }

    private async void OnDatePickerWithConstraintsClicked(object sender, EventArgs e)
    {
        var result = await DatePickerDialog.ShowAsync(
            "Select Appointment Date",
            "Choose a date within the next 30 days",
            DateTime.Today,
            DateTime.Today,
            DateTime.Today.AddDays(30));

        ResultLabel.Text = result.HasValue
            ? $"Appointment date: {result:d}"
            : "Date picker cancelled";
    }

    private async void OnTimePickerClicked(object sender, EventArgs e)
    {
        var result = await TimePickerDialog.ShowAsync(
            "Select Time",
            "Choose a time for your reminder");

        ResultLabel.Text = result.HasValue
            ? $"Time selected: {DateTime.Today.Add(result.Value):h:mm tt}"
            : "Time picker cancelled";
    }

    private async void OnDateTimePickerClicked(object sender, EventArgs e)
    {
        var result = await DateTimePickerDialog.ShowAsync(
            "Schedule Event",
            "Select both date and time for your event",
            DateTime.Now);

        ResultLabel.Text = result.HasValue
            ? $"Event scheduled: {result:g}"
            : "Date/time picker cancelled";
    }
}
