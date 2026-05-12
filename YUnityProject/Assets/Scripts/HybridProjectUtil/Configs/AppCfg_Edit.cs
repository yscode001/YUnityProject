/*
 需要经常修改的配置，比如：打各种配置的包时
 */

public static partial class AppCfg
{
    /// <summary>
    /// 环境配置
    /// </summary>
    private const EnvEnum envEnum = EnvEnum.develop;

    /// <summary>
    /// 是否开启日志打印
    /// </summary>
    public static bool IsEnableLog => IsDevelop;

    /// <summary>
    /// 是否开启Debug面板调试
    /// </summary>
    public static bool IsEnableDebugPanel => IsDevelop;
}