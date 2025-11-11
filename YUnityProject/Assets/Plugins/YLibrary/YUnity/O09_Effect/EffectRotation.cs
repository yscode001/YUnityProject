// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-1-12
// ------------------------------

using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 旋转特效
    /// </summary>
    public partial class EffectRotation : MonoBehaviourBaseY
    {
        [Header("旋转速度")]
        [SerializeField] private float RotationSpeedX;
        [SerializeField] private float RotationSpeedY;
        [SerializeField] private float RotationSpeedZ;

        [Header("是否允许播放")]
        [SerializeField] private bool IsAllowPlay;

        private void Update()
        {
            if (IsAllowPlay)
            {
                TransformY.Rotate(new Vector3(RotationSpeedX, RotationSpeedY, RotationSpeedZ) * Time.deltaTime);
            }
        }

        public void SetupData(bool isAllowPlay, float rotationSpeedX, float rotationSpeedY, float rotationSpeedZ)
        {
            IsAllowPlay = isAllowPlay;
            RotationSpeedX = rotationSpeedX;
            RotationSpeedY = rotationSpeedY;
            RotationSpeedZ = rotationSpeedZ;
        }
    }
    public partial class EffectRotation
    {
        public bool Isallowplay => IsAllowPlay;
        public float Rotationspeedx => RotationSpeedX;
        public float Rotationspeedy => RotationSpeedY;
        public float Rotationspeedz => RotationSpeedZ;
    }
}