using System;
using System.Collections.Generic;
using System.Linq;

namespace YCSharp
{
    public static class ListUtil
    {
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
}