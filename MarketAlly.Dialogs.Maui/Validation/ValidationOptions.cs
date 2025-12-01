namespace MarketAlly.Dialogs.Maui.Validation
{
    /// <summary>
    /// Configuration options for input validation
    /// </summary>
    public class ValidationOptions
    {
        /// <summary>
        /// Gets or sets when validation should be triggered
        /// </summary>
        public ValidationTrigger Trigger { get; set; } = ValidationTrigger.OnSubmit;

        /// <summary>
        /// Gets or sets whether to show error messages
        /// </summary>
        public bool ShowErrorMessage { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to show error border on invalid input
        /// </summary>
        public bool ShowErrorBorder { get; set; } = true;

        /// <summary>
        /// Gets or sets the error color for visual feedback
        /// </summary>
        public Color ErrorColor { get; set; } = Colors.Red;

        /// <summary>
        /// Gets or sets the debounce delay in milliseconds for real-time validation
        /// </summary>
        public int DebounceDelayMs { get; set; } = 300;

        /// <summary>
        /// Gets or sets whether to disable the submit button when input is invalid
        /// </summary>
        public bool DisableSubmitWhenInvalid { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to validate on first load
        /// </summary>
        public bool ValidateOnLoad { get; set; } = false;

        /// <summary>
        /// Creates default validation options
        /// </summary>
        public static ValidationOptions Default => new ValidationOptions();

        /// <summary>
        /// Creates real-time validation options
        /// </summary>
        public static ValidationOptions RealTime => new ValidationOptions
        {
            Trigger = ValidationTrigger.RealTime,
            DebounceDelayMs = 300
        };

        /// <summary>
        /// Creates validation options that only validate on submit
        /// </summary>
        public static ValidationOptions OnSubmitOnly => new ValidationOptions
        {
            Trigger = ValidationTrigger.OnSubmit,
            DisableSubmitWhenInvalid = false
        };
    }

    /// <summary>
    /// Specifies when validation should be triggered
    /// </summary>
    public enum ValidationTrigger
    {
        /// <summary>
        /// Validate only when user submits
        /// </summary>
        OnSubmit,

        /// <summary>
        /// Validate in real-time as user types
        /// </summary>
        RealTime,

        /// <summary>
        /// Validate when input loses focus
        /// </summary>
        OnBlur,

        /// <summary>
        /// Validate both in real-time and on submit
        /// </summary>
        Both
    }
}
