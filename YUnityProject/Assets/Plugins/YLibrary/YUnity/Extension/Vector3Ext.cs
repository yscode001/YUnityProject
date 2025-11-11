using UnityEngine;

namespace YUnity
{
    public static class Vector3Ext
    {
        /// <summary>
        /// 从当前点到目标点的方向(已归一化单位化)
        /// </summary>
        /// <param name="from">当前点，即起始点</param>
        /// <param name="to">目标点</param>
        /// <returns>从当前点到目标点的方向(已归一化单位化)</returns>
        public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }

        /// <summary>
        /// 从起始点到当前点的方向(已归一化单位化)
        /// </summary>
        /// <param name="to">当前点，即目标点</param>
        /// <param name="from">起始点</param>
        /// <returns>从起始点到当前点的方向(已归一化单位化)</returns>
        public static Vector3 DirectionFrom(this Vector3 to, Vector3 from)
        {
            return (to - from).normalized;
        }

        /// <summary>
        /// 先执行WorldToViewportPoint，再判断是否在相机的视口坐标范围内
        /// </summary>
        /// <param name="position"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsInCamera(this Vector3 position, Camera camera)
        {
            Vector3 wtvp = camera.WorldToViewportPoint(position);
            return wtvp.x >= 0 && wtvp.x <= 1 && wtvp.y >= 0 && wtvp.y <= 1 && wtvp.z >= camera.nearClipPlane && wtvp.z <= camera.farClipPlane;
        }
    }
}