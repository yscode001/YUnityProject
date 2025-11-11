// Author：yaoshuai
// Email：yscode@126.com
// Date：2025-7-30
// ------------------------------
using UnityEngine;

namespace YUnity
{
    public static class MaterialExt
    {
        public static void ResetShader(this Material material)
        {
            Shader shader = Shader.Find(material.shader.name);
            if (shader != null)
            {
                material.shader = shader;
            }
        }
    }
}