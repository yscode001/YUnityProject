public static partial class AppCfg
{
    /// <summary>
    /// 环境配置
    /// </summary>
    private const EnvEnum envEnum = EnvEnum.online;

    /// <summary>
    /// 是否开启日志打印
    /// </summary>
    public static bool IsEnableLog => IsDevelop;

    /// <summary>
    /// 是否开启Debug面板调试
    /// </summary>
    public static bool IsEnableDebugPanel => IsDevelop;

    /// <summary>
    /// 默认帧率
    /// </summary>
    public const int TargetFrameRateDefault = 60;
}