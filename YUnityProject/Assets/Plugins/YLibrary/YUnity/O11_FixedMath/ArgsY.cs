using System;

namespace YUnity
{
    /// <summary>
    /// 运算参数
    /// </summary>
    public struct ArgsY
    {
        #region 属性及构造函数
        /// <summary>
        /// 缩放后的值
        /// </summary>
        public int ScaledValue { get; private set; }

        /// <summary>
        /// 放大倍数
        /// </summary>
        public uint Multipler { get; private set; }

        /// <summary>
        /// 浮点型原始值，可进行显示，但不可参与逻辑运算
        /// </summary>
        /// <returns></returns>
        public float RawFloatValue => ScaledValue * 1.0f / Multipler;

        /// <summary>
        /// 将当前的值(弧度值)转换为角度值，不可再用于逻辑运算
        /// </summary>
        /// <returns></returns>
        public int AngleValue => (int)Math.Round(RawFloatValue / Math.PI * 180);

        public ArgsY(int scaledValue, uint multipler)
        {
            ScaledValue = scaledValue;
            Multipler = multipler;
        }
        #endregion
        #region 常用重写
        public override string ToString()
        {
            return string.Format("scaledValue:{0} multipler:{1}", ScaledValue, Multipler);
        }
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            ArgsY val = (ArgsY)obj;
            if (Multipler == val.Multipler) { return ScaledValue == val.ScaledValue; }
            throw new Exception("multipler is unequal");
        }
        public override int GetHashCode()
        {
            return ScaledValue.GetHashCode();
        }
        #endregion
        #region 常用值
        public static ArgsY Zero { get { return new ArgsY(0, 10000); } }
        public static ArgsY One { get { return new ArgsY(1, 10000); } }
        public static ArgsY PI_HALF { get { return new ArgsY(15708, 10000); } }
        public static ArgsY PI { get { return new ArgsY(31416, 10000); } }
        public static ArgsY PI_TWO { get { return new ArgsY(62832, 10000); } }
        #endregion
        #region 关系运算符
        public static bool operator ==(ArgsY a, ArgsY b)
        {
            if (a.Multipler == b.Multipler) { return a.ScaledValue == b.ScaledValue; }
            throw new Exception("multipler is unequal");
        }
        public static bool operator !=(ArgsY a, ArgsY b)
        {
            if (a.Multipler == b.Multipler) { return a.ScaledValue != b.ScaledValue; }
            throw new Exception("multipler is unequal");
        }
        public static bool operator >(ArgsY a, ArgsY b)
        {
            if (a.Multipler == b.Multipler) { return a.ScaledValue > b.ScaledValue; }
            throw new Exception("multipler is unequal");
        }
        public static bool operator >=(ArgsY a, ArgsY b)
        {
            if (a.Multipler == b.Multipler) { return a.ScaledValue >= b.ScaledValue; }
            throw new Exception("multipler is unequal");
        }
        public static bool operator <(ArgsY a, ArgsY b)
        {
            if (a.Multipler == b.Multipler) { return a.ScaledValue < b.ScaledValue; }
            throw new Exception("multipler is unequal");
        }
        public static bool operator <=(ArgsY a, ArgsY b)
        {
            if (a.Multipler == b.Multipler) { return a.ScaledValue <= b.ScaledValue; }
            throw new Exception("multipler is unequal");
        }
        #endregion
    }
}