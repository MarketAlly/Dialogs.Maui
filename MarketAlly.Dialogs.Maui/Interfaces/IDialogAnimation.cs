using MarketAlly.Dialogs.Maui.Animations;

namespace MarketAlly.Dialogs.Maui.Interfaces
{
    /// <summary>
    /// Interface for custom dialog animations
    /// </summary>
    public interface IDialogAnimation
    {
        /// <summary>
        /// Gets the animation type identifier
        /// </summary>
        DialogAnimationType AnimationType { get; }

        /// <summary>
        /// Prepares the view for the enter animation by setting initial state
        /// </summary>
        /// <param name="view">The view to prepare</param>
        void PrepareForAnimateIn(View view);

        /// <summary>
        /// Animates the view into visibility
        /// </summary>
        /// <param name="view">The view to animate</param>
        /// <param name="duration">Animation duration in milliseconds</param>
        /// <returns>A task that completes when the animation finishes</returns>
        Task AnimateIn(View view, uint duration);

        /// <summary>
        /// Animates the view out of visibility
        /// </summary>
        /// <param name="view">The view to animate</param>
        /// <param name="duration">Animation duration in milliseconds</param>
        /// <returns>A task that completes when the animation finishes</returns>
        Task AnimateOut(View view, uint duration);

        /// <summary>
        /// Resets the view to its normal state after animation
        /// </summary>
        /// <param name="view">The view to reset</param>
        void ResetView(View view);
    }
}
