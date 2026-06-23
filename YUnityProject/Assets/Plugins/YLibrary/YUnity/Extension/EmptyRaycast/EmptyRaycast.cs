using UnityEngine;
using UnityEngine.UI;

namespace YUnity
{
    /// <summary>
    /// 无渲染隐形点击热区，完全替代 UGUI2.5.0+ RaycastReceiver
    /// 零DrawCall、不参与合批、最低射线检测开销、兼容Mask/RectMask2D
    /// </summary>
    public class EmptyRaycast : MaskableGraphic
    {
        // 缓存默认UI材质，全局只实例化一次
        private static Material _uiDefaultMat;

        /// <summary>
        /// 清空顶点，完全不生成渲染面片
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }

        /// <summary>
        /// 缓存复用默认UI材质，极简写法减少分支
        /// </summary>
        public override Material material
        {
            get => _uiDefaultMat ?? (_uiDefaultMat = defaultMaterial);
        }

        /// <summary>
        /// 优化：不需要贴图，跳过纹理更新逻辑
        /// </summary>
        public override Texture mainTexture => null;

        /// <summary>
        /// 优化：无颜色渲染，修改颜色不会触发网格重生成
        /// </summary>
        public override Color color
        {
            get => Color.white;
            set { }
        }

        /// <summary>
        /// 优化：无射线内边距，隐藏基类偏移计算（new隐藏，非override，无报错）
        /// </summary>
        public new Vector4 raycastPadding => Vector4.zero;

        // ========== 阻断脏标记刷新，杜绝无用UI重绘（这两个是virtual可安全override） ==========
        public override void SetVerticesDirty() { }
        public override void SetMaterialDirty() { }

        // ========== 编辑器可视化辅助（仅编辑模式生效，打包自动移除） ==========
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            RectTransform rt = rectTransform;
            if (rt == null) return;

            Gizmos.color = new Color(1f, 1f, 1f, 0.3f);
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);
        }
#endif
    }
}