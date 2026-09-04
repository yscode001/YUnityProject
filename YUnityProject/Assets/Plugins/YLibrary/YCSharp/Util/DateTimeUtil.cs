using System;

namespace YCSharp
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// <para>当前 UTC 时间对应的 Unix 时间戳（毫秒)</para>
        /// <para>DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()</para>
        /// </summary>
        public static long GetUTCMilliseconds() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        /// <summary>
        /// <para>当前 UTC 时间对应的 Unix 时间戳（秒)</para>
        /// <para>DateTimeOffset.UtcNow.ToUnixTimeSeconds()</para>
        /// </summary>
        public static long GetUTCSeconds() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}