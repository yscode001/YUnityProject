using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    public static class TransformExt
    {
        #region 重置
        public static void ResetLocal(this Transform tf)
        {
            tf.localPosition = Vector3.zero;
            tf.localEulerAngles = Vector3.zero;
            tf.localScale = Vector3.one; ;
        }
        public static void ResetLocal(this Transform tf, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale)
        {
            tf.localPosition = localPosition;
            tf.localEulerAngles = localEulerAngles;
            tf.localScale = localScale;
        }
        #endregion

        #region 查找子物体
        public static Transform FindChildRecursively(this Transform parent, string name)
        {
            Transform child = parent.Find(name);
            if (child == null)
            {
                foreach (Transform tran in parent)
                {
                    child = FindChildRecursively(tran, name);
                    if (child != null)
                    {
                        return child;
                    }
                }
            }
            return child;
        }
        public static Transform FindChildByPath(this Transform parent, string path)
        {
            return parent.Find(path);
        }
        #endregion

        #region 当前位置前后间隔距离的点
        /// <summary>
        /// 当前位置前后间隔距离(大于0才有意义)的点
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="isForward">前方true，后方false</param>
        /// <param name="distance">间隔距离，大于0才有意义</param>
        /// <returns>当前位置前后间隔距离(大于0才有意义)的点</returns>
        public static Vector3 DistancePosition(this Transform tf, bool isForward, float distance)
        {
            if (distance <= 0) { return tf.position; }
            return tf.position + (isForward ? 1 : -1) * tf.forward.normalized * Mathf.Abs(distance);
        }
        #endregion

        public static float Distance(this Transform tf, Transform other)
        {
            if (tf == null || other == null || tf == other) { return 0; }
            return Vector3.Distance(tf.position, other.position);
        }

        /// <summary>
        /// Copy基础属性至另一个(位置、角度、旋转)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="isLocal"></param>
        public static void CopyBasicPropertyToAnthor(this Transform from, Transform to, bool isLocal)
        {
            if (isLocal)
            {
                to.localPosition = from.localPosition;
                to.localRotation = from.localRotation;
            }
            else
            {
                to.position = from.position;
                to.rotation = from.rotation;
            }
            to.localScale = from.localScale;
        }

        /// <summary>
        /// 把自己移动至另一个Parent下面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        public static void MoveSelfGOToAnotherParent(this Transform self, Transform parent, bool worldPositionStays)
        {
            if (self == null || parent == null || self.parent == parent) { return; }
            self.SetParent(parent, worldPositionStays);
        }

        /// <summary>
        /// 创建一个和自己Transform一样的空Go，然后放到Parent下面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        /// <returns></returns>
        public static Transform CopySelfEmptyGoToAnotherParent(this Transform self, Transform parent, bool worldPositionStays)
        {
            if (self == null || parent == null || self.parent == parent) { return null; }
            GameObject go = new GameObject();
            Transform goT = go.transform;
            goT.name = self.name;
            goT.SetParent(self.parent);
            goT.localScale = self.localScale;
            goT.localPosition = self.localPosition;
            goT.localEulerAngles = self.localEulerAngles;
            goT.MoveSelfGOToAnotherParent(parent, worldPositionStays);
            return goT;
        }

        /// <summary>
        /// 把自己移动至Array中的某一个具有相同ParentName的下面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="array"></param>
        /// <param name="worldPositionStays"></param>
        public static void MoveSelfGOToInArrayWithSameParentName(this Transform self, Transform[] array, bool worldPositionStays)
        {
            if (self == null || self.parent == null || array == null || array.Length <= 0) { return; }
            string selfParentName = self.transform.parent.name;
            for (int i = 0; i < array.Length; i++)
            {
                Transform p = array[i];
                if (p.name == selfParentName)
                {
                    self.MoveSelfGOToAnotherParent(p, worldPositionStays);
                    break;
                }
            }
        }

        /// <summary>
        /// 把自己移动至Array中的某一个具有相同ParentName的下面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="array"></param>
        /// <param name="worldPositionStays"></param>
        public static void MoveSelfGOToInArrayWithSameParentName(this Transform self, List<Transform> array, bool worldPositionStays)
        {
            if (self == null || self.parent == null || array == null || array.Count <= 0) { return; }
            string selfParentName = self.transform.parent.name;
            for (int i = 0; i < array.Count; i++)
            {
                Transform p = array[i];
                if (p.name == selfParentName)
                {
                    self.MoveSelfGOToAnotherParent(p, worldPositionStays);
                    break;
                }
            }
        }

        /// <summary>
        /// 创建一个和自己Transform一样的空Go，然后放到Array中的某一个具有相同ParentName的下面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="array"></param>
        /// <param name="worldPositionStays"></param>
        /// <returns></returns>
        public static Transform CopySelfEmptyGoToInArrayWithSameParentName(this Transform self, Transform[] array, bool worldPositionStays)
        {
            if (self == null || self.parent == null || array == null || array.Length <= 0) { return null; }
            string selfParentName = self.transform.parent.name;
            for (int i = 0; i < array.Length; i++)
            {
                Transform p = array[i];
                if (p.name == selfParentName)
                {
                    return self.CopySelfEmptyGoToAnotherParent(p, worldPositionStays);
                }
            }
            return null;
        }

        /// <summary>
        /// 创建一个和自己Transform一样的空Go，然后放到Array中的某一个具有相同ParentName的下面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="array"></param>
        /// <param name="worldPositionStays"></param>
        /// <returns></returns>
        public static Transform CopySelfEmptyGoToInArrayWithSameParentName(this Transform self, List<Transform> array, bool worldPositionStays)
        {
            if (self == null || self.parent == null || array == null || array.Count <= 0) { return null; }
            string selfParentName = self.transform.parent.name;
            for (int i = 0; i < array.Count; i++)
            {
                Transform p = array[i];
                if (p.name == selfParentName)
                {
                    return self.CopySelfEmptyGoToAnotherParent(p, worldPositionStays);
                }
            }
            return null;
        }
    }
}