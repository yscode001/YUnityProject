using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace YOtherLibraryExt
{
    #region 双向计数器
    public static partial class RxExt
    {
        /// <summary>
        /// 双向计数器（支持递增/递减）
        /// </summary>
        /// <param name="start">计数起始值</param>
        /// <param name="end">计数结束值（可大于/小于start，支持递增/递减）</param>
        /// <param name="current">每次计数的当前值回调（包含start和end；仅最后一步会自动将步进值修正为end，确保精准匹配结束值）</param>
        /// <param name="completed">计数正常结束回调（手动Dispose不会触发）</param>
        /// <param name="canceled">计数被手动取消时的回调（调用Dispose触发）</param>
        /// <param name="step">步长（取值范围：1 ~ uint.MaxValue；uint类型保证非负；转换为int时若超过int.MaxValue会抛出OverflowException；实际会根据递增/递减自动添加正负方向）</param>
        /// <param name="useMainThread">是否在Unity主线程执行所有回调（current/completed/canceled），默认true；
        /// 注：Unity API（如Transform/Debug）必须在主线程执行，开启此参数可避免跨线程异常</param>
        /// <param name="dueTime">首次执行延迟时间（默认TimeSpan.Zero，立即执行）</param>
        /// <param name="period">计数时间间隔（默认TimeSpan.FromSeconds(1)，1秒/次）</param>
        /// <returns>可释放对象，调用Dispose()可手动取消计数、触发canceled回调并释放资源；
        /// 注：Unity中建议使用AddTo(this)或加入CompositeDisposable统一管理，避免内存泄漏</returns>
        /// <exception cref="ArgumentException">参数不合法时抛出（start=end/step=0/dueTime<0/period≤0）</exception>
        /// <exception cref="ArgumentNullException">current回调为null时抛出</exception>
        /// <exception cref="OverflowException">步长转换为int时溢出（step超过int.MaxValue）；总计数次数超过int.MaxValue时溢出；步进值计算时溢出</exception>
        /// <example>
        /// 递增示例：Counter(0, 10, val => Debug.Log(val), step:2) → 回调0→2→4→6→8→10
        /// 递减示例：Counter(10, 0, val => Debug.Log(val), step:2) → 回调10→8→6→4→2→0
        /// 边界示例：Counter(0, 7, val => Debug.Log(val), step:3) → 回调0→3→6→7（最后一步自动修正为end）
        /// 取消示例：
        /// var disposable = RxExt.Counter(0,10, val => Debug.Log(val));
        /// disposable.Dispose(); // 触发canceled回调，终止计数（不会触发completed）
        /// 资源管理示例（Unity）：
        /// void Start()
        /// {
        ///     // 方式1：AddTo（推荐，自动在OnDestroy时释放）
        ///     RxExt.Counter(0,10, val => Debug.Log(val)).AddTo(this);
        ///     // 方式2：CompositeDisposable统一管理
        ///     private CompositeDisposable _disposables = new CompositeDisposable();
        ///     _disposables.Add(RxExt.Counter(0,10, val => Debug.Log(val)));
        /// }
        /// void OnDestroy()
        /// {
        ///     _disposables?.Dispose(); // 统一释放
        /// }
        /// </example>
        public static IDisposable Counter(
            int start,
            int end,
            Action<int> current,
            Action completed = null,
            Action canceled = null,
            uint step = 1,
            bool useMainThread = true,
            TimeSpan dueTime = default,
            TimeSpan period = default)
        {
            // 1. 严格入参校验（快速失败原则）
            if (start == end)
            {
                throw new ArgumentException($"起始值（{start}）不能等于结束值（{end}）", nameof(start));
            }
            if (current == null)
            {
                throw new ArgumentNullException(nameof(current), "计数回调不能为空");
            }
            if (step == 0)
            {
                throw new ArgumentException("步长必须大于0", nameof(step));
            }

            // 补全默认值并校验（简化写法，替代null判断）
            dueTime = dueTime == default ? TimeSpan.Zero : dueTime;
            if (dueTime < TimeSpan.Zero)
            {
                throw new ArgumentException($"首次执行延迟时间（{dueTime}）不能为负数", nameof(dueTime));
            }
            period = period == default ? TimeSpan.FromSeconds(1) : period;
            if (period <= TimeSpan.Zero)
            {
                throw new ArgumentException($"计数时间间隔（{period}）必须大于0", nameof(period));
            }

            // 2. 安全转换步长（uint→int，防止溢出）
            int stepInt;
            try
            {
                stepInt = checked((int)step);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException($"步长{step}超过int类型最大值（{int.MaxValue}），无法转换", ex);
            }

            // 3. 计算递增/递减的实际步长（带方向）
            int directionalStep = start < end ? stepInt : -stepInt;

            // 4. 计算总执行步数（整数向上取整，避免浮点精度丢失；确保覆盖到end值）
            long difference = (long)end - start;
            long absoluteDifference = Math.Abs(difference);
            long absoluteStep = stepInt;
            long totalStepsLong = (absoluteDifference + absoluteStep - 1) / absoluteStep;

            int totalSteps;
            try
            {
                totalSteps = checked((int)totalStepsLong);
            }
            catch (OverflowException ex)
            {
                throw new OverflowException($"总计数步数{totalStepsLong}超过int最大值（{int.MaxValue}），无法执行", ex);
            }

            // 5. UniRx核心计数逻辑：携带index，用于判断是否是最后一步
            var initialValue = Observable.Return((Value: start, IsLastStep: false)); // 初始值不是最后一步
            var stepValues = Observable
                .Timer(dueTime, period)
                .Take(totalSteps) // 步进值的次数为总步数
                .Select(index =>
                {
                    try
                    {
                        long stepMultiple = checked(directionalStep * (index + 1L));
                        long nextValueLong = checked(start + stepMultiple);
                        int nextValue = checked((int)nextValueLong);
                        // 判断是否是最后一步（index从0开始，最后一步index=totalSteps-1）
                        bool isLastStep = index == totalSteps - 1;
                        return (Value: nextValue, IsLastStep: isLastStep);
                    }
                    catch (OverflowException ex)
                    {
                        throw new OverflowException($"步进值计算溢出：start={start} + directionalStep={directionalStep} * (index+1)={index + 1} 超出int范围", ex);
                    }
                });

            var observable = initialValue.Concat(stepValues); // 合并：起始值 → 步进值

            // 6. 统一线程处理
            if (useMainThread)
            {
                observable = observable.ObserveOnMainThread();
            }

            // 7. 处理取消回调（异常捕获）
            observable = observable.DoOnCancel(() =>
            {
                try
                {
                    canceled?.Invoke();
                }
                catch (Exception ex)
                {
                    LogError($"Cancel回调执行异常（start={start}, end={end}, step={step}）", ex);
                }
            });

            // 8. 订阅回调（仅最后一步修正为end）
            return observable.Subscribe(
                onNext: tuple =>
                {
                    try
                    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                        if (!useMainThread && !MainThreadUtil.IsMainThread)
                        {
                            Debug.LogWarning($"[RxExt.Counter] useMainThread=false，当前回调在非主线程执行，若调用Unity API可能崩溃（start={start}, end={end}）");
                        }
#endif
                        // 仅最后一步修正为end，确保精准匹配
                        int finalVal = tuple.IsLastStep ? end : tuple.Value;
                        current.Invoke(finalVal);
                    }
                    catch (Exception ex)
                    {
                        LogError($"Current计数回调执行异常（当前值：{tuple.Value}，是否最后一步：{tuple.IsLastStep}，start={start}, end={end}, step={step}）", ex);
                    }
                },
                onError: ex =>
                {
                    LogError($"计数器执行异常（start={start}, end={end}, step={step}）", ex);
                },
                onCompleted: () =>
                {
                    try
                    {
                        completed?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        LogError($"Completed回调执行异常（start={start}, end={end}, step={step}）", ex);
                    }
                }
            );
        }

        /// <summary>
        /// 统一日志输出（适配Unity/非Unity环境）
        /// </summary>
        private static void LogError(string message, Exception ex)
        {
#if UNITY_ENGINE
#if UNITY_EDITOR
            Debug.LogError($"[RxExt.Counter] {message}：{ex}\n堆栈信息：{ex.StackTrace}");
#else
            Debug.LogError($"[RxExt.Counter] {message}：{ex.Message}");
#endif
#else
            Console.WriteLine($"[RxExt.Counter] {message}：{ex}\n堆栈信息：{ex.StackTrace}");
#endif
        }
    }
    #endregion

    #region 异步任务扩展
    public static partial class RxExt
    {
        /// <summary>
        /// 异步操作转Observable(只监听完成)
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IObservable<AsyncOperation> ToObservable(this AsyncOperation operation)
        {
            // 空检查
            if (operation == null)
            {
                return Observable.Throw<AsyncOperation>(new ArgumentNullException(nameof(operation)));
            }
            // 协程包装
            return Observable.FromCoroutine<AsyncOperation>(observer =>
            {
                IEnumerator WatchOperation()
                {
                    // 等待异步执行完毕
                    yield return operation;

                    // 执行完成
                    observer.OnNext(operation);
                    observer.OnCompleted();
                }

                return WatchOperation();
            });
        }

        /// <summary>
        /// 异步操作转Observable(只监听完成，无参数)
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IObservable<Unit> ToObservable_Complete(this AsyncOperation operation)
        {
            return operation.ToObservable().AsUnitObservable();
        }

        /// <summary>
        /// 异步操作转Observable(监听异步操作的实时进度：0~1)
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IObservable<float> ToObservable_Progress(this AsyncOperation operation)
        {
            if (operation == null)
            {
                return Observable.Throw<float>(new ArgumentNullException(nameof(operation)));
            }
            return Observable.FromCoroutine<float>(observer =>
            {
                IEnumerator WatchProgress()
                {
                    while (operation.isDone == false)
                    {
                        observer.OnNext(operation.progress);
                        // 等3帧，不用太频繁
                        yield return Observable.NextFrame().DelayFrame(2).ToYieldInstruction();
                    }

                    // 最后推送 100%
                    observer.OnNext(1f);
                    observer.OnCompleted();
                }

                return WatchProgress();
            });
        }
    }
    #endregion

    // 修复Unity主线程检测工具类
    internal static class MainThreadUtil
    {
        private static int _mainThreadId = -1;

        /// <summary>
        /// Unity启动时自动初始化主线程ID（确保在主线程执行）
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeMainThreadId()
        {
            _mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        public static int GetMainThreadId() => _mainThreadId;

        public static bool IsMainThread =>
            _mainThreadId != -1 &&
            System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThreadId;
    }
}