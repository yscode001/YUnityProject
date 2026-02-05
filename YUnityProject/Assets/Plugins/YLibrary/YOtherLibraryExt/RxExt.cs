using System;
using UniRx;

namespace YOtherLibraryExt
{
    public static class RxExt
    {
        /// <summary>
        /// 双向计数器（支持正计数/倒计数）
        /// </summary>
        /// <param name="start">计数起始值</param>
        /// <param name="end">计数结束值（可大于/小于start）</param>
        /// <param name="current">每次计数的当前值回调（包含start和end，立即执行首次）</param>
        /// <param name="interval">计数时间间隔，默认1秒</param>
        /// <param name="onCompleted">计数正常结束后的回调（可选，手动取消不会触发）</param>
        /// <returns>可释放对象，调用Dispose()可手动取消计数，释放资源</returns>
        /// <exception cref="ArgumentNullException">current回调为空时抛出</exception>
        /// <exception cref="ArgumentException">时间间隔小于等于0时抛出</exception>
        public static IDisposable Counter(int start, int end, Action<int> current,
            TimeSpan? interval = null,
            Action onCompleted = null)
        {
            // 1. 基础入参校验，快速失败
            if (current == null)
                throw new ArgumentNullException(nameof(current), "当前值回调委托不能为空");
            // 间隔默认1秒，校验间隔合法性
            var timeInterval = interval ?? TimeSpan.FromSeconds(1);
            if (timeInterval <= TimeSpan.Zero)
                throw new ArgumentException("计数时间间隔必须大于0", nameof(interval));

            // 2. 起始值=结束值，执行一次回调+结束回调，返回空释放对象
            if (start == end)
            {
                current.Invoke(start);
                onCompleted?.Invoke();
                return Disposable.Empty;
            }

            // 3. 计算双向计数的核心参数（通用化，适配递增/递减）
            int step = start > end ? -1 : 1; // 步长：递减=-1，递增=1
            int totalCount = Math.Abs(start - end) + 1; // 总执行次数（包含首尾，例：5→1共5次，0→3共4次）

            // 4. Rx.NET核心双向计数逻辑（通用化实现，无分支）
            return Observable
                // 立即执行（初始延迟0），之后按自定义间隔发射自增索引（0,1,2...）
                .Timer(TimeSpan.Zero, timeInterval)
                // 映射为当前计数值：起始值 + 步长 * 索引（适配递增/递减）
                .Select(index => start + step * (int)index)
                // 精准控制发射次数，确保最后一个值是end，避免无限序列
                .Take(totalCount)
                // 订阅序列，处理回调、结束、异常
                .Subscribe(
                    onNext: current,       // 每次计数的当前值回调（核心）
                    onError: _ => { },     // 异常回调（此处无特殊处理，可按需扩展）
                    onCompleted: onCompleted // 计数正常结束回调（手动Dispose不会触发）
                );
        }

        public static IDisposable Counter(int start, int end, Action<int> current) => Counter(start, end, current, null, null);
    }
}