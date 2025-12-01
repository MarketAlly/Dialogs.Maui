using System.Text.RegularExpressions;

namespace MarketAlly.Dialogs.Maui.Validation.Validators
{
    /// <summary>
    /// Validates input against a regular expression pattern
    /// </summary>
    public class RegexValidator : IInputValidator
    {
        private readonly Regex _regex;
        private readonly string? _customMessage;

        public RegexValidator(string pattern, string? customMessage = null, RegexOptions options = RegexOptions.None)
        {
            _regex = new Regex(pattern, options);
            _customMessage = customMessage;
        }

        public string ErrorMessageKey => "validation_invalid_format";

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success; // Empty is valid for regex (use Required for empty check)
            }

            if (!_regex.IsMatch(value))
            {
                var message = _customMessage ?? Core.DialogService.Instance.Localization.ValidationInvalidFormat;
                return ValidationResult.Error(message);
            }
            return ValidationResult.Success;
        }
    }
}
