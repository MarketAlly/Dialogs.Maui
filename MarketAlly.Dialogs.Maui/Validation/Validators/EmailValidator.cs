using System.Text.RegularExpressions;

namespace MarketAlly.Dialogs.Maui.Validation.Validators
{
    /// <summary>
    /// Validates email addresses
    /// </summary>
    public class EmailValidator : IInputValidator
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly string? _customMessage;

        public EmailValidator(string? customMessage = null)
        {
            _customMessage = customMessage;
        }

        public string ErrorMessageKey => "validation_invalid_email";

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success; // Empty is valid (use Required for empty check)
            }

            if (!EmailRegex.IsMatch(value))
            {
                var message = _customMessage ?? Core.DialogService.Instance.Localization.ValidationInvalidEmail;
                return ValidationResult.Error(message);
            }
            return ValidationResult.Success;
        }
    }
}
