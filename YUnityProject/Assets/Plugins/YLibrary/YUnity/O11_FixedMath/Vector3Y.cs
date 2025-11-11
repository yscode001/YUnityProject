using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 确定性的向量运算
    /// </summary>
    public struct Vector3Y
    {
        #region 属性与访问器
        public IntY x, y, z;
        /// <summary>
        /// 访问器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IntY this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: break;
                }
            }
        }
        #endregion
        #region 构造函数
        public Vector3Y(IntY x, IntY y, IntY z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3Y(float x, float y, float z)
        {
            this.x = (IntY)x;
            this.y = (IntY)y;
            this.z = (IntY)z;
        }
        public Vector3Y(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3Y(Vector3 vector3)
        {
            x = (IntY)vector3.x;
            y = (IntY)vector3.y;
            z = (IntY)vector3.z;
        }
        #endregion
        #region 显示转换
        /// <summary>
        /// float必须显式转换，会损失精度
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator Vector3Y(Vector3 vector3)
        {
            return new Vector3Y(vector3);
        }
        #endregion
        #region 常用向量
        public static Vector3Y Zero { get { return new Vector3Y(0, 0, 0); } }
        public static Vector3Y One { get { return new Vector3Y(1, 1, 1); } }

        public static Vector3Y Forward { get { return new Vector3Y(0, 0, 1); } }
        public static Vector3Y Back { get { return new Vector3Y(0, 0, -1); } }
        public static Vector3Y Left { get { return new Vector3Y(-1, 0, 0); } }
        public static Vector3Y Right { get { return new Vector3Y(1, 0, 0); } }
        public static Vector3Y Up { get { return new Vector3Y(0, 1, 0); } }
        public static Vector3Y Down { get { return new Vector3Y(0, -1, 0); } }
        #endregion
        #region 常用重写
        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", x, y, z);
        }
        public override int GetHashCode()
        {
            return x.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            Vector3Y val = (Vector3Y)obj;
            return val.x == x && val.y == y && val.z == z;
        }
        #endregion
        #region 返回x,y,z缩放后的值
        /// <summary>
        /// 返回x,y,z缩放后的值
        /// </summary>
        /// <returns></returns>
        public long[] ScaledValues()
        {
            return new long[] { x.ScaledValue, y.ScaledValue, z.ScaledValue };
        }
        #endregion
        #region 原始值
        /// <summary>
        /// UnityVector3原始值，可进行显示，但不可参与逻辑运算
        /// </summary>
        public Vector3 RawVector3 { get { return new Vector3(x.RawFloat, y.RawFloat, z.RawFloat); } }
        /// <summary>
        /// 浮点型原始值，可进行显示，但不可参与逻辑运算
        /// </summary>
        public float RawXFloat => x.RawFloat;
        /// <summary>
        /// 浮点型原始值，可进行显示，但不可参与逻辑运算
        /// </summary>
        public float RawYFloat => y.RawFloat;
        /// <summary>
        /// 浮点型原始值，可进行显示，但不可参与逻辑运算
        /// </summary>
        public float RawZFloat => z.RawFloat;
        /// <summary>
        /// 浮点型原始值，可进行显示，但不可参与逻辑运算
        /// </summary>
        public float[] RawFloatValues { get { return new float[] { RawXFloat, RawYFloat, RawZFloat }; } }
        /// <summary>
        /// 整形原始值，可进行显示，也可参与逻辑运算
        /// </summary>
        public int RawXInt => x.RawInt;
        /// <summary>
        /// 整形原始值，可进行显示，也可参与逻辑运算
        /// </summary>
        public int RawYInt => y.RawInt;
        /// <summary>
        /// 整形原始值，可进行显示，也可参与逻辑运算
        /// </summary>
        public int RawZInt => z.RawInt;
        /// <summary>
        /// 整形原始值，可进行显示，也可参与逻辑运算
        /// </summary>
        public int[] RawIntValues { get { return new int[] { RawXInt, RawYInt, RawZInt }; } }
        #endregion
        #region 逻辑运算符
        public static Vector3Y operator -(Vector3Y a)
        {
            return new Vector3Y(-a.x, -a.y, -a.z);
        }
        public static Vector3Y operator +(Vector3Y a, Vector3Y b)
        {
            return new Vector3Y(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3Y operator -(Vector3Y a, Vector3Y b)
        {
            return new Vector3Y(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3Y operator *(Vector3Y a, IntY value)
        {
            return new Vector3Y(a.x * value, a.y * value, a.z * value);
        }
        public static Vector3Y operator *(IntY value, Vector3Y a)
        {
            return new Vector3Y(a.x * value, a.y * value, a.z * value);
        }
        public static Vector3Y operator /(Vector3Y a, IntY value)
        {
            return new Vector3Y(a.x / value, a.y / value, a.z / value);
        }
        #endregion
        #region 关系运算符
        public static bool operator ==(Vector3Y a, Vector3Y b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3Y a, Vector3Y b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
        #endregion
        #region 向量长度及长度的平方值
        /// <summary>
        /// 当前向量长度的平方值
        /// </summary>
        public IntY SqrMagnitude => x * x + y * y + z * z;
        /// <summary>
        /// 求向量长度的平方值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IntY GetSqrMagnitude(Vector3Y value)
        {
            return value.SqrMagnitude;
        }
        /// <summary>
        /// 当前向量的长度
        /// </summary>
        public IntY Magnitude => CalculateY.Sqrt(SqrMagnitude);
        /// <summary>
        /// 求向量的长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IntY GetMagnitude(Vector3Y value)
        {
            return CalculateY.Sqrt(value.SqrMagnitude);
        }
        #endregion
        #region 向量归一化
        /// <summary>
        /// 返回当前向量的单位向量
        /// </summary>
        public Vector3Y Normalized
        {
            get
            {
                if (Magnitude > 0)
                {
                    IntY rate = IntY.One / Magnitude;
                    return new Vector3Y(x * rate, y * rate, z * rate);
                }
                else
                {
                    return Vector3Y.Zero;
                }
            }
        }
        /// <summary>
        /// 求向量的单位向量
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3Y GetNormalized(Vector3Y value)
        {
            return value.Normalized;
        }
        /// <summary>
        /// 将当前向量变为单位向量
        /// </summary>
        public void Normalize()
        {
            IntY rate = IntY.One / Magnitude;
            x *= rate;
            y *= rate;
            z *= rate;
        }
        #endregion
        #region 点积与叉积
        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IntY Dot(Vector3Y value)
        {
            return x * value.x + y * value.y + z * value.z;
        }
        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IntY Dot(Vector3Y a, Vector3Y b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }
        /// <summary>
        /// 叉积
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Vector3Y Cross(Vector3Y value)
        {
            return new Vector3Y(y * value.z - z * value.y, z * value.x - x * value.z, x * value.y - y * value.x);
        }
        /// <summary>
        /// 叉积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3Y Cross(Vector3Y a, Vector3Y b)
        {
            return new Vector3Y(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }
        #endregion
        #region 夹角
        /// <summary>
        /// 2个向量之间的弧度值
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static ArgsY Angle(Vector3Y from, Vector3Y to)
        {
            IntY dot = Dot(from, to);
            IntY mod = from.Magnitude * to.Magnitude;
            if (mod == 0) { return ArgsY.Zero; }
            IntY value = dot / mod;
            // 反余弦函数计算
            return CalculateY.Acos(value);
        }
        /// <summary>
        /// 2个向量之间的弧度值
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ArgsY AngleFromThisToValue(Vector3Y to)
        {
            return Angle(this, to);
        }
        /// <summary>
        /// 2个向量之间的弧度值
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ArgsY AngleFromValueToThis(Vector3Y from)
        {
            return Angle(from, this);
        }
        #endregion
    }
}