namespace MarketAlly.Dialogs.Maui.Animations
{
    /// <summary>
    /// Defines the available animation types for dialogs
    /// </summary>
    public enum DialogAnimationType
    {
        /// <summary>
        /// No animation
        /// </summary>
        None,

        /// <summary>
        /// Default animation (platform-specific)
        /// </summary>
        Default,

        /// <summary>
        /// Simple fade in/out
        /// </summary>
        Fade,

        /// <summary>
        /// Scale from center
        /// </summary>
        Scale,

        /// <summary>
        /// Scale with bounce effect
        /// </summary>
        ScaleBounce,

        /// <summary>
        /// Slide up from bottom
        /// </summary>
        SlideUp,

        /// <summary>
        /// Slide down from top
        /// </summary>
        SlideDown,

        /// <summary>
        /// Slide in from left
        /// </summary>
        SlideLeft,

        /// <summary>
        /// Slide in from right
        /// </summary>
        SlideRight,

        /// <summary>
        /// Combined fade and scale
        /// </summary>
        FadeScale,

        /// <summary>
        /// Combined slide and fade
        /// </summary>
        SlideFade
    }
}
