using System;
using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 直线穿透子弹类型
    /// </summary>
    public enum BulletLinePenetrateType
    {
        /// <summary>
        /// 飞行达到最长时间消失
        /// </summary>
        Time,

        /// <summary>
        /// 飞行超过最远距离消失
        /// </summary>
        Distance,
    }

    /// <summary>
    /// 误差距离类型
    /// </summary>
    public enum ErrorDistanceType
    {
        /// <summary>
        /// 距离
        /// </summary>
        Distance,

        /// <summary>
        /// X轴
        /// </summary>
        X,

        /// <summary>
        /// Y轴
        /// </summary>
        Y,

        /// <summary>
        /// Z轴
        /// </summary>
        Z,
    }

    /// <summary>
    /// 直线穿透子弹，飞行达到最长时间或超过最远距离后消失
    /// </summary>
    public partial class BulletLinePenetrate : MonoBehaviourBaseY
    {
        /// <summary>
        /// 子弹类型
        /// </summary>
        private BulletLinePenetrateType Type = BulletLinePenetrateType.Distance;

        /// <summary>
        /// 飞行方向
        /// </summary>
        private Vector3 Direction = Vector3.zero;

        /// <summary>
        /// 最长飞行秒数
        /// </summary>
        private float MaxSeconds = 0;

        /// <summary>
        /// 最远飞行距离
        /// </summary>
        private float MaxDistance = 0;

        /// <summary>
        /// 子弹速度
        /// </summary>
        private float MoveSpeed = 0;

        /// <summary>
        /// 误差距离类型
        /// </summary>
        private ErrorDistanceType ErrorDistanceType = ErrorDistanceType.Distance;

        /// <summary>
        /// 误差距离
        /// </summary>
        private float ErrorDistance = 0;

        /// <summary>
        /// 路径上的敌人集合
        /// </summary>
        private List<Transform> AllEnemyList;

        /// <summary>
        /// 对路径上的敌人造成伤害，参数是伤害到第几个敌人了(从1开始)
        /// </summary>
        private Action<Transform, int> Damage;

        /// <summary>
        /// 完成回调
        /// </summary>
        private Action Complete = null;

        // 下面是辅助属性
        private bool IsFlying = false; // 是否正在飞行
        private float CurDeltaTime = 0; // 辅助子弹类型为时间类型
        private Vector3 StartPos = Vector3.zero; // 辅助子弹类型为距离类型
        private int DamageCount = 0; // 这颗子弹对多少个敌人造成伤害了

        private void Clear()
        {
            IsFlying = false;
            CurDeltaTime = 0;
            StartPos = Vector3.zero;
            DamageCount = 0;

            Direction = Vector3.zero;
            MaxSeconds = MaxDistance = MoveSpeed = ErrorDistance = 0;
            ErrorDistanceType = ErrorDistanceType.Distance;
            AllEnemyList = null;
            Damage = null;
            Complete = null;
        }
    }
    public partial class BulletLinePenetrate
    {
        /// <summary>
        /// 子弹开始飞行
        /// </summary>
        /// <param name="type">子弹类型</param>
        /// <param name="direction">飞行方向</param>
        /// <param name="maxValue">最长飞行秒数或最远飞行距离</param>
        /// <param name="moveSpeed">飞行速度</param>
        /// <param name="errorDistanceType">误差距离类型</param>
        /// <param name="errorDistance">误差距离</param>
        /// <param name="allEnemyList">路径上的敌人集合</param>
        /// <param name="damage">对路径上的敌人造成伤害，参数是伤害到第几个敌人了(从1开始)</param>
        /// <param name="complete">完成回调</param>
        public void BeginFly(BulletLinePenetrateType type, Vector3 direction, float maxValue, float moveSpeed, ErrorDistanceType errorDistanceType, float errorDistance, List<Transform> allEnemyList, Action<Transform, int> damage, Action complete)
        {
            Clear();
            if (direction == Vector3.zero || moveSpeed <= 0 || maxValue <= 0 || errorDistance < 0)
            {
                return;
            }
            Type = type;
            Direction = direction;
            if (type == BulletLinePenetrateType.Distance) { MaxDistance = maxValue; StartPos = TransformY.position; }
            else { MaxSeconds = maxValue; }
            MoveSpeed = moveSpeed;
            ErrorDistanceType = errorDistanceType;
            ErrorDistance = errorDistance;
            AllEnemyList = allEnemyList;
            Damage = damage;
            Complete = complete;

            IsFlying = true; // 开始飞行
        }
    }
    public partial class BulletLinePenetrate
    {
        private void DamageEnemy(Vector3 bulletStartPosition, Vector3 bulletEndPosition)
        {
            if (Damage == null || AllEnemyList == null || AllEnemyList.Count <= 0)
            {
                return;
            }
            for (int i = AllEnemyList.Count - 1; i >= 0; i--)
            {
                Transform enemyTransform = AllEnemyList[i];
                if (enemyTransform != null)
                {
                    if (ErrorDistanceType == ErrorDistanceType.Distance && MathfUtil.GetPointToStraightLineDistance(enemyTransform.position, bulletStartPosition, bulletEndPosition) <= ErrorDistance)
                    {
                        // 距离误差
                        DamageCount += 1;
                        Damage?.Invoke(enemyTransform, DamageCount);
                        AllEnemyList.RemoveAt(i);
                    }
                    else if (ErrorDistanceType == ErrorDistanceType.X && Mathf.Abs(TransformY.position.x - enemyTransform.position.x) <= ErrorDistance)
                    {
                        // X轴误差
                        DamageCount += 1;
                        Damage?.Invoke(enemyTransform, DamageCount);
                        AllEnemyList.RemoveAt(i);
                    }
                    else if (ErrorDistanceType == ErrorDistanceType.Y && Mathf.Abs(TransformY.position.y - enemyTransform.position.y) <= ErrorDistance)
                    {
                        // Y轴误差
                        DamageCount += 1;
                        Damage?.Invoke(enemyTransform, DamageCount);
                        AllEnemyList.RemoveAt(i);
                    }
                    else if (ErrorDistanceType == ErrorDistanceType.Z && Mathf.Abs(TransformY.position.z - enemyTransform.position.z) <= ErrorDistance)
                    {
                        // Z轴误差
                        DamageCount += 1;
                        Damage?.Invoke(enemyTransform, DamageCount);
                        AllEnemyList.RemoveAt(i);
                    }
                }
            }
        }
        private void Update()
        {
            if (IsFlying == false)
            {
                return;
            }

            DamageEnemy(TransformY.position, TransformY.position + MoveSpeed * Time.deltaTime * Direction);

            if ((Type == BulletLinePenetrateType.Distance && Vector3.Distance(TransformY.position, StartPos) >= MaxDistance) ||
                (Type == BulletLinePenetrateType.Time && CurDeltaTime >= MaxSeconds))
            {
                Complete?.Invoke();
                Clear();
                return;
            }
            if (Type == BulletLinePenetrateType.Time)
            {
                CurDeltaTime += Time.deltaTime;
            }
            TransformY.Translate(MoveSpeed * Time.deltaTime * Direction, Space.World);
        }
    }
}