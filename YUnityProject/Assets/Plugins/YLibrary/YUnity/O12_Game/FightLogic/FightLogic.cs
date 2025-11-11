// Author：yaoshuai
// Email：yscode@126.com
// Date：2025-7-29
// ------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YUnity.Game
{
    public partial class FightLogic : MonoBehaviourBaseY
    {
        public static FightLogic Instance { get; private set; } = null;

        /// <summary>
        /// 游戏玩法
        /// </summary>
        public int GamePlay { get; private set; } = 0;

        /// <summary>
        /// 战斗状态
        /// </summary>
        public FightState FightState { get; private set; } = FightState.Default;

        /// <summary>
        /// 战斗状态
        /// </summary>
        public event Action Event_FightStateChanged;

        /// <summary>
        /// 战斗结果
        /// </summary>
        public FightResult FightResult { get; private set; } = FightResult.Draw;

        /// <summary>
        /// 战斗结果原因
        /// </summary>
        public string FightResultReason { get; private set; } = null;

        /// <summary>
        /// 战斗奖励
        /// </summary>
        public List<RewardModel> FightRewards { get; private set; } = null;

        /// <summary>
        /// 战斗结果
        /// </summary>
        public event Action Event_FightResultChanged;

        /// <summary>
        /// 战前准备剩余秒数(用于UI显示：3、2、1、开始战斗)，0表示战斗开始了(开始战斗UI应该消失)
        /// </summary>
        public event Action<int> Event_ReadySecondsChanged;

        private int ReadyTotalSeconds = 0;
        private int ReadyCurSeconds = 0;
        private float ReadyDeltaSeconds = 0;
    }
    public partial class FightLogic
    {
        private void Update()
        {
            if (FightState == FightState.Ready && ReadyTotalSeconds > 0)
            {
                ReadyDeltaSeconds += Time.deltaTime;
                if (ReadyDeltaSeconds >= 1)
                {
                    ReadyDeltaSeconds = 0;
                    ReadyCurSeconds += 1;
                    if (ReadyCurSeconds >= ReadyTotalSeconds)
                    {
                        Begin();
                    }
                    else
                    {
                        Event_ReadySecondsChanged?.Invoke(ReadyTotalSeconds - ReadyCurSeconds);
                    }
                }
            }
        }
    }
    public partial class FightLogic
    {
        private FightLogic() { }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(Instance);
                Instance = null;
            }
            Instance = this;

            GamePlay = 0;
            ReadyTotalSeconds = 0;
            ReadyCurSeconds = 0;
            ReadyDeltaSeconds = 0;
            FightState = FightState.Default;
            FightResult = FightResult.Draw;
            FightResultReason = null;
            FightRewards = null;

            // 状态、准备倒计时
            Event_FightStateChanged?.Invoke();
            Event_ReadySecondsChanged?.Invoke(ReadyTotalSeconds);
        }

        /// <summary>
        /// 战前准备，剩余秒数(用于UI显示：3、2、1、开始战斗)，0表示战斗开始了(开始战斗UI应该消失)
        /// </summary>
        /// <param name="readyTotalSeconds">战前准备剩余秒数(用于UI显示：3、2、1、开始战斗)，0表示战斗开始了(开始战斗UI应该消失)</param>
        /// <exception cref="Exception"></exception>
        public void Ready(int gamePlay, int readyTotalSeconds)
        {
            if (gamePlay < 0 || readyTotalSeconds <= 0)
            {
                throw new Exception("gamePlay需大于等于0，readyTotalSeconds需大于0");
            }
            GamePlay = gamePlay;
            ReadyTotalSeconds = readyTotalSeconds;
            ReadyCurSeconds = 0;
            ReadyDeltaSeconds = 0;
            FightState = FightState.Ready;
            FightResult = FightResult.Draw;
            FightResultReason = null;
            FightRewards = null;

            // 状态、准备倒计时
            Event_FightStateChanged?.Invoke();
            Event_ReadySecondsChanged?.Invoke(ReadyTotalSeconds);
        }

        private void Begin()
        {
            ReadyTotalSeconds = 0;
            ReadyCurSeconds = 0;
            ReadyDeltaSeconds = 0;
            FightState = FightState.Fighting;
            FightResult = FightResult.Draw;
            FightResultReason = null;
            FightRewards = null;

            // 状态、准备倒计时
            Event_FightStateChanged?.Invoke();
            Event_ReadySecondsChanged?.Invoke(ReadyCurSeconds);
        }

        /// <summary>
        /// 战斗结束
        /// </summary>
        /// <param name="fightResult"></param>
        /// <param name="fightResultReason"></param>
        /// <param name="rewards"></param>
        public void Over(FightResult fightResult, string fightResultReason, List<RewardModel> rewards)
        {
            ReadyTotalSeconds = 0;
            ReadyCurSeconds = 0;
            ReadyDeltaSeconds = 0;
            FightState = FightState.Over;
            FightResult = fightResult;
            FightResultReason = fightResultReason;
            FightRewards = rewards;

            // 状态、结果
            Event_FightStateChanged?.Invoke();
            Event_FightResultChanged?.Invoke();
        }
    }
}