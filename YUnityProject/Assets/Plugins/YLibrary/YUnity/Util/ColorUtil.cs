// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-6-17
// ------------------------------

using UnityEngine;

namespace YUnity
{
    public class ColorUtil
    {
        /// <summary>
        /// 创建颜色
        /// </summary>
        /// <param name="r">0 - 255</param>
        /// <param name="g">0 - 255</param>
        /// <param name="b">0 - 255</param>
        /// <returns></returns>
        public static Color Color(int r, int g, int b)
        {
            int rc = Mathf.Clamp(r, 0, 255);
            int gc = Mathf.Clamp(g, 0, 255);
            int bc = Mathf.Clamp(b, 0, 255);

            float rv = rc * 1.0f / 255.0f;
            float gv = gc * 1.0f / 255.0f;
            float bv = bc * 1.0f / 255.0f;
            return new Color(rv, gv, bv);
        }

        /// <summary>
        /// 创建颜色
        /// </summary>
        /// <param name="r">0 - 255</param>
        /// <param name="g">0 - 255</param>
        /// <param name="b">0 - 255</param>
        /// <param name="a">0 - 1</param>
        /// <returns></returns>
        public static Color Color(int r, int g, int b, float a)
        {
            int rc = Mathf.Clamp(r, 0, 255);
            int gc = Mathf.Clamp(g, 0, 255);
            int bc = Mathf.Clamp(b, 0, 255);
            float ac = Mathf.Clamp(a, 0, 1);

            float rv = rc * 1.0f / 255.0f;
            float gv = gc * 1.0f / 255.0f;
            float bv = bc * 1.0f / 255.0f;
            return new Color(rv, gv, bv, ac);
        }
    }
}