// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-6-17
// ------------------------------

using UnityEngine;

namespace YUnity
{
    public static class ColorExt
    {
        public static string RGBString(this Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        public static string RGBAString(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
    }
}