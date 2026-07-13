namespace YCSharp
{
    public static class ClassExt
    {
        /// <summary>
        /// 通用前置校验：
        /// - 如果两者引用相等，返回 true（完全相等）
        /// - 如果只有一边为 null，返回 false（不相等）
        /// - 否则返回 null，表示需要继续按业务字段比较
        ///
        /// 注意：此方法只做引用/空检查，不依赖或触发类型对 == 的重载。若类型重载了 ==，请在外部根据业务决定是否使用该运算符。
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="t">当前对象（可为 null）</param>
        /// <param name="another">待比较对象（可为 null）</param>
        /// <returns>
        /// true  - 引用相同；
        /// false - 只有一方为 null（单边空）；
        /// null  - 无前置结论，需要进一步按字段比较。
        /// </returns>
        public static bool? QuickCompare<T>(this T t, T another) where T : class
        {
            // 同一对象引用（包含同为 null），完全相等
            if (ReferenceEquals(t, another)) return true;
            // 使用 ReferenceEquals 做 null 检查以避免触发 T 的 == 重载
            if (ReferenceEquals(t, null) || ReferenceEquals(another, null)) return false;
            // 无前置结论，外部继续对比字段
            return null;
        }
    }
}