using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace YUnity
{
    /// <summary>
    /// 相机点位切换(Camera必须放里面)
    /// </summary>
    public partial class EffectCameraPointSwitcher : MonoBehaviour
    {
        [Header("是否自动切换点位")]
        [SerializeField] private bool AutoSwitch = false;
        [Header("看向的目标")]
        public Transform Target;
        [Header("需切换的点位集合")]
        public Transform[] SwitchPoints;
        [Header("切换的时间间隔")]
        public float TimeInterval = 3;
        [Header("目标和点位之间的最小间距(太近了点位无效)")]
        public float MinDistance = 0.5f;
        [Header("相机视野曲线(x-点位与目标的距离，y-fieldOfView)")]
        public AnimationCurve CameraFieldOfView = AnimationCurve.Linear(1, 30, 10, 30);
        [Header("是否很精确的望向目标")]
        public bool ForceStable = false;
        [Header("切换镜头时看向目标的旋转速度")]
        public float RotationSpeed = 2;

        // 随机一个数，小于这个值很精确的望向目标
        private float stability = 0.5f;

        private Vector3 followPoint; // 要看向的点

        public void SetupDataAndThenStartAutoChange(Transform target, Transform[] switchPoints, float timeInterval = 3, float minDistance = 0.5f, float rotationSpeed = 2, bool forceStable = false)
        {
            Target = target;
            SwitchPoints = switchPoints;
            TimeInterval = timeInterval;
            MinDistance = minDistance;
            RotationSpeed = rotationSpeed;
            StartAutoChange();
        }
        public void SetupTargetAndThenStartAutoChange(Transform target)
        {
            Target = target;
            StartAutoChange();
        }
    }
    public partial class EffectCameraPointSwitcher
    {
        private void Start()
        {
            if (AutoSwitch)
            {
                StartAutoChange();
            }
        }
        private void Update()
        {
            if (!CanotExecute)
            {
                var param = Mathf.Exp(-RotationSpeed * Time.deltaTime);
                followPoint = Vector3.Lerp(Target.position, followPoint, param);
                transform.LookAt(followPoint);
            }
        }
    }
    public partial class EffectCameraPointSwitcher
    {
        public void StartAutoChange()
        {
            StopAutoChange();
            // 务必：先移除无效点位，再做判断
            RemoveInvalidPoints();
            if (!CanotExecute)
            {
                StartCoroutine("AutoChange");
            }
        }
        public void StopAutoChange()
        {
            StopCoroutine("AutoChange");
        }
    }
    public partial class EffectCameraPointSwitcher
    {
        private IEnumerator AutoChange()
        {
            for (var current = SwitchPoints[0]; true; current = GetNextPoint(current))
            {
                if (current == null)
                {
                    StopAutoChange();
                    yield return null;
                }
                else
                {
                    ChangePosition(current, ForceStable);
                    yield return new WaitForSeconds(TimeInterval);
                }
            }
        }
        private Transform GetNextPoint(Transform current)
        {
            if (SwitchPoints.Length < 2)
            {
                return null;
            }
            while (true)
            {
                var nextPoint = SwitchPoints[Random.Range(0, SwitchPoints.Length)];
                if (nextPoint != current)
                {
                    return nextPoint;
                }
            }
        }

        /// <summary>
        /// 切换点位
        /// </summary>
        /// <param name="destination">目标点位</param>
        /// <param name="forceStable">是否很精确的望向目标</param>
        public void ChangePosition(Transform destination, bool forceStable)
        {
            if (destination == null || !enabled) { return; }
            transform.position = destination.position;

            if (ForceStable || Random.value < stability)
            {
                followPoint = Target.position;
            }
            else
            {
                followPoint = Target.position + Random.insideUnitSphere;
            }
            var dist = Vector3.Distance(Target.position, transform.position);
            ChildCamera.fieldOfView = CameraFieldOfView.Evaluate(dist);
        }
    }

    #region 工具函数
    public partial class EffectCameraPointSwitcher
    {
        private Camera _childCamera;
        private Camera ChildCamera
        {
            get
            {
                if (_childCamera == null)
                {
                    _childCamera = GetComponentInChildren<Camera>();
                }
                return _childCamera;
            }
        }

        /// <summary>
        /// 是否不可以执行
        /// </summary>
        private bool CanotExecute =>
            ChildCamera == null ||
            Target == null ||
            SwitchPoints == null || SwitchPoints.Length <= 0 ||
            TimeInterval <= 0;

        /// <summary>
        /// 移除无效点位，即和目标距离太近的点位
        /// </summary>
        private void RemoveInvalidPoints()
        {
            if (SwitchPoints == null || SwitchPoints.Length <= 0) { return; }
            List<Transform> points = new List<Transform>();
            for (int i = 0; i < SwitchPoints.Length; i++)
            {
                if (Vector3.Distance(SwitchPoints[i].position, Target.position) >= MinDistance)
                {
                    points.Add(SwitchPoints[i]);
                }
            }
            if (points.Count > 0)
            {
                SwitchPoints = points.ToArray();
            }
            else
            {
                SwitchPoints = null;
            }
        }
    }
    #endregion
}