/*
 无需经常修改的配置，基本配完就不动了
 */

/// <summary>
/// 环境配置
/// </summary>
public enum EnvEnum
{
    /// <summary>
    /// 开发环境
    /// </summary>
    develop,

    /// <summary>
    /// 线上环境
    /// </summary>
    online,
}
public static partial class AppCfg
{
    /// <summary>
    /// 默认帧率(60)
    /// </summary>
    public const int TargetFrameRateDefault = 60;

    /// <summary>
    /// 是否是开发环境
    /// </summary>
    public static bool IsDevelop => envEnum == EnvEnum.develop;

    /// <summary>
    /// 是否是线上环境
    /// </summary>
    public static bool IsOnline => envEnum == EnvEnum.online;
}