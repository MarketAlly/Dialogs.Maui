namespace MarketAlly.Dialogs.Maui.Models
{
    /// <summary>
    /// Defines the result of a Snackbar interaction
    /// </summary>
    public enum SnackbarResult
    {
        /// <summary>
        /// User dismissed the snackbar (swipe, tap outside, or programmatic)
        /// </summary>
        Dismissed,

        /// <summary>
        /// User clicked the action button
        /// </summary>
        ActionClicked,

        /// <summary>
        /// Snackbar timed out and auto-dismissed
        /// </summary>
        TimedOut
    }
}
