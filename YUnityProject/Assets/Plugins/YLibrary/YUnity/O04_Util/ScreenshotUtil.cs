// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-7-28
// ------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 截屏工具
    /// </summary>
    public class ScreenshotUtil
    {
        /// <summary>
        /// 截取全屏
        /// </summary>
        /// <param name="waitSeconds"></param>
        /// <param name="complete"></param>
        /// <returns></returns>
        public static IEnumerator CapruerFullScreen(float waitSeconds, Action<Texture2D> complete)
        {
            if (waitSeconds > 0)
            {
                yield return new WaitForSeconds(waitSeconds);
            }
            yield return new WaitForEndOfFrame();
            complete?.Invoke(ScreenCapture.CaptureScreenshotAsTexture());
        }

        /// <summary>
        /// 截取部分屏幕
        /// </summary>
        /// <param name="rect">截取屏幕的范围</param>
        /// <param name="format"></param>
        /// <param name="waitSeconds"></param>
        /// <param name="complete"></param>
        /// <returns></returns>
        public static IEnumerator CapturePartScreen(Rect rect, TextureFormat format, float waitSeconds, Action<Texture2D> complete)
        {
            if (waitSeconds > 0)
            {
                yield return new WaitForSeconds(waitSeconds);
            }
            yield return new WaitForEndOfFrame();
            if (rect.x > Screen.width || rect.y > Screen.height || rect.width <= 0 || rect.height <= 0)
            {
                complete?.Invoke(null);
            }
            else
            {
                Rect newR = new Rect(Mathf.Max(0, rect.x), Mathf.Max(0, rect.y), rect.width, rect.height);
                float maxW = Screen.width - newR.x;
                float maxH = Screen.height - newR.y;
                Rect newR2 = new Rect(newR.x, newR.y, Mathf.Min(newR.width, maxW), Mathf.Min(newR.height, maxH));

                Texture2D t2d = new Texture2D((int)newR2.width, (int)newR2.height, format, false);
                t2d.ReadPixels(newR2, 0, 0);
                t2d.Apply();
                complete?.Invoke(t2d);
            }
        }

        /// <summary>
        /// 对指定相机进行截屏
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="xPixel">x轴像素(越高越清晰)</param>
        /// <param name="yPixel">y轴像素(越高越清晰)</param>
        /// <param name="format"></param>
        /// <param name="waitSeconds"></param>
        /// <param name="complete"></param>
        /// <returns></returns>
        public static IEnumerator CaptureCameraScreen(Camera camera, int xPixel, int yPixel, TextureFormat format, float waitSeconds, Action<Texture2D> complete)
        {
            if (waitSeconds > 0)
            {
                yield return new WaitForSeconds(waitSeconds);
            }
            yield return new WaitForEndOfFrame();
            if (camera == null || xPixel <= 0 || yPixel <= 0)
            {
                complete?.Invoke(null);
            }
            else
            {
                // 保存原始值，便于恢复
                RenderTexture originCameraRT = camera.targetTexture;
                RenderTexture originRTActive = RenderTexture.active;

                RenderTexture rt = RenderTexture.GetTemporary(xPixel, yPixel, 32);
                rt.antiAliasing = 8;

                camera.targetTexture = rt;
                camera.RenderDontRestore();

                RenderTexture.active = rt;
                Texture2D t2d = new Texture2D(xPixel, yPixel, format, false);
                t2d.ReadPixels(new Rect(0, 0, xPixel, yPixel), 0, 0);
                t2d.Apply();

                // 恢复
                camera.targetTexture = originCameraRT;
                RenderTexture.active = originRTActive;

                // 销毁临时物体
                RenderTexture.ReleaseTemporary(rt);

                complete?.Invoke(t2d);
            }
        }
    }
}