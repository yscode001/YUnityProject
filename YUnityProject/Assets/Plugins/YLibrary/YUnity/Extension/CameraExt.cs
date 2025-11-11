using UnityEngine;

namespace YUnity
{
    public static class CameraExt
    {
        /// 打开或关闭层
        public static void CullingMaskOpenOrClose(this Camera camera, bool open, string cullingMask)
        {
            int layer = LayerMask.NameToLayer(cullingMask);
            if (layer < 0) { return; }
            if (open)
            {
                camera.cullingMask |= (1 << layer);
            }
            else
            {
                camera.cullingMask &= ~(1 << layer);
            }
        }

        /// 显示所有层，除了指定的层
        public static void CullingMaskShowAllExcept(this Camera camera, string cullingMask)
        {
            int layer = LayerMask.NameToLayer(cullingMask);
            if (layer < 0) { return; }
            camera.cullingMask = ~(1 << layer);
        }

        /// 只显示指定的层
        public static void CullingMaskOnlyShow(this Camera camera, string cullingMask)
        {
            int layer = LayerMask.NameToLayer(cullingMask);
            if (layer < 0) { return; }
            camera.cullingMask = 1 << layer;
        }
    }
}