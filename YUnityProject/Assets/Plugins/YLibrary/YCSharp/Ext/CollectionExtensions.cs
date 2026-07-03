using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YCSharp
{
    public static partial class CollectionExtensions
    {
        #region HasAny & 内部辅助方法（统一放这里）
        /// <summary>数组不为 null 且包含至少一个元素</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny<T>(this T[] array)
            => InternalHasAny(array?.Length);

        /// <summary>字典不为 null 且包含至少一组键值对</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny<TKey, TValue>(this Dictionary<TKey, TValue> dict)
            => InternalHasAny(dict?.Count);

        /// <summary>通用 ICollection 集合不为 null 且有元素</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny<T>(this ICollection<T> collection)
            => InternalHasAny(collection?.Count);

        /// <summary>泛型枚举是否包含任意元素（优先读取Count，无Count则迭代一次）</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return false;
            if (enumerable is IReadOnlyCollection<T> roCol) return roCol.Count > 0;
            if (enumerable is ICollection<T> col) return col.Count > 0;
            return enumerable.Any();
        }

        /// <summary>私有统一收敛逻辑，所有数字判有元素走这里</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool InternalHasAny(int? count) => count > 0;

        /// <summary>私有统一收敛逻辑，所有数字判空走这里</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool InternalIsEmpty(int? count) => count is null or 0;
        #endregion
    }

    public static partial class CollectionExtensions
    {
        #region IsNotEmpty
        /// <summary>数组不为 null 且存在元素，等价 HasAny</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this T[] array) => array.HasAny();

        /// <summary>字典不为 null 且包含至少一组键值对，等价 HasAny</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict) => dict.HasAny();

        /// <summary>通用 ICollection 集合不为 null 且有元素，等价 HasAny</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this ICollection<T> collection) => collection.HasAny();

        /// <summary>泛型枚举是否包含任意元素，等价 HasAny</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable) => enumerable.HasAny();
        #endregion
    }

    public static partial class CollectionExtensions
    {
        #region IsEmpty
        /// <summary>数组为 null 或无元素</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this T[] array)
            => InternalIsEmpty(array?.Length);

        /// <summary>字典为 null 或无键值对</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict)
            => InternalIsEmpty(dict?.Count);

        /// <summary>ICollection 为 null 或无元素</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this ICollection<T> collection)
            => InternalIsEmpty(collection?.Count);

        /// <summary>枚举为 null 或无元素</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
            => !enumerable.HasAny();
        #endregion
    }

    public static partial class CollectionExtensions
    {
        #region HasIndex 仅支持带索引器的容器（数组/IList/IReadOnlyList）
        /// <summary>数组不为 null，且下标 index 合法可访问</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasIndex<T>(this T[] array, int index)
            => array.HasAny() && index >= 0 && index < array.Length;

        /// <summary>列表不为 null，且下标 index 合法可访问（List<T>等）</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasIndex<T>(this IList<T> list, int index)
            => list.HasAny() && index >= 0 && index < list.Count;
        #endregion
    }
}