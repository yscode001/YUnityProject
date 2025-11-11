// Author：yaoshuai
// Email：yscode@126.com
// Date：2023-10-20
// ------------------------------
using System;
using UnityEngine;

namespace YUnity
{
    public static class MathfUtil
    {
        /// <summary>
        /// 获取点到直线的距离
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="straightLineStart">直线开始点</param>
        /// <param name="straightLineEnd">直线结束点</param>
        /// <returns>获取点到直线的距离</returns>
        public static float GetPointToStraightLineDistance(Vector3 point, Vector3 straightLineStart, Vector3 straightLineEnd)
        {
            Vector3 sp = point - straightLineStart;
            Vector3 se = straightLineEnd - straightLineStart;
            Vector3 project = Vector3.Project(sp, se);
            Vector3 line = sp - project;
            return line.magnitude;
        }

        /// <summary>
        /// 获取点到线段的距离
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="lineSegmentStart">线段开始点</param>
        /// <param name="lineSegmentEnd">线段结束点</param>
        /// <returns>如果点到线段的投影在线段之外，返回false；否则返回true</returns>
        public static Tuple<bool, float> GetPointToLineSegmentDistance(Vector3 point, Vector3 lineSegmentStart, Vector3 lineSegmentEnd)
        {
            Vector3 sp = point - lineSegmentStart;
            Vector3 se = lineSegmentEnd - lineSegmentStart;
            Vector3 project = Vector3.Project(sp, se);
            if (se.magnitude >= project.magnitude)
            {
                Vector3 line = sp - project;
                return new Tuple<bool, float>(true, line.magnitude);
            }
            else
            {
                return new Tuple<bool, float>(false, 0);
            }
        }
    }
}