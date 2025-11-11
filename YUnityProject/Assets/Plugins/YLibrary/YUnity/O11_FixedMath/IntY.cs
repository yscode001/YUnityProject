using System;

namespace YUnity
{
    /// <summary>
    /// 定点数
    /// </summary>
    public struct IntY
    {
        #region 属性
        // 移位计数
        private const int bit_move_count = 10;

        // 放大倍数
        private const long multiplier_factory = 1 << bit_move_count;

        private long scaledValue;

        /// <summary>
        /// 缩放后的值
        /// </summary>
        public long ScaledValue
        {
            get { return scaledValue; }
            set { scaledValue = value; }
        }
        #endregion

        #region 构造函数
        // 内部使用，已经缩放过的数据
        private IntY(long scaledValue)
        {
            this.scaledValue = scaledValue;
        }
        public IntY(int value)
        {
            scaledValue = value << bit_move_count;
        }
        public IntY(float value)
        {
            scaledValue = (long)Math.Round(value * multiplier_factory);
        }
        #endregion
        #region 隐式转换与显式转换
        /// <summary>
        /// int可以隐式转换，不会损失精度
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator IntY(int value)
        {
            return new IntY(value);
        }
        /// <summary>
        /// float必须显式转换，会损失精度
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator IntY(float value)
        {
            return new IntY((long)Math.Round(value * multiplier_factory));
        }
        #endregion
        #region 常用数值
        public static readonly IntY Zero = 0;
        public static readonly IntY One = 1;
        #endregion
        #region 逻辑运算符
        public static IntY operator -(IntY value)
        {
            return new IntY(-value.scaledValue);
        }
        public static IntY operator +(IntY a, IntY b)
        {
            return new IntY(a.scaledValue + b.scaledValue);
        }
        public static IntY operator -(IntY a, IntY b)
        {
            return new IntY(a.scaledValue - b.scaledValue);
        }
        public static IntY operator *(IntY a, IntY b)
        {
            long value = a.scaledValue * b.scaledValue;
            if (value >= 0) { return new IntY(value >> bit_move_count); }
            else { return new IntY(-(-value >> bit_move_count)); }
        }
        public static IntY operator /(IntY a, IntY b)
        {
            if (b.scaledValue == 0) { throw new Exception(); }
            return new IntY((a.scaledValue << bit_move_count) / b.scaledValue);
        }
        public static IntY operator %(IntY a, IntY b)
        {
            if (b.scaledValue == 0) { throw new Exception(); }
            return new IntY(a.RawInt % b.RawInt);
        }
        public static IntY operator <<(IntY value, int moveCount)
        {
            return new IntY(value.scaledValue << moveCount);
        }
        public static IntY operator >>(IntY value, int moveCount)
        {
            if (value.scaledValue >= 0) { return new IntY(value.scaledValue >> moveCount); }
            else { return new IntY(-(-value.scaledValue >> moveCount)); }
        }
        #endregion
        #region 关系运算符
        public static bool operator ==(IntY a, IntY b)
        {
            return a.scaledValue == b.scaledValue;
        }
        public static bool operator !=(IntY a, IntY b)
        {
            return a.scaledValue != b.scaledValue;
        }
        public static bool operator >(IntY a, IntY b)
        {
            return a.scaledValue > b.scaledValue;
        }
        public static bool operator >=(IntY a, IntY b)
        {
            return a.scaledValue >= b.scaledValue;
        }
        public static bool operator <(IntY a, IntY b)
        {
            return a.scaledValue < b.scaledValue;
        }
        public static bool operator <=(IntY a, IntY b)
        {
            return a.scaledValue <= b.scaledValue;
        }
        #endregion
        #region 原始值
        /// <summary>
        /// 浮点型原始值，可进行显示，但不可参与逻辑运算
        /// </summary>
        public float RawFloat
        {
            get { return scaledValue * 1.0f / multiplier_factory; }
        }
        /// <summary>
        /// 整形原始值，可进行显示，也可参与逻辑运算
        /// </summary>
        public int RawInt
        {
            get
            {
                if (scaledValue >= 0) { return (int)(scaledValue >> bit_move_count); }
                else { return -(int)(-scaledValue >> bit_move_count); }
            }
        }
        #endregion
        #region 常用重写
        public override string ToString()
        {
            return RawFloat.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            return scaledValue == ((IntY)obj).scaledValue;
        }
        public override int GetHashCode()
        {
            return scaledValue.GetHashCode();
        }
        #endregion
        #region 数值约束
        public static IntY Clamp(IntY input, IntY min, IntY max)
        {
            if (input < min) { return min; }
            if (input > max) { return max; }
            return input;
        }
        public IntY Clamp(IntY min, IntY max)
        {
            if (this < min) { return min; }
            if (this > max) { return max; }
            return this;
        }
        #endregion
    }
}