// Author：yaoshuai
// Email：yscode@126.com
// Date：2024-5-24
// ------------------------------
using System;

namespace YUnity.Game.BlockPuzzle
{
    public partial class Block
    {
        public int X { get; private set; } = 0;
        public int ColIdx { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public int RowIdx { get; private set; } = 0;

        public bool IsEnable { get; private set; } = true;
        public uint HP { get; private set; } = 0;
        public FillType FillType { get; private set; } = FillType.Blue;
        public FillState FillState { get; private set; } = FillState.Normal;

        public bool IsDisable => !IsEnable;
        public bool IsEmpty => HP <= 0;
        public bool IsFilled => HP > 0;

        public event Action<uint, FillType, FillState> Event_FillValueChanged;
    }
    public partial class Block
    {
        public void Init(int rowIdx, int colIdx)
        {
            X = ColIdx = colIdx;
            Y = RowIdx = rowIdx;
            IsEnable = true;
            HP = 0;
            FillType = FillType.Blue;
            FillState = FillState.Normal;
        }
    }
    public partial class Block
    {
        public void SetupEnableState(bool enable)
        {
            IsEnable = enable;
        }
        public void SetupHP(uint hp)
        {
            if (HP != hp)
            {
                HP = hp;
                if (IsEmpty && FillState != FillState.Normal)
                {
                    FillState = FillState.Normal;
                }
                Event_FillValueChanged?.Invoke(HP, FillType, FillState);
            }
        }
        public void SetupFillType(FillType fillType)
        {
            if (FillType != fillType)
            {
                FillType = fillType;
                Event_FillValueChanged?.Invoke(HP, FillType, FillState);
            }
        }
        public void SetupFillState(FillState fillState)
        {
            FillState newState = IsEmpty ? FillState.Normal : fillState;
            if (FillState != newState)
            {
                FillState = newState;
                Event_FillValueChanged?.Invoke(HP, FillType, FillState);
            }
        }
        public void SetupFillData(uint hp, FillType fillType, FillState fillState)
        {
            bool isChanged = false;
            if (HP != hp)
            {
                HP = hp;
                isChanged = true;
            }
            if (FillType != fillType)
            {
                FillType = fillType;
                isChanged = true;
            }
            FillState newState = IsEmpty ? FillState.Normal : fillState;
            if (FillState != newState)
            {
                FillState = newState;
                isChanged = true;
            }
            if (isChanged)
            {
                Event_FillValueChanged?.Invoke(HP, FillType, FillState);
            }
        }
    }
    public partial class Block
    {
        /// <summary>
        /// 正常消除，生命值减1
        /// </summary>
        public void Eliminate()
        {
            if (HP > 0)
            {
                HP -= 1;
                Event_FillValueChanged?.Invoke(HP, FillType, FillState);
            }
        }
    }
}