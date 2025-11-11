using System;

namespace YUnity
{
    /// <summary>
    /// 常用定点数数学运算
    /// </summary>
    public class CalculateY
    {
        /// <summary>
        /// 求定点数的平方根
        /// </summary>
        /// <param name="value"></param>
        /// <param name="interator">迭代次数</param>
        /// <returns></returns>
        public static IntY Sqrt(IntY value, uint interatorCount = 8)
        {
            if (value == IntY.Zero) { return 0; }
            if (value < IntY.Zero) { throw new Exception(); }
            IntY result = value;
            IntY history;
            uint count = 0;
            do
            {
                history = result;
                result = (result + value / result) >> 1;
                count += 1;
            } while (history != result && count < interatorCount);
            return result;
        }

        /// <summary>
        /// 根据弧度返回反余弦角度值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ArgsY Acos(IntY value)
        {
            IntY rate = (value * AcosTable.halfIndexCount) + AcosTable.halfIndexCount;
            rate = Clamp(rate, IntY.Zero, AcosTable.indexCount);
            return new ArgsY(AcosTable.table[rate.RawInt], AcosTable.multipler);
        }

        /// <summary>
        /// 数值的约束
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IntY Clamp(IntY input, IntY min, IntY max)
        {
            if (input < min) { return min; }
            if (input > max) { return max; }
            return input;
        }
    }
}