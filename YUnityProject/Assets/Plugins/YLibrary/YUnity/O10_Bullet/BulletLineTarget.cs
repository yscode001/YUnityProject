using System;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 子弹直线飞行追踪目标
    /// </summary>
    public partial class BulletLineTarget : MonoBehaviourBaseY
    {
        /// <summary>
        /// 移动目标
        /// </summary>
        private Transform TargetTransform;

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
        private bool IsFlying = false; // 是否正在飞行

        private void Clear()
        {
            IsFlying = false;

            TargetTransform = null;
            MoveSpeed = ErrorDistance = 0;
            TargetDeathWhenFlying = ReachedComplete = null;
        }
    }
    public partial class BulletLineTarget
    {
        /// <summary>
        /// 子弹开始飞行
        /// </summary>
        /// <param name="targetTransform">移动目标</param>
        /// <param name="moveSpeed">飞行速度</param>
        /// <param name="errorDistance">最大误差距离</param>
        /// <param name="targetDeathWhenFlying">飞行过程中目标死亡了(如被其他玩家干掉了，不会再执行ReachedComplete)</param>
        /// <param name="reachedComplete">达到目标位置后的回调</param>
        public void BeginFly(Transform targetTransform, float moveSpeed, float errorDistance, Action targetDeathWhenFlying, Action reachedComplete)
        {
            Clear();
            if (targetTransform == null || !targetTransform.gameObject.activeInHierarchy || moveSpeed <= 0 || errorDistance < 0)
            {
                return;
            }
            TargetTransform = targetTransform;
            MoveSpeed = moveSpeed;
            ErrorDistance = errorDistance;
            TargetDeathWhenFlying = targetDeathWhenFlying;
            ReachedComplete = reachedComplete;

            IsFlying = true;
        }
    }
    public partial class BulletLineTarget
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
                TransformY.LookAt(TargetTransform.position);
                Vector3 willMove = MoveSpeed * Time.deltaTime * TransformY.forward.normalized;
                TransformY.Translate(willMove, Space.World);
            }
        }
    }
}