namespace MarketAlly.Dialogs.Maui.Validation.Validators
{
    /// <summary>
    /// Validates maximum string length
    /// </summary>
    public class MaxLengthValidator : IInputValidator
    {
        private readonly int _maxLength;
        private readonly string? _customMessage;

        public MaxLengthValidator(int maxLength, string? customMessage = null)
        {
            _maxLength = maxLength;
            _customMessage = customMessage;
        }

        public string ErrorMessageKey => "validation_max_length";

        public ValidationResult Validate(string? value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > _maxLength)
            {
                var message = _customMessage ??
                    Core.DialogService.Instance.Localization.GetString(ErrorMessageKey, _maxLength);
                return ValidationResult.Error(message);
            }
            return ValidationResult.Success;
        }
    }
}
