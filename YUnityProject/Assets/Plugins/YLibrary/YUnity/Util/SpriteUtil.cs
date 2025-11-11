// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-7-28
// ------------------------------

using UnityEngine;

namespace YUnity
{
    public class SpriteUtil
    {
        public static Sprite Generate(int width, int height, Color[] colors, bool updateMipmaps = true, bool makeNoLongerReadable = false)
        {
            if (width <= 0 || height <= 0 || colors == null || colors.Length != width * height) { return null; }
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.SetPixels(colors);
            texture.Apply(updateMipmaps, makeNoLongerReadable);
            return Generate(texture);
        }
        public static Sprite Generate(int width, int height, Color32[] colors, bool updateMipmaps = true, bool makeNoLongerReadable = false)
        {
            if (width <= 0 || height <= 0 || colors == null || colors.Length != width * height) { return null; }
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.SetPixels32(colors);
            texture.Apply(updateMipmaps, makeNoLongerReadable);
            return Generate(texture);
        }
        public static Sprite Generate(byte[] imgBytes)
        {
            Texture2D texture = Texture2DUtil.Generate(imgBytes);
            return Generate(texture);
        }
        public static Sprite Generate(Texture2D texture)
        {
            if (texture != null)
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 生成透明图片
        /// </summary>
        public static Sprite GenerateTransparentSprite(int width, int height)
        {
            Texture2D texture2D = Texture2DUtil.GenerateTransparentTexture2D(width, height);
            // 转换
            return Sprite.Create(texture2D, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        }

        private static Sprite _transparentSprite = null;

        /// <summary>
        /// 大小为100*100的透明图片
        /// </summary>
        public static Sprite TransparentSprite
        {
            get
            {
                if (_transparentSprite == null)
                {
                    _transparentSprite = GenerateTransparentSprite(100, 100);
                }
                return _transparentSprite;
            }
        }
    }
}