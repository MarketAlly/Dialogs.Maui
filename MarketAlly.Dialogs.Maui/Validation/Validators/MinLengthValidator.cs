namespace MarketAlly.Dialogs.Maui.Validation.Validators
{
    /// <summary>
    /// Validates minimum string length
    /// </summary>
    public class MinLengthValidator : IInputValidator
    {
        private readonly int _minLength;
        private readonly string? _customMessage;

        public MinLengthValidator(int minLength, string? customMessage = null)
        {
            _minLength = minLength;
            _customMessage = customMessage;
        }

        public string ErrorMessageKey => "validation_min_length";

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < _minLength)
            {
                var message = _customMessage ??
                    Core.DialogService.Instance.Localization.GetString(ErrorMessageKey, _minLength);
                return ValidationResult.Error(message);
            }
            return ValidationResult.Success;
        }
    }
}
