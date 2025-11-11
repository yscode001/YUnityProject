// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-6-17
// ------------------------------

using UnityEngine;

namespace YUnity
{
    public static class StringToColorExt
    {
        public static Color Color(this string value)
        {
            if (ColorUtility.TryParseHtmlString(value, out Color col))
            {
                return col;
            }
            else
            {
                return UnityEngine.Color.clear;
            }
        }
    }
}