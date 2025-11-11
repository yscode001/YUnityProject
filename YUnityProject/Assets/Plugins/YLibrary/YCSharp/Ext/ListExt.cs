// Author：yaoshuai
// Email：yscode@126.com
// Date：2024-6-28
// ------------------------------

using System;
using System.Collections.Generic;

namespace YCSharp
{
    public static class ListExt
    {
        public static void RemoveElements<T>(this List<T> list, Predicate<T> filterCondition)
        {
            if (filterCondition != null)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (filterCondition.Invoke(list[i]))
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }
        public static void AddIfNotContain<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }
        }
    }
}