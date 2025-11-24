namespace YUnity
{
    /// <summary>
    /// 定义协议，需要回收的Object需遵守此协议
    /// </summary>
    public interface IReusable
    {
        /// <summary>
        /// 当游戏物体从池中取出或放入池中时，执行的一些操作(如初始化)
        /// </summary>
        /// <param name="isSpawn">是否是Spawn</param>
        void OnSpawnOrUnSpawn(bool isSpawn);

        /// <summary>
        /// 调用对象池，将游戏物体进行回收
        /// </summary>
        /// <returns></returns>
        void UnSpawnFromObjectPool();

        /// <summary>
        /// 调用对象池，将游戏物体进行释放
        /// </summary>
        /// <param name="immediage">是否立即释放</param>
        /// <returns></returns>
        void ReleaseFromObjectPool(bool immediage = false);
    }
}