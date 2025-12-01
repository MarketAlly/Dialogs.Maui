using System.Text.RegularExpressions;

namespace MarketAlly.Dialogs.Maui.Validation.Validators
{
    /// <summary>
    /// Validates phone numbers
    /// </summary>
    public class PhoneValidator : IInputValidator
    {
        // Basic phone pattern allowing various formats
        private static readonly Regex PhoneRegex = new Regex(
            @"^[\+]?[(]?[0-9]{1,4}[)]?[-\s\./0-9]*$",
            RegexOptions.Compiled);

        private readonly string? _customMessage;
        private readonly int _minDigits;
        private readonly int _maxDigits;

        public PhoneValidator(string? customMessage = null, int minDigits = 7, int maxDigits = 15)
        {
            _customMessage = customMessage;
            _minDigits = minDigits;
            _maxDigits = maxDigits;
        }

        public string ErrorMessageKey => "validation_invalid_phone";

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success; // Empty is valid (use Required for empty check)
            }

            // Check basic format
            if (!PhoneRegex.IsMatch(value))
            {
                var message = _customMessage ?? Core.DialogService.Instance.Localization.ValidationInvalidPhone;
                return ValidationResult.Error(message);
            }

            // Check digit count
            var digitCount = value.Count(char.IsDigit);
            if (digitCount < _minDigits || digitCount > _maxDigits)
            {
                var message = _customMessage ?? Core.DialogService.Instance.Localization.ValidationInvalidPhone;
                return ValidationResult.Error(message);
            }

            return ValidationResult.Success;
        }
    }
}
