namespace MarketAlly.Dialogs.Maui.Models
{
    /// <summary>
    /// Defines the duration for Snackbar notifications
    /// </summary>
    public enum SnackbarDuration
    {
        /// <summary>
        /// Short duration (4 seconds)
        /// </summary>
        Short,

        /// <summary>
        /// Long duration (7 seconds)
        /// </summary>
        Long,

        /// <summary>
        /// Stays visible until user interacts (clicks action or dismisses)
        /// </summary>
        Indefinite
    }
}
