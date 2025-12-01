using System.Windows.Input;
using MarketAlly.Dialogs.Maui.Dialogs;
using MarketAlly.Dialogs.Maui.Models;

namespace MarketAlly.Dialogs.Maui.Commands
{
    /// <summary>
    /// Provides factory methods for creating dialog-related commands
    /// </summary>
    public static class DialogCommands
    {
        /// <summary>
        /// Creates a command that shows an alert dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="messageFunc">Function that returns the dialog message</param>
        /// <param name="okText">Optional OK button text</param>
        /// <param name="dialogType">Optional dialog type</param>
        /// <param name="onDismissed">Optional callback when dialog is dismissed</param>
        /// <returns>An ICommand that shows the alert</returns>
        public static ICommand CreateAlertCommand(
            Func<string> titleFunc,
            Func<string> messageFunc,
            string? okText = null,
            DialogType dialogType = DialogType.None,
            Action? onDismissed = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                await AlertDialog.ShowAsync(titleFunc(), messageFunc(), okText, dialogType);
                onDismissed?.Invoke();
            });
        }

        /// <summary>
        /// Creates a command that shows a confirm dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="messageFunc">Function that returns the dialog message</param>
        /// <param name="onConfirm">Action to execute when confirmed</param>
        /// <param name="onCancel">Optional action when cancelled</param>
        /// <param name="confirmText">Optional confirm button text</param>
        /// <param name="cancelText">Optional cancel button text</param>
        /// <param name="dialogType">Optional dialog type</param>
        /// <returns>An ICommand that shows the confirm dialog</returns>
        public static ICommand CreateConfirmCommand(
            Func<string> titleFunc,
            Func<string> messageFunc,
            Action onConfirm,
            Action? onCancel = null,
            string? confirmText = null,
            string? cancelText = null,
            DialogType dialogType = DialogType.None)
        {
            return new AsyncRelayCommand(async () =>
            {
                var result = await ConfirmDialog.ShowAsync(
                    titleFunc(), messageFunc(), confirmText, cancelText, dialogType);

                if (result)
                {
                    onConfirm();
                }
                else
                {
                    onCancel?.Invoke();
                }
            });
        }

        /// <summary>
        /// Creates a command that shows a prompt dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="onResult">Action to execute with the entered text (null if cancelled)</param>
        /// <param name="placeholder">Optional placeholder text</param>
        /// <param name="initialValue">Optional initial value</param>
        /// <param name="okText">Optional OK button text</param>
        /// <param name="cancelText">Optional cancel button text</param>
        /// <param name="keyboard">Optional keyboard type</param>
        /// <returns>An ICommand that shows the prompt dialog</returns>
        public static ICommand CreatePromptCommand(
            Func<string> titleFunc,
            Action<string?> onResult,
            string? placeholder = null,
            string? initialValue = null,
            string? okText = null,
            string? cancelText = null,
            Keyboard? keyboard = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                var result = await PromptDialog.ShowAsync(
                    titleFunc(), null, placeholder, initialValue, keyboard ?? Keyboard.Default);
                onResult(result);
            });
        }

        /// <summary>
        /// Creates a command that shows a date picker dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="onResult">Action to execute with the selected date (null if cancelled)</param>
        /// <param name="initialDate">Optional initial date</param>
        /// <param name="minDate">Optional minimum date</param>
        /// <param name="maxDate">Optional maximum date</param>
        /// <returns>An ICommand that shows the date picker dialog</returns>
        public static ICommand CreateDatePickerCommand(
            Func<string> titleFunc,
            Action<DateTime?> onResult,
            DateTime? initialDate = null,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                var result = await DatePickerDialog.ShowAsync(
                    titleFunc(), null, initialDate, minDate, maxDate);
                onResult(result);
            });
        }

        /// <summary>
        /// Creates a command that shows a time picker dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="onResult">Action to execute with the selected time (null if cancelled)</param>
        /// <param name="initialTime">Optional initial time</param>
        /// <returns>An ICommand that shows the time picker dialog</returns>
        public static ICommand CreateTimePickerCommand(
            Func<string> titleFunc,
            Action<TimeSpan?> onResult,
            TimeSpan? initialTime = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                var result = await TimePickerDialog.ShowAsync(titleFunc(), null, initialTime);
                onResult(result);
            });
        }

        /// <summary>
        /// Creates a command that shows a date/time picker dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="onResult">Action to execute with the selected date/time (null if cancelled)</param>
        /// <param name="initialDateTime">Optional initial date/time</param>
        /// <param name="minDate">Optional minimum date</param>
        /// <param name="maxDate">Optional maximum date</param>
        /// <returns>An ICommand that shows the date/time picker dialog</returns>
        public static ICommand CreateDateTimePickerCommand(
            Func<string> titleFunc,
            Action<DateTime?> onResult,
            DateTime? initialDateTime = null,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                var result = await DateTimePickerDialog.ShowAsync(
                    titleFunc(), null, initialDateTime, minDate, maxDate);
                onResult(result);
            });
        }

        /// <summary>
        /// Creates a command that shows a color picker dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="onResult">Action to execute with the selected color (null if cancelled)</param>
        /// <param name="initialColor">Optional initial color</param>
        /// <returns>An ICommand that shows the color picker dialog</returns>
        public static ICommand CreateColorPickerCommand(
            Func<string> titleFunc,
            Action<Color?> onResult,
            Color? initialColor = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                var result = await ColorPickerDialog.ShowAsync(titleFunc(), null, initialColor);
                onResult(result);
            });
        }

        /// <summary>
        /// Creates a command that shows an action list dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="itemsFunc">Function that returns the list of action items</param>
        /// <param name="onResult">Action to execute with the selected item (null if cancelled)</param>
        /// <param name="cancelText">Optional cancel button text</param>
        /// <returns>An ICommand that shows the action list dialog</returns>
        public static ICommand CreateActionListCommand(
            Func<string> titleFunc,
            Func<List<ActionItem>> itemsFunc,
            Action<ActionItem?> onResult,
            string? cancelText = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                var items = itemsFunc();
                var result = await ActionListDialog.ShowAsync(titleFunc(), items, cancelText);
                onResult(result >= 0 && result < items.Count ? items[result] : null);
            });
        }

        /// <summary>
        /// Creates a command that shows an editor dialog
        /// </summary>
        /// <param name="titleFunc">Function that returns the dialog title</param>
        /// <param name="onResult">Action to execute with the entered text (null if cancelled)</param>
        /// <param name="placeholder">Optional placeholder text</param>
        /// <param name="initialValue">Optional initial value</param>
        /// <param name="okText">Optional OK button text</param>
        /// <param name="cancelText">Optional cancel button text</param>
        /// <returns>An ICommand that shows the editor dialog</returns>
        public static ICommand CreateEditorCommand(
            Func<string> titleFunc,
            Action<string?> onResult,
            string? placeholder = null,
            string? initialValue = null,
            string? okText = null,
            string? cancelText = null)
        {
            return new AsyncRelayCommand(async () =>
            {
                var result = await EditorDialog.ShowAsync(
                    titleFunc(), placeholder, initialValue, okText, cancelText);
                onResult(result);
            });
        }
    }
}
