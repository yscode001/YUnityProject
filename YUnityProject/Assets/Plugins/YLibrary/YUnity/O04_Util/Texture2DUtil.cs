// Author：yaoshuai
// Email：yscode@126.com
// Date：2025-9-17
// ------------------------------

using UnityEngine;

namespace YUnity
{
    public class Texture2DUtil
    {
        public static Texture2D Generate(byte[] imgBytes)
        {
            if (imgBytes == null || imgBytes.Length == 0) { return null; }
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (texture.LoadImage(imgBytes))
            {
                texture.Apply();
                return texture;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 生成透明图片
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture2D GenerateTransparentTexture2D(int width, int height)
        {
            // 创建一个透明的Texture2D
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            // 设置所有像素为透明
            Color transparentColor = new Color(0, 0, 0, 0);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = transparentColor;
            }
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

        private static Texture2D _transparentTexture2D = null;

        /// <summary>
        /// 大小为100*100的透明图片
        /// </summary>
        public static Texture2D TransparentTexture2D
        {
            get
            {
                if (_transparentTexture2D == null)
                {
                    _transparentTexture2D = GenerateTransparentTexture2D(100, 100);
                }
                return _transparentTexture2D;
            }
        }
    }
}