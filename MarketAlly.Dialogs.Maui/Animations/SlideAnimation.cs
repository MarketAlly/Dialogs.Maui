using MarketAlly.Dialogs.Maui.Interfaces;

namespace MarketAlly.Dialogs.Maui.Animations
{
    /// <summary>
    /// Slide animation from specified direction
    /// </summary>
    public class SlideAnimation : IDialogAnimation
    {
        private readonly SlideDirection _direction;
        private readonly bool _withFade;

        public enum SlideDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        public SlideAnimation(SlideDirection direction, bool withFade = false)
        {
            _direction = direction;
            _withFade = withFade;
        }

        public DialogAnimationType AnimationType => _direction switch
        {
            SlideDirection.Up => _withFade ? DialogAnimationType.SlideFade : DialogAnimationType.SlideUp,
            SlideDirection.Down => _withFade ? DialogAnimationType.SlideFade : DialogAnimationType.SlideDown,
            SlideDirection.Left => _withFade ? DialogAnimationType.SlideFade : DialogAnimationType.SlideLeft,
            SlideDirection.Right => _withFade ? DialogAnimationType.SlideFade : DialogAnimationType.SlideRight,
            _ => DialogAnimationType.SlideUp
        };

        public void PrepareForAnimateIn(View view)
        {
            var offset = GetStartOffset();
            view.TranslationX = offset.X;
            view.TranslationY = offset.Y;
            if (_withFade)
            {
                view.Opacity = 0;
            }
        }

        public async Task AnimateIn(View view, uint duration)
        {
            var tasks = new List<Task>
            {
                view.TranslateTo(0, 0, duration, Easing.CubicOut)
            };

            if (_withFade)
            {
                tasks.Add(view.FadeTo(1, duration / 2, Easing.Linear));
            }

            await Task.WhenAll(tasks);
        }

        public async Task AnimateOut(View view, uint duration)
        {
            var offset = GetEndOffset();
            var tasks = new List<Task>
            {
                view.TranslateTo(offset.X, offset.Y, duration, Easing.CubicIn)
            };

            if (_withFade)
            {
                tasks.Add(view.FadeTo(0, duration, Easing.CubicIn));
            }

            await Task.WhenAll(tasks);
        }

        public void ResetView(View view)
        {
            view.TranslationX = 0;
            view.TranslationY = 0;
            view.Opacity = 1;
        }

        private (double X, double Y) GetStartOffset()
        {
            return _direction switch
            {
                SlideDirection.Up => (0, 300),
                SlideDirection.Down => (0, -300),
                SlideDirection.Left => (300, 0),
                SlideDirection.Right => (-300, 0),
                _ => (0, 300)
            };
        }

        private (double X, double Y) GetEndOffset()
        {
            return _direction switch
            {
                SlideDirection.Up => (0, 300),
                SlideDirection.Down => (0, -300),
                SlideDirection.Left => (-300, 0),
                SlideDirection.Right => (300, 0),
                _ => (0, 300)
            };
        }
    }
}
