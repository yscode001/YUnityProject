using System;
using DG.Tweening;

namespace YOtherLibraryExt
{
    public static class DOTweenHelper
    {
        public static Tween FloatAni(float startValue, float endValue, float duration, Action<float> onValueChanged, Action onComplete = null)
        {
            float currentValue = startValue;
            return DOTween.To(() => currentValue, x => currentValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    onValueChanged?.Invoke(currentValue);
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, false);
        }
        public static Tween DoubleAni(double startValue, double endValue, float duration, Action<double> onValueChanged, Action onComplete = null)
        {
            double currentValue = startValue;
            return DOTween.To(() => currentValue, x => currentValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    onValueChanged?.Invoke(currentValue);
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, false);
        }
        public static Tween IntAni(int startValue, int endValue, float duration, Action<int> onValueChanged, Action onComplete = null)
        {
            int currentValue = startValue;
            return DOTween.To(() => currentValue, x => currentValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    onValueChanged?.Invoke(currentValue);
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, false);
        }
        public static Tween LongAni(long startValue, long endValue, float duration, Action<long> onValueChanged, Action onComplete = null)
        {
            long currentValue = startValue;
            return DOTween.To(() => currentValue, x => currentValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    onValueChanged?.Invoke(currentValue);
                })
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, false);
        }
    }
}