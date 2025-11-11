namespace YUnity.Game.BlockPuzzle
{
    /// <summary>
    /// 填充进度
    /// </summary>
    public enum FillProgress
    {
        /// <summary>
        /// 未填充，全部为空
        /// </summary>
        Empty,

        /// <summary>
        /// 填充部分
        /// </summary>
        Part,

        /// <summary>
        /// 填充满了
        /// </summary>
        Full,
    }
}