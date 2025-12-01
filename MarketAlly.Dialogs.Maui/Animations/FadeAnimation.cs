using MarketAlly.Dialogs.Maui.Interfaces;

namespace MarketAlly.Dialogs.Maui.Animations
{
    /// <summary>
    /// Simple fade in/out animation
    /// </summary>
    public class FadeAnimation : IDialogAnimation
    {
        public DialogAnimationType AnimationType => DialogAnimationType.Fade;

        public void PrepareForAnimateIn(View view)
        {
            view.Opacity = 0;
        }

        public async Task AnimateIn(View view, uint duration)
        {
            await view.FadeTo(1, duration, Easing.CubicOut);
        }

        public async Task AnimateOut(View view, uint duration)
        {
            await view.FadeTo(0, duration, Easing.CubicIn);
        }

        public void ResetView(View view)
        {
            view.Opacity = 1;
        }
    }
}
