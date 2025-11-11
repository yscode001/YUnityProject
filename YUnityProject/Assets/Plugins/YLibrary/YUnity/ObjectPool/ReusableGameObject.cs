using System;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 定义抽象类，需要回收的Object需继承此抽象类
    /// </summary>
    public class ReusableGameObject : MonoBehaviourBaseY, IReusable
    {
        /// <summary>
        /// 在从池中取出之前SetActive之前调用
        /// </summary>
        public virtual void BeforeSpawn() { }

        /// <summary>
        /// 当游戏物体从池中取出或放入池中时，执行的一些操作(如初始化)
        /// </summary>
        /// <param name="isSpawn">是否是spawn</param>
        public virtual void OnSpawnOrUnSpawn(bool isSpawn)
        {
            StopAllCoroutines();
        }

        private Coroutine UnSpawnCoroutine;

        /// <summary>
        /// 调用对象池，将游戏物体进行回收
        /// </summary>
        /// <param name="delaySeconds">延迟回收秒数</param>
        /// <param name="doBeforeUnSpawn">真正回收之前做的操作</param>
        /// <returns></returns>
        public Coroutine UnSpawnFromObjectPool(float delaySeconds = 0, Action doBeforeUnSpawn = null)
        {
            if (UnSpawnCoroutine != null)
            {
                StopCoroutine(UnSpawnCoroutine);
            }
            UnSpawnCoroutine = DoAfterDelay(delaySeconds, () =>
            {
                doBeforeUnSpawn?.Invoke();
                ObjectPool.UnSpawn(GameObjectY);
            });
            return UnSpawnCoroutine;
        }

        /// <summary>
        /// 调用对象池，将继承自ReusableGameObject的子物体(不包括自己)进行回收
        /// </summary>
        public void UnSpawnChildrenWhoInheritReusableGameObjectFromObjectPool()
        {
            if (TransformY.childCount <= 0) { return; }
            for (int i = 0; i < TransformY.childCount; i++)
            {
                Transform childT = TransformY.GetChild(i);
                ReusableGameObject[] objs = childT.GetComponentsInChildren<ReusableGameObject>(true);
                if (objs != null && objs.Length > 0)
                {
                    foreach (var obj in objs)
                    {
                        obj.UnSpawnFromObjectPool();
                    }
                }
            }
        }

        private Coroutine ReleaseCoroutine;

        /// <summary>
        /// 调用对象池，将游戏物体进行释放
        /// </summary>
        /// <param name="delaySeconds">延迟释放秒数</param>
        /// <param name="immediage">到时间后是否立即释放</param>
        /// <param name="doBeforeRelease">真正释放之前做的操作</param>
        /// <returns></returns>
        public Coroutine ReleaseFromObjectPool(float delaySeconds = 0, bool immediage = false, Action doBeforeRelease = null)
        {
            if (ReleaseCoroutine != null)
            {
                StopCoroutine(ReleaseCoroutine);
            }
            ReleaseCoroutine = DoAfterDelay(delaySeconds, () =>
            {
                doBeforeRelease?.Invoke();
                ObjectPool.Release(GameObjectY, immediage);
            });
            return ReleaseCoroutine;
        }

        /// <summary>
        /// 调用对象池，将继承自ReusableGameObject的子物体(不包括自己)进行释放
        /// </summary>
        public void ReleaseChildrenWhoInheritReusableGameObjectFromObjectPool()
        {
            if (TransformY.childCount <= 0) { return; }
            for (int i = 0; i < TransformY.childCount; i++)
            {
                Transform childT = TransformY.GetChild(i);
                ReusableGameObject[] objs = childT.GetComponentsInChildren<ReusableGameObject>(true);
                if (objs != null && objs.Length > 0)
                {
                    foreach (var obj in objs)
                    {
                        obj.ReleaseFromObjectPool();
                    }
                }
            }
        }
    }
}