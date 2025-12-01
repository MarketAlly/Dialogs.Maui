namespace MarketAlly.Dialogs.Maui.Validation
{
    /// <summary>
    /// Represents the result of a validation check
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets whether the validation passed
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets the error message if validation failed
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets a successful validation result
        /// </summary>
        public static ValidationResult Success => new ValidationResult { IsValid = true };

        /// <summary>
        /// Creates a failed validation result with an error message
        /// </summary>
        /// <param name="message">The error message</param>
        /// <returns>A failed validation result</returns>
        public static ValidationResult Error(string message) => new ValidationResult
        {
            IsValid = false,
            ErrorMessage = message
        };

        /// <summary>
        /// Creates a validation result from a boolean condition
        /// </summary>
        /// <param name="isValid">Whether the validation passed</param>
        /// <param name="errorMessage">Error message if validation failed</param>
        public static ValidationResult FromCondition(bool isValid, string errorMessage)
        {
            return isValid ? Success : Error(errorMessage);
        }
    }
}
