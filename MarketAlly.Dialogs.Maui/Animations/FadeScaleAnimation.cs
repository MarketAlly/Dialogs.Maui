using MarketAlly.Dialogs.Maui.Interfaces;

namespace MarketAlly.Dialogs.Maui.Animations
{
    /// <summary>
    /// Combined fade and scale animation
    /// </summary>
    public class FadeScaleAnimation : IDialogAnimation
    {
        public DialogAnimationType AnimationType => DialogAnimationType.FadeScale;

        public void PrepareForAnimateIn(View view)
        {
            view.Scale = 0.9;
            view.Opacity = 0;
        }

        public async Task AnimateIn(View view, uint duration)
        {
            await Task.WhenAll(
                view.ScaleTo(1, duration, Easing.CubicOut),
                view.FadeTo(1, duration, Easing.CubicOut)
            );
        }

        public async Task AnimateOut(View view, uint duration)
        {
            await Task.WhenAll(
                view.ScaleTo(0.9, duration, Easing.CubicIn),
                view.FadeTo(0, duration, Easing.CubicIn)
            );
        }

        public void ResetView(View view)
        {
            view.Scale = 1;
            view.Opacity = 1;
        }
    }
}
