using DG.Tweening;

namespace YOtherLibraryExt
{
    public static class DOTweenHelper
    {
        /// <summary>
        /// 浮点数动画
        /// </summary>
        public static Tween FloatAni(float startValue, float endValue, float duration,
            System.Action<float> onValueChanged,
            System.Action onComplete = null)
        {
            float currentValue = startValue;
            return DOTween.To(() => currentValue, x => currentValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    if (currentValue > endValue)
                    {
                        currentValue = endValue;
                    }
                    onValueChanged?.Invoke(currentValue);
                })
                .OnComplete(() =>
                {
                    onValueChanged?.Invoke(endValue);
                    onComplete?.Invoke();
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, false);
        }

        /// <summary>
        /// 浮点数动画
        /// </summary>
        public static Tween DoubleAni(double startValue, double endValue, float duration,
            System.Action<double> onValueChanged,
            System.Action onComplete = null)
        {
            double currentValue = startValue;
            return DOTween.To(() => currentValue, x => currentValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    if (currentValue > endValue)
                    {
                        currentValue = endValue;
                    }
                    onValueChanged?.Invoke(currentValue);
                })
                .OnComplete(() =>
                {
                    onValueChanged?.Invoke(endValue);
                    onComplete?.Invoke();
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, false);
        }

        /// <summary>
        /// 整数动画
        /// </summary>
        public static Tween IntAni(int startValue, int endValue, float duration,
           System.Action<int> onValueChanged,
           System.Action onComplete = null)
        {
            int currentValue = startValue;
            return DOTween.To(() => currentValue, x => currentValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    if (currentValue > endValue)
                    {
                        currentValue = endValue;
                    }
                    onValueChanged?.Invoke(currentValue);
                })
                .OnComplete(() =>
                {
                    onValueChanged?.Invoke(endValue);
                    onComplete?.Invoke();
                })
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Normal, false);
        }
    }
}