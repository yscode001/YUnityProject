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

        /// <summary>
        /// 调用对象池，将游戏物体进行回收
        /// </summary>
        public void UnSpawnFromObjectPool()
        {
            ObjectPool.UnSpawn(GameObjectY);
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

        /// <summary>
        /// 调用对象池，将游戏物体进行释放
        /// </summary>
        /// <param name="immediage">是否立即释放</param>
        public void ReleaseFromObjectPool(bool immediage = false)
        {
            ObjectPool.Release(GameObjectY, immediage);
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