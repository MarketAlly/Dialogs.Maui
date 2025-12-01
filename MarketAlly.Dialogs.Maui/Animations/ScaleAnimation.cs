using MarketAlly.Dialogs.Maui.Interfaces;

namespace MarketAlly.Dialogs.Maui.Animations
{
    /// <summary>
    /// Scale animation from center
    /// </summary>
    public class ScaleAnimation : IDialogAnimation
    {
        private readonly bool _withBounce;

        public ScaleAnimation(bool withBounce = false)
        {
            _withBounce = withBounce;
        }

        public DialogAnimationType AnimationType => _withBounce ? DialogAnimationType.ScaleBounce : DialogAnimationType.Scale;

        public void PrepareForAnimateIn(View view)
        {
            view.Scale = 0;
            view.Opacity = 0;
        }

        public async Task AnimateIn(View view, uint duration)
        {
            var easing = _withBounce ? Easing.SpringOut : Easing.CubicOut;
            var targetScale = _withBounce ? 1.0 : 1.0;

            await Task.WhenAll(
                view.ScaleTo(targetScale, duration, easing),
                view.FadeTo(1, duration / 2, Easing.Linear)
            );
        }

        public async Task AnimateOut(View view, uint duration)
        {
            await Task.WhenAll(
                view.ScaleTo(0.8, duration, Easing.CubicIn),
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
