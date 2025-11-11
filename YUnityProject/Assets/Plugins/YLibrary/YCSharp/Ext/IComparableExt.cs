using System;
using System.Collections.Generic;
using System.Linq;

namespace YCSharp
{
    public enum CollectionValue
    {
        Max,
        Min,
    }
    public static class IComparableExt
    {
        public static T GetValue<T>(this List<T> list, CollectionValue valueType) where T : IComparable
        {
            T max = list[0];
            foreach (var item in list)
            {
                if (valueType == CollectionValue.Max && max.CompareTo(item) < 0)
                {
                    max = item;
                }
                else if (valueType == CollectionValue.Min && max.CompareTo(item) > 0)
                {
                    max = item;
                }
            }
            return max;
        }
        public static T GetValue<T>(this T[] array, CollectionValue valueType) where T : IComparable
        {
            T max = array[0];
            foreach (var item in array)
            {
                if (valueType == CollectionValue.Max && max.CompareTo(item) < 0)
                {
                    max = item;
                }
                else if (valueType == CollectionValue.Min && max.CompareTo(item) > 0)
                {
                    max = item;
                }
            }
            return max;
        }
        public static T2 GetValue<T1, T2>(this Dictionary<T1, T2> dict, CollectionValue valueType) where T2 : IComparable
        {
            T2 max = dict.Values.First();
            foreach (var item in dict)
            {
                if (valueType == CollectionValue.Max && max.CompareTo(item.Value) < 0)
                {
                    max = item.Value;
                }
                else if (valueType == CollectionValue.Min && max.CompareTo(item.Value) > 0)
                {
                    max = item.Value;
                }
            }
            return max;
        }
    }
}