// Author：yaoshuai
// Email：yscode@126.com
// Date：2025-7-29
// ------------------------------

namespace YUnity.Game
{
    /// <summary>
    /// 战斗状态
    /// </summary>
    public enum FightState
    {
        /// <summary>
        /// 默认状态，初始化时的状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// 战斗准备阶段
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 战斗中
        /// </summary>
        Fighting = 2,

        /// <summary>
        /// 战斗结束
        /// </summary>
        Over = 3,
    }
}