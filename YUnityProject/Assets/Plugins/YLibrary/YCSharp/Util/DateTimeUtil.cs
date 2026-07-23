using System;

namespace YCSharp
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// 当前 UTC 时间对应的 Unix 时间戳（毫秒)
        /// DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        /// </summary>
        public static long GetUTCMilliseconds() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        /// <summary>
        /// 当前 UTC 时间对应的 Unix 时间戳（秒)
        /// DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        /// </summary>
        public static long GetUTCSeconds() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}