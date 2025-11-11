// Author：yaoshuai
// Email：yscode@126.com
// Date：2025-7-29
// ------------------------------

namespace YUnity.Game
{
    /// <summary>
    /// 奖励
    /// </summary>
    public struct RewardModel
    {
        /// <summary>
        /// 奖励类型
        /// </summary>
        public int Type { get; private set; }

        /// <summary>
        /// 奖励ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 奖励数量
        /// </summary>
        public int Count { get; private set; }

        public RewardModel(int type, int id, int count)
        {
            Type = type;
            ID = id;
            Count = count;
        }
    }
}