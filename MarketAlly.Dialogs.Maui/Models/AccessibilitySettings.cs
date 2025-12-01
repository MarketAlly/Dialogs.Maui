namespace MarketAlly.Dialogs.Maui.Models
{
    /// <summary>
    /// Accessibility settings for dialogs
    /// </summary>
    public class AccessibilitySettings
    {
        /// <summary>
        /// Gets or sets whether to automatically focus the first interactive element
        /// </summary>
        public bool AutoFocusFirstElement { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to announce the dialog title to screen readers
        /// </summary>
        public bool AnnounceDialogTitle { get; set; } = true;

        /// <summary>
        /// Gets or sets whether pressing Escape closes the dialog
        /// </summary>
        public bool EscapeToClose { get; set; } = true;

        /// <summary>
        /// Gets or sets whether pressing Enter confirms/submits the dialog
        /// </summary>
        public bool EnterToConfirm { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to trap focus within the dialog
        /// </summary>
        public bool TrapFocus { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to announce state changes
        /// </summary>
        public bool AnnounceStateChanges { get; set; } = true;

        /// <summary>
        /// Creates default accessibility settings
        /// </summary>
        public static AccessibilitySettings Default => new AccessibilitySettings();

        /// <summary>
        /// Creates minimal accessibility settings
        /// </summary>
        public static AccessibilitySettings Minimal => new AccessibilitySettings
        {
            AutoFocusFirstElement = false,
            AnnounceDialogTitle = false,
            AnnounceStateChanges = false
        };
    }
}
