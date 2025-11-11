using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    #region 属性及设置数据
    public partial class BlendShapeUtil
    {
        public SkinnedMeshRenderer TargetSMR { get; private set; } = null;
        public float MinValue { get; private set; } = 0;
        public float MaxValue { get; private set; } = 0;

        /// <summary>
        /// Key是名字，Value是索引
        /// </summary>
        public Dictionary<string, int> BlendShapes { get; private set; } = new Dictionary<string, int>();

        public void SetupData(SkinnedMeshRenderer targetSMR, float minValue, float maxValue)
        {
            if (targetSMR == null) { return; }
            TargetSMR = targetSMR;
            MinValue = minValue;
            MaxValue = maxValue;
            Mesh mesh = targetSMR.sharedMesh;

            BlendShapes = new Dictionary<string, int>();
            for (int i = 0; i < mesh.blendShapeCount; i++)
            {
                BlendShapes.Add(mesh.GetBlendShapeName(i), i);
            }
        }
    }
    #endregion

    public partial class BlendShapeUtil
    {
        /// 成功返回空字符串，出错返回错误信息
        public string SetBlendShapeValue(string blendShapeName, float value)
        {
            if (string.IsNullOrWhiteSpace(blendShapeName))
            {
                return "要修改的BlendShape名字未空，无法修改";
            }
            if (TargetSMR == null)
            {
                return "您还未设置SkinnedMeshRenderer数据，无法修改";
            }
            if (BlendShapes.TryGetValue(blendShapeName, out int idx))
            {
                float editValue = Mathf.Clamp(value, MinValue, MaxValue);
                TargetSMR.SetBlendShapeWeight(idx, editValue);
                return "";
            }
            else
            {
                return $"不存在名为{blendShapeName}的Blend，无法修改";
            }
        }

        /// 出错或默认为0
        public float GetBlendShapeValue(string blendShapeName)
        {
            if (string.IsNullOrWhiteSpace(blendShapeName))
            {
                return 0;
            }
            if (TargetSMR == null)
            {
                return 0;
            }
            if (BlendShapes.TryGetValue(blendShapeName, out int idx))
            {
                return TargetSMR.GetBlendShapeWeight(idx);
            }
            return 0;
        }
    }
}