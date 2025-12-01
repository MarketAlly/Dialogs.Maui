namespace MarketAlly.Dialogs.Maui.Validation
{
    /// <summary>
    /// Interface for input validators
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// Validates the input value
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>The validation result</returns>
        ValidationResult Validate(string? value);

        /// <summary>
        /// Gets the localization key for the error message
        /// </summary>
        string ErrorMessageKey { get; }
    }
}
