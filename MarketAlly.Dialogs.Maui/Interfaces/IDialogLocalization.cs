namespace MarketAlly.Dialogs.Maui.Interfaces
{
    /// <summary>
    /// Interface for dialog localization
    /// </summary>
    public interface IDialogLocalization
    {
        /// <summary>
        /// Gets the localized text for the OK button
        /// </summary>
        string OkButtonText { get; }

        /// <summary>
        /// Gets the localized text for the Cancel button
        /// </summary>
        string CancelButtonText { get; }

        /// <summary>
        /// Gets the localized text for the Yes button
        /// </summary>
        string YesButtonText { get; }

        /// <summary>
        /// Gets the localized text for the No button
        /// </summary>
        string NoButtonText { get; }

        /// <summary>
        /// Gets the localized text for the Loading text
        /// </summary>
        string LoadingText { get; }

        /// <summary>
        /// Gets the localized text for the Select placeholder
        /// </summary>
        string SelectPlaceholder { get; }

        /// <summary>
        /// Gets the localized text for Hex label in color picker
        /// </summary>
        string HexLabel { get; }

        /// <summary>
        /// Gets the localized text for Red label in color picker
        /// </summary>
        string RedLabel { get; }

        /// <summary>
        /// Gets the localized text for Green label in color picker
        /// </summary>
        string GreenLabel { get; }

        /// <summary>
        /// Gets the localized text for Blue label in color picker
        /// </summary>
        string BlueLabel { get; }

        /// <summary>
        /// Gets the localized text for Alpha label in color picker
        /// </summary>
        string AlphaLabel { get; }

        /// <summary>
        /// Gets the localized text for Preset Colors label
        /// </summary>
        string PresetColorsLabel { get; }

        /// <summary>
        /// Gets the localized text for items count with scroll indicator
        /// </summary>
        string ItemsScrollIndicator { get; }

        /// <summary>
        /// Gets the localized text for the Dismiss action
        /// </summary>
        string DismissText { get; }

        /// <summary>
        /// Gets the localized text for the Undo action
        /// </summary>
        string UndoText { get; }

        /// <summary>
        /// Gets the localized text for the Retry action
        /// </summary>
        string RetryText { get; }

        // Date/Time picker strings

        /// <summary>
        /// Gets the localized text for Date label
        /// </summary>
        string DateLabel { get; }

        /// <summary>
        /// Gets the localized text for Time label
        /// </summary>
        string TimeLabel { get; }

        /// <summary>
        /// Gets the localized text for Select Date
        /// </summary>
        string SelectDateText { get; }

        /// <summary>
        /// Gets the localized text for Select Time
        /// </summary>
        string SelectTimeText { get; }

        /// <summary>
        /// Gets the localized text for Today button
        /// </summary>
        string TodayText { get; }

        /// <summary>
        /// Gets the localized text for Now button
        /// </summary>
        string NowText { get; }

        /// <summary>
        /// Gets the localized text for Clear button
        /// </summary>
        string ClearText { get; }

        // Validation strings

        /// <summary>
        /// Gets the localized text for Required field validation
        /// </summary>
        string ValidationRequired { get; }

        /// <summary>
        /// Gets the localized text for Minimum length validation
        /// </summary>
        string ValidationMinLength { get; }

        /// <summary>
        /// Gets the localized text for Maximum length validation
        /// </summary>
        string ValidationMaxLength { get; }

        /// <summary>
        /// Gets the localized text for Invalid format validation
        /// </summary>
        string ValidationInvalidFormat { get; }

        /// <summary>
        /// Gets the localized text for Invalid email validation
        /// </summary>
        string ValidationInvalidEmail { get; }

        /// <summary>
        /// Gets the localized text for Invalid phone validation
        /// </summary>
        string ValidationInvalidPhone { get; }

        /// <summary>
        /// Gets a localized string by key
        /// </summary>
        /// <param name="key">The localization key</param>
        /// <returns>The localized string</returns>
        string GetString(string key);

        /// <summary>
        /// Gets a formatted localized string by key
        /// </summary>
        /// <param name="key">The localization key</param>
        /// <param name="args">Format arguments</param>
        /// <returns>The formatted localized string</returns>
        string GetString(string key, params object[] args);
    }
}