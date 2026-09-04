using System;
using System.Collections.Generic;
using System.Linq;

public static class HashSetUtil
{
    /// <summary>
    /// 求交集、A独有、B独有
    /// </summary>
    public static void CalcDiff<T>(HashSet<T> a, HashSet<T> b,
        out (HashSet<T> intersect, HashSet<T> aOnly, HashSet<T> bOnly) result)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        if (b == null)
            throw new ArgumentNullException(nameof(b));

        var intersect = new HashSet<T>(a.Comparer);
        var aOnly = new HashSet<T>(a.Comparer);
        var bOnly = new HashSet<T>(b.Comparer);

        foreach (var item in a)
        {
            if (b.Contains(item))
                intersect.Add(item);
            else
                aOnly.Add(item);
        }

        foreach (var item in b.Where(item => !a.Contains(item)))
        {
            bOnly.Add(item);
        }

        result = (intersect, aOnly, bOnly);
    }

    /// <summary>
    /// 求交集、A独有、B独有
    /// </summary>
    public static void CalcDiff<T>(List<T> a, List<T> b,
        out (List<T> intersect, List<T> aOnly, List<T> bOnly) result)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        if (b == null)
            throw new ArgumentNullException(nameof(b));

        var intersect = new List<T>();
        var aOnly = new List<T>();

        foreach (var item in a)
        {
            if (b.Contains(item))
                intersect.Add(item);
            else
                aOnly.Add(item);
        }

        var bOnly = b.Where(item => !a.Contains(item)).ToList();

        result = (intersect, aOnly, bOnly);
    }
}