using MarketAlly.Dialogs.Maui.Validation.Validators;
using System.Text.RegularExpressions;

namespace MarketAlly.Dialogs.Maui.Validation
{
    /// <summary>
    /// Fluent API for building input validation rules
    /// </summary>
    public class InputValidationService
    {
        private readonly List<IInputValidator> _validators = new();

        /// <summary>
        /// Gets all configured validators
        /// </summary>
        public IReadOnlyList<IInputValidator> Validators => _validators.AsReadOnly();

        /// <summary>
        /// Adds a required field validator
        /// </summary>
        /// <param name="errorMessage">Custom error message</param>
        public InputValidationService Required(string? errorMessage = null)
        {
            _validators.Add(new RequiredValidator(errorMessage));
            return this;
        }

        /// <summary>
        /// Adds a minimum length validator
        /// </summary>
        /// <param name="minLength">Minimum required length</param>
        /// <param name="errorMessage">Custom error message</param>
        public InputValidationService MinLength(int minLength, string? errorMessage = null)
        {
            _validators.Add(new MinLengthValidator(minLength, errorMessage));
            return this;
        }

        /// <summary>
        /// Adds a maximum length validator
        /// </summary>
        /// <param name="maxLength">Maximum allowed length</param>
        /// <param name="errorMessage">Custom error message</param>
        public InputValidationService MaxLength(int maxLength, string? errorMessage = null)
        {
            _validators.Add(new MaxLengthValidator(maxLength, errorMessage));
            return this;
        }

        /// <summary>
        /// Adds length range validation
        /// </summary>
        /// <param name="minLength">Minimum length</param>
        /// <param name="maxLength">Maximum length</param>
        public InputValidationService Length(int minLength, int maxLength)
        {
            _validators.Add(new MinLengthValidator(minLength));
            _validators.Add(new MaxLengthValidator(maxLength));
            return this;
        }

        /// <summary>
        /// Adds a regex pattern validator
        /// </summary>
        /// <param name="pattern">The regex pattern</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <param name="options">Regex options</param>
        public InputValidationService Pattern(string pattern, string? errorMessage = null, RegexOptions options = RegexOptions.None)
        {
            _validators.Add(new RegexValidator(pattern, errorMessage, options));
            return this;
        }

        /// <summary>
        /// Adds an email validator
        /// </summary>
        /// <param name="errorMessage">Custom error message</param>
        public InputValidationService Email(string? errorMessage = null)
        {
            _validators.Add(new EmailValidator(errorMessage));
            return this;
        }

        /// <summary>
        /// Adds a phone number validator
        /// </summary>
        /// <param name="errorMessage">Custom error message</param>
        /// <param name="minDigits">Minimum number of digits</param>
        /// <param name="maxDigits">Maximum number of digits</param>
        public InputValidationService Phone(string? errorMessage = null, int minDigits = 7, int maxDigits = 15)
        {
            _validators.Add(new PhoneValidator(errorMessage, minDigits, maxDigits));
            return this;
        }

        /// <summary>
        /// Adds a custom validator
        /// </summary>
        /// <param name="predicate">The validation predicate (returns true if valid)</param>
        /// <param name="errorMessage">Error message when validation fails</param>
        public InputValidationService Custom(Func<string?, bool> predicate, string errorMessage)
        {
            _validators.Add(new CustomValidator(predicate, errorMessage));
            return this;
        }

        /// <summary>
        /// Adds any validator implementing IInputValidator
        /// </summary>
        /// <param name="validator">The validator to add</param>
        public InputValidationService Add(IInputValidator validator)
        {
            _validators.Add(validator);
            return this;
        }

        /// <summary>
        /// Validates the input against all configured validators
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>The first failed validation result, or success if all pass</returns>
        public ValidationResult Validate(string? value)
        {
            foreach (var validator in _validators)
            {
                var result = validator.Validate(value);
                if (!result.IsValid)
                {
                    return result;
                }
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// Validates and returns all validation errors
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>List of all validation errors</returns>
        public List<string> ValidateAll(string? value)
        {
            var errors = new List<string>();
            foreach (var validator in _validators)
            {
                var result = validator.Validate(value);
                if (!result.IsValid && !string.IsNullOrEmpty(result.ErrorMessage))
                {
                    errors.Add(result.ErrorMessage);
                }
            }
            return errors;
        }

        /// <summary>
        /// Checks if the value is valid against all validators
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid(string? value)
        {
            return Validate(value).IsValid;
        }

        /// <summary>
        /// Creates a new empty validation service
        /// </summary>
        public static InputValidationService Create() => new InputValidationService();

        /// <summary>
        /// Creates a required field validation service
        /// </summary>
        public static InputValidationService RequiredField(string? errorMessage = null)
        {
            return new InputValidationService().Required(errorMessage);
        }

        /// <summary>
        /// Creates an email validation service
        /// </summary>
        public static InputValidationService EmailField(bool required = true)
        {
            var service = new InputValidationService();
            if (required)
            {
                service.Required();
            }
            return service.Email();
        }

        /// <summary>
        /// Creates a phone validation service
        /// </summary>
        public static InputValidationService PhoneField(bool required = true)
        {
            var service = new InputValidationService();
            if (required)
            {
                service.Required();
            }
            return service.Phone();
        }
    }
}
