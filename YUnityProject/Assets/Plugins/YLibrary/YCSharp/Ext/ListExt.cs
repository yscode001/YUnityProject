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
                int totalCount = list.Count;
                for (int i = totalCount - 1; i >= 0; i--)
                {
                    if (filterCondition.Invoke(list[i]))
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }
        public static void RemoveDuplicate<T>(this List<T> list)
        {
            List<T> newList = new List<T>();
            list.For(null, null, data =>
            {
                if (!newList.Contains(data.element))
                {
                    newList.Add(data.element);
                }
            });
            list.Clear();
            list.AddRange(newList);
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