using System;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 子弹曲线追踪移动目标
    /// </summary>
    public partial class BulletCurveTarget : MonoBehaviourBaseY
    {
        /// <summary>
        /// 移动目标
        /// </summary>
        private Transform TargetTransform = null;

        /// <summary>
        /// 子弹速度
        /// </summary>
        private float MoveSpeed = 0;

        /// <summary>
        /// 误差距离
        /// </summary>
        private float ErrorDistance = 0;

        /// <summary>
        /// 飞行过程中目标死亡了(如被其他玩家干掉了，不会再执行ReachedComplete)
        /// </summary>
        private Action TargetDeathWhenFlying = null;

        /// <summary>
        /// 达到目标位置后的回调
        /// </summary>
        private Action ReachedComplete = null;

        // 下面是辅助属性
        private bool IsFlying = false; // 是否正在移动
        private Vector3 CurveDir = Vector3.zero; // 曲线方向

        private void Clear()
        {
            IsFlying = false;
            CurveDir = Vector3.zero;

            TargetTransform = null;
            MoveSpeed = ErrorDistance = 0;
            TargetDeathWhenFlying = ReachedComplete = null;
        }
    }
    public partial class BulletCurveTarget
    {
        /// <summary>
        /// 开始飞行
        /// </summary>
        /// <param name="curveDir">曲线方向，zero表示随机曲线方向</param>
        /// <param name="curveRandomSeed">随机方向种子，仅在curveDir为zero时有意义</param>
        /// <param name="curveWeight">曲线权重(0-1)</param>
        /// <param name="targetTransform">移动目标</param>
        /// <param name="moveSpeed">飞行速度</param>
        /// <param name="errorDistance">误差距离</param>
        /// <param name="targetDeathWhenFlying">飞行过程中目标死亡了(如被其他玩家干掉了，不会再执行ReachedComplete)</param>
        /// <param name="reachedComplete">达到目标位置后的回调</param>
        public void BeginFly(Vector3 curveDir, int curveRandomSeed, float curveWeight, Transform targetTransform, float moveSpeed, float errorDistance, Action targetDeathWhenFlying, Action reachedComplete)
        {
            Clear();
            if (targetTransform == null || !targetTransform.gameObject.activeInHierarchy || moveSpeed <= 0 || errorDistance < 0 || (curveDir == Vector3.zero && curveRandomSeed <= 0))
            {
                return;
            }
            TargetTransform = targetTransform;
            MoveSpeed = moveSpeed;
            ErrorDistance = errorDistance;
            TargetDeathWhenFlying = targetDeathWhenFlying;
            ReachedComplete = reachedComplete;
            if (curveDir == Vector3.zero)
            {
                // 随机曲线弹道
                System.Random ran = new System.Random(curveRandomSeed);
                Vector3 v1 = Vector3.Cross(targetTransform.position - TransformY.position, Vector3.up).normalized;
                v1 *= ran.Next(-100, 100);
                Vector3 v2 = Vector3.up * ran.Next(0, 100);
                CurveDir = (v1 + v2).normalized * Mathf.Clamp(curveWeight, 0, 1);
            }
            else
            {
                // 指定曲线弹道
                CurveDir = curveDir * Mathf.Clamp(curveWeight, 0, 1);
            }
            IsFlying = true; // 开始飞行
        }
    }
    public partial class BulletCurveTarget
    {
        private void Update()
        {
            if (IsFlying == false)
            {
                return;
            }
            if (TargetTransform == null || !TargetTransform.gameObject.activeInHierarchy)
            {
                TargetDeathWhenFlying?.Invoke();
                Clear();
                return;
            }
            if (Vector3.Distance(TargetTransform.position, TransformY.position) <= ErrorDistance)
            {
                // 抵达终点
                ReachedComplete?.Invoke();
                Clear();
            }
            else
            {
                // 飞向目标
                Vector3 dir = (TargetTransform.position - TransformY.position).normalized;
                if (CurveDir != Vector3.zero)
                {
                    dir = (dir + CurveDir).normalized;
                }
                TransformY.Translate(MoveSpeed * Time.deltaTime * dir, Space.World);
            }
        }
    }
}