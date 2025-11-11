using System;
using System.Collections.Generic;
using System.Linq;

namespace YCSharp
{
    public static class IEnumerableExt
    {
        public static bool Contains<T>(this IEnumerable<T> values, Predicate<T> filterCondition)
        {
            foreach (var item in values)
            {
                if (filterCondition.Invoke(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static void For<T>(this IEnumerable<T> values, Func<bool> breakCondition, Predicate<T> filterCondition, Action<(int index, T element)> doAction)
        {
            if (breakCondition != null && breakCondition.Invoke()) { return; }

            int index = 0;
            foreach (var element in values)
            {
                if (breakCondition != null && breakCondition.Invoke()) { break; }

                if (filterCondition == null || filterCondition.Invoke(element))
                {
                    // 没有过滤条件，或通过过滤条件
                    doAction?.Invoke((index, element));
                }

                if (breakCondition != null && breakCondition.Invoke()) { break; }
                index += 1;
            }
        }
        public static void ForReverse<T>(this IEnumerable<T> values, Func<bool> breakCondition, Predicate<T> filterCondition, Action<(int index, T element)> doAction)
        {
            if (breakCondition != null && breakCondition.Invoke()) { return; }

            int index = values.Count() - 1;
            foreach (var element in values.Reverse())
            {
                if (breakCondition != null && breakCondition.Invoke()) { break; }

                if (filterCondition == null || filterCondition.Invoke(element))
                {
                    // 没有过滤条件，或通过过滤条件
                    doAction?.Invoke((index, element));
                }

                if (breakCondition != null && breakCondition.Invoke()) { break; }
                index -= 1;
            }
        }
        public static List<T> DeepCopy<T>(this IEnumerable<T> values, bool allowDuplicate, Predicate<T> filterCondition)
        {
            List<T> list = new List<T>();
            foreach (var element in values)
            {
                if (filterCondition == null || filterCondition.Invoke(element))
                {
                    // 没有过滤条件，或通过过滤条件
                    if (allowDuplicate)
                    {
                        list.Add(element);
                    }
                    else if (!allowDuplicate && !list.Contains(element))
                    {
                        list.Add(element);
                    }
                }
            }
            return list;
        }
        public static List<T2> Convert<T1, T2>(this IEnumerable<T1> values, bool allowDuplicate, Predicate<T1> filterCondition, Func<T1, T2> convertCondition)
        {
            List<T2> list = new List<T2>();
            foreach (var element in values)
            {
                if (filterCondition == null || filterCondition.Invoke(element))
                {
                    // 没有过滤条件，或通过过滤条件
                    T2 t2 = convertCondition.Invoke(element);
                    if (allowDuplicate)
                    {
                        list.Add(t2);
                    }
                    else if (!allowDuplicate && !list.Contains(t2))
                    {
                        list.Add(t2);
                    }
                }
            }
            return list;
        }
    }
}