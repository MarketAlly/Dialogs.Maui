namespace MarketAlly.Dialogs.Maui.Models
{
    /// <summary>
    /// Defines the type of dialog which determines the icon displayed
    /// </summary>
    public enum DialogType
    {
        /// <summary>
        /// No icon displayed
        /// </summary>
        None,

        /// <summary>
        /// Information icon
        /// </summary>
        Info,

        /// <summary>
        /// Success/checkmark icon
        /// </summary>
        Success,

        /// <summary>
        /// Warning/caution icon
        /// </summary>
        Warning,

        /// <summary>
        /// Error/problem icon
        /// </summary>
        Error,

        /// <summary>
        /// Help/question icon
        /// </summary>
        Help,

        /// <summary>
        /// Decision/fork icon
        /// </summary>
        Decide,

        /// <summary>
        /// Stop/hand icon
        /// </summary>
        Stop,

        /// <summary>
        /// Custom icon provided by the user
        /// </summary>
        Custom
    }
}