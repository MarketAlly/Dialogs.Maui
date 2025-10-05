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