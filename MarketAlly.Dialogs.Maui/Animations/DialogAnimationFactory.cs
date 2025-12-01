using MarketAlly.Dialogs.Maui.Interfaces;

namespace MarketAlly.Dialogs.Maui.Animations
{
    /// <summary>
    /// Factory for creating dialog animations
    /// </summary>
    public static class DialogAnimationFactory
    {
        /// <summary>
        /// Creates an animation instance for the specified type
        /// </summary>
        /// <param name="type">The animation type</param>
        /// <returns>An animation instance, or null for None type</returns>
        public static IDialogAnimation? Create(DialogAnimationType type)
        {
            return type switch
            {
                DialogAnimationType.None => null,
                DialogAnimationType.Default => new FadeScaleAnimation(),
                DialogAnimationType.Fade => new FadeAnimation(),
                DialogAnimationType.Scale => new ScaleAnimation(withBounce: false),
                DialogAnimationType.ScaleBounce => new ScaleAnimation(withBounce: true),
                DialogAnimationType.SlideUp => new SlideAnimation(SlideAnimation.SlideDirection.Up),
                DialogAnimationType.SlideDown => new SlideAnimation(SlideAnimation.SlideDirection.Down),
                DialogAnimationType.SlideLeft => new SlideAnimation(SlideAnimation.SlideDirection.Left),
                DialogAnimationType.SlideRight => new SlideAnimation(SlideAnimation.SlideDirection.Right),
                DialogAnimationType.FadeScale => new FadeScaleAnimation(),
                DialogAnimationType.SlideFade => new SlideAnimation(SlideAnimation.SlideDirection.Up, withFade: true),
                _ => new FadeScaleAnimation()
            };
        }

        /// <summary>
        /// Gets a platform-safe animation that falls back to fade on Windows for slide animations
        /// Windows has issues with translation animations on dialogs
        /// </summary>
        /// <param name="type">The requested animation type</param>
        /// <returns>A platform-appropriate animation</returns>
        public static IDialogAnimation? GetPlatformSafeAnimation(DialogAnimationType type)
        {
#if WINDOWS
            // Windows has issues with slide animations, fall back to fade
            if (type == DialogAnimationType.SlideUp ||
                type == DialogAnimationType.SlideDown ||
                type == DialogAnimationType.SlideLeft ||
                type == DialogAnimationType.SlideRight ||
                type == DialogAnimationType.SlideFade)
            {
                return new FadeAnimation();
            }
#endif
            return Create(type);
        }

        /// <summary>
        /// Checks if the animation type is a slide-based animation
        /// </summary>
        public static bool IsSlideAnimation(DialogAnimationType type)
        {
            return type == DialogAnimationType.SlideUp ||
                   type == DialogAnimationType.SlideDown ||
                   type == DialogAnimationType.SlideLeft ||
                   type == DialogAnimationType.SlideRight ||
                   type == DialogAnimationType.SlideFade;
        }

        /// <summary>
        /// Gets the recommended animation for the current platform
        /// </summary>
        public static DialogAnimationType GetPlatformDefaultAnimation()
        {
#if WINDOWS
            return DialogAnimationType.FadeScale;
#elif ANDROID
            return DialogAnimationType.FadeScale;
#elif IOS || MACCATALYST
            return DialogAnimationType.Scale;
#else
            return DialogAnimationType.FadeScale;
#endif
        }
    }
}
