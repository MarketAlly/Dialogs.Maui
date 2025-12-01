namespace MarketAlly.Dialogs.Maui.Validation.Validators
{
    /// <summary>
    /// Validates that input is not empty
    /// </summary>
    public class RequiredValidator : IInputValidator
    {
        private readonly string? _customMessage;

        public RequiredValidator(string? customMessage = null)
        {
            _customMessage = customMessage;
        }

        public string ErrorMessageKey => "validation_required";

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ValidationResult.Error(_customMessage ?? Core.DialogService.Instance.Localization.ValidationRequired);
            }
            return ValidationResult.Success;
        }
    }
}
