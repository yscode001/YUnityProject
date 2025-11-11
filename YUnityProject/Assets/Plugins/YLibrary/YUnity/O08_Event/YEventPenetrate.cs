// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-7-20
// ------------------------------
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 事件穿透
    /// </summary>
    public class YEventPenetrate : MonoBehaviour, ICanvasRaycastFilter
    {
        /// <summary>
        /// 事件将要穿透的目标区域
        /// </summary>
        public RectTransform PassThroughTargetRT { get; private set; } = null;

        /// <summary>
        /// 设置将要穿透的目标区域，null表示没有穿透区域，即不穿透
        /// </summary>
        /// <param name="passThroughTargetRT"></param>
        public void SetupPassThroughtTarget(RectTransform passThroughTargetRT)
        {
            PassThroughTargetRT = passThroughTargetRT;
        }

        /// <summary>
        /// 是否允许射线投射，true(允许射线投射，将拦截事件)；false(不允许射线投射，射线(事件)将穿透)
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <param name="eventCamera"></param>
        /// <returns>true(允许射线投射，将拦截事件)；false(不允许射线投射，射线(事件)将穿透)</returns>
        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (PassThroughTargetRT == null)
            {
                // 没有需要穿透的目标区域，允许射线投射，拦截事件
                return true;
            }
            // 在目标区域内，不允许射线拦截，将进行事件的穿透
            return !RectTransformUtility.RectangleContainsScreenPoint(PassThroughTargetRT, screenPoint, eventCamera);
        }
    }
}