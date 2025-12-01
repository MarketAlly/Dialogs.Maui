namespace MarketAlly.Dialogs.Maui.Validation.Validators
{
    /// <summary>
    /// Custom validator using a predicate function
    /// </summary>
    public class CustomValidator : IInputValidator
    {
        private readonly Func<string?, bool> _predicate;
        private readonly string _errorMessage;

        public CustomValidator(Func<string?, bool> predicate, string errorMessage)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        public string ErrorMessageKey => "validation_custom";

        public ValidationResult Validate(string? value)
        {
            if (!_predicate(value))
            {
                return ValidationResult.Error(_errorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
