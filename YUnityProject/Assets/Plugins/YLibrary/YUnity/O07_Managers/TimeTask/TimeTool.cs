namespace YUnity
{
    /// <summary>
    /// 定时任务工具
    /// </summary>
    public class TimeTool
    {
        private TimeTool() { }

        /// <summary>
        /// 获取延时的毫秒数
        /// </summary>
        /// <param name="delay">延时的数值</param>
        /// <param name="unit">延时数值得单位</param>
        /// <returns></returns>
        public static double GetMillisecond(double delay, TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Millisecond:
                    return delay;
                case TimeUnit.Second:
                    return delay * 1000;
                case TimeUnit.Minute:
                    return delay * 60 * 1000;
                case TimeUnit.Hour:
                    return delay * 60 * 60 * 1000;
                case TimeUnit.Day:
                    return delay * 24 * 60 * 60 * 1000;
                default:
                    return 0;
            }
        }
    }
}