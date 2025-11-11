// Author：yaoshuai
// Email：yscode@126.com
// Date：2023-10-7
// ------------------------------

using System;
using UnityEngine;

namespace YUnity
{
    public static class LayoutUtil
    {
        /// <summary>
        /// 布局遮罩图片集合，使其组成中间矩形镂空
        /// </summary>
        /// <param name="maskBox">遮罩图片集合的父容器</param>
        /// <param name="maskTop">上边的遮罩图片</param>
        /// <param name="maskBottom">下边的遮罩图片</param>
        /// <param name="maskLeft">左边的遮罩图片</param>
        /// <param name="maskRight">右边的遮罩图片</param>
        /// <param name="canvasRectTransformWidth">参考布局Canvas的RectTransform的宽度</param>
        /// <param name="canvasRectTransformHeight">参考布局Canvas的RectTransform的高度</param>
        /// <param name="hollowOutLeftDownPoint">镂空区域的左下角坐标</param>
        /// <param name="hollowOutRightTopPoint">镂空区域的右上角坐标</param>
        public static void LayoutMaskImages(RectTransform maskBox, RectTransform maskTop, RectTransform maskBottom, RectTransform maskLeft, RectTransform maskRight, float canvasRectTransformWidth, float canvasRectTransformHeight, Vector2 hollowOutLeftDownPoint, Vector2 hollowOutRightTopPoint)
        {
            // 先初始化遮罩
            maskBox.anchorMin = maskBox.anchorMax = maskBox.pivot = Vector2.zero;
            maskTop.anchorMin = maskTop.anchorMax = maskTop.pivot = Vector2.zero;
            maskBottom.anchorMin = maskBottom.anchorMax = maskBottom.pivot = Vector2.zero;
            maskLeft.anchorMin = maskLeft.anchorMax = maskLeft.pivot = Vector2.zero;
            maskRight.anchorMin = maskRight.anchorMax = maskRight.pivot = Vector2.zero;
            // 屏幕宽高和镂空坐标点
            float screenWidth = canvasRectTransformWidth;
            float screenHeight = canvasRectTransformHeight;
            Vector2 leftDown = hollowOutLeftDownPoint;
            Vector2 rightTop = hollowOutRightTopPoint;
            // 重新布局遮罩
            maskBox.anchoredPosition = Vector2.zero;
            maskBox.sizeDelta = new Vector2(screenWidth, screenHeight);

            maskTop.anchoredPosition = new Vector2(0, rightTop.y);
            maskTop.sizeDelta = new Vector2(screenWidth, screenHeight - rightTop.y);

            maskBottom.anchoredPosition = Vector2.zero;
            maskBottom.sizeDelta = new Vector2(screenWidth, leftDown.y);

            maskLeft.anchoredPosition = new Vector2(0, leftDown.y);
            maskLeft.sizeDelta = new Vector2(leftDown.x, rightTop.y - leftDown.y);

            maskRight.anchoredPosition = new Vector2(rightTop.x, leftDown.y);
            maskRight.sizeDelta = new Vector2(screenWidth - rightTop.x, rightTop.y - leftDown.y);
        }

        /// <summary>
        /// 布局遮罩图片集合，使其组成中间矩形镂空
        /// </summary>
        /// <param name="maskBox">遮罩图片集合的父容器</param>
        /// <param name="maskTop">上边的遮罩图片</param>
        /// <param name="maskBottom">下边的遮罩图片</param>
        /// <param name="maskLeft">左边的遮罩图片</param>
        /// <param name="maskRight">右边的遮罩图片</param>
        /// <param name="canvas">参考布局Canvas</param>
        /// <param name="hollowOutLeftDownPoint">镂空区域的左下角坐标</param>
        /// <param name="hollowOutRightTopPoint">镂空区域的右上角坐标</param>
        public static void LayoutMaskImages(RectTransform maskBox, RectTransform maskTop, RectTransform maskBottom, RectTransform maskLeft, RectTransform maskRight, Canvas canvas, Vector2 hollowOutLeftDownPoint, Vector2 hollowOutRightTopPoint)
        {
            Rect canvasRect = (canvas.transform as RectTransform).rect;
            LayoutMaskImages(maskBox, maskTop, maskBottom, maskLeft, maskRight, canvasRect.width, canvasRect.height, hollowOutLeftDownPoint, hollowOutRightTopPoint);
        }

        /// <summary>
        /// 布局遮罩图片集合，使其组成中间矩形镂空
        /// </summary>
        /// <param name="maskBox">遮罩图片集合的父容器</param>
        /// <param name="maskTop">上边的遮罩图片</param>
        /// <param name="maskBottom">下边的遮罩图片</param>
        /// <param name="maskLeft">左边的遮罩图片</param>
        /// <param name="maskRight">右边的遮罩图片</param>
        /// <param name="canvas">参考布局Canvas</param>
        /// <param name="targetHollowOutRectTransform">根据Target计算镂空区域的左下角右上角坐标，然后布局遮罩图片留出镂空区域</param>
        public static void LayoutMaskImages(RectTransform maskBox, RectTransform maskTop, RectTransform maskBottom, RectTransform maskLeft, RectTransform maskRight, Canvas canvas, RectTransform targetHollowOutRectTransform)
        {
            Rect canvasRect = (canvas.transform as RectTransform).rect;

            var data = GetTargetRectTransformPointsInCanvas(targetHollowOutRectTransform, canvas);
            Vector2 leftDown = data.Item1;
            Vector2 rightTop = data.Item3;

            LayoutMaskImages(maskBox, maskTop, maskBottom, maskLeft, maskRight, canvasRect.width, canvasRect.height, leftDown, rightTop);
        }

        /// <summary>
        /// 获取RectTransform的左下、左上、右上、右下、中心点在Canvas上的坐标相对于Canvas的左下角)
        /// </summary>
        /// <param name="canvas">Canvas</param>
        /// <param name="world">世界坐标点</param>
        /// <returns></returns>
        public static Tuple<Vector2,Vector2,Vector2,Vector2,Vector2> GetTargetRectTransformPointsInCanvas(RectTransform targetRectTransform, Canvas canvas)
        {
            Vector3[] _corners = new Vector3[4];
            targetRectTransform.GetWorldCorners(_corners); // 获得对象的四个角世界坐标

            _corners[0] = WorldToCanvasPoint(canvas, _corners[0]);
            _corners[1] = WorldToCanvasPoint(canvas, _corners[1]);
            _corners[2] = WorldToCanvasPoint(canvas, _corners[2]);
            _corners[3] = WorldToCanvasPoint(canvas, _corners[3]);

            float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
            float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
            Vector2 centerPos = new Vector2(x, y);

            return new Tuple<Vector2,Vector2,Vector2,Vector2,Vector2>(_corners[0],_corners[1],_corners[2],_corners[3],centerPos);
        }

        /// <summary>
        /// 将世界坐标转换为Canvas上的坐标点(相对于Canvas的左下角)
        /// </summary>
        /// <param name="canvas">Canvas</param>
        /// <param name="world">世界坐标点</param>
        /// <returns></returns>
        public static Vector2 WorldToCanvasPoint(Canvas canvas, Vector3 world)
        {
            // 把世界坐标转化为屏幕坐标
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, world);

            // 屏幕坐标转换为局部坐标
            Vector2 localPoint;
            RectTransform canvasRT = canvas.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, screenPoint, canvas.worldCamera, out localPoint);

            // 相对屏幕最小角
            localPoint.x += canvasRT.rect.width * 0.5f;
            localPoint.y += canvasRT.rect.height * 0.5f;

            return localPoint;
        }

        /// <summary>
        /// 获取屏幕4个角在某一层游戏物体上的投影(左下、左上、右上、右下)
        /// </summary>
        /// <param name="camera">射线检测的相机(一般为主相机)</param>
        /// <param name="layerMask">投影在哪一层游戏物体上</param>
        /// <returns></returns>
        public static Tuple<Tuple<bool, Vector3>, Tuple<bool, Vector3>, Tuple<bool, Vector3>, Tuple<bool, Vector3>> ScreenCornerPointsProjectionWorldPoints(Camera camera, int layerMask)
        {
            Vector3 screenLeftBottomPoint = Vector3.zero; // 屏幕左下角
            Vector3 screenLeftTopPoint = new Vector3(0, Screen.height, 0); // 屏幕左上角
            Vector3 screenRightTopPoint = new Vector3(Screen.width, Screen.height, 0); // 屏幕右上角
            Vector3 screenRightBottomPoint = new Vector3(Screen.width, 0, 0); // 屏幕右下角

            Tuple<bool, Vector3> tupleLeftBottom = new Tuple<bool, Vector3>(false, Vector3.zero); // Camera左下角在3D物体上的投影
            Tuple<bool, Vector3> tupleLeftTop = new Tuple<bool, Vector3>(false, Vector3.zero); // Camera左上角在3D物体上的投影
            Tuple<bool, Vector3> tupleRightTop = new Tuple<bool, Vector3>(false, Vector3.zero); // Camera右上角在3D物体上的投影
            Tuple<bool, Vector3> tupleRightBottom = new Tuple<bool, Vector3>(false, Vector3.zero); // Camera右下角在3D物体上的投影

            // 左下角射线检测
            Ray ray = camera.ScreenPointToRay(screenLeftBottomPoint);
            layerMask = 1 << layerMask;
            if (Physics.Raycast(ray, out RaycastHit hitLeftBottom, Mathf.Infinity, layerMask))
            {
                tupleLeftBottom = new Tuple<bool, Vector3>(true, hitLeftBottom.point);
            }
            // 左上角射线检测
            ray = camera.ScreenPointToRay(screenLeftTopPoint);
            if (Physics.Raycast(ray, out RaycastHit hitLeftTop, Mathf.Infinity, layerMask))
            {
                tupleLeftTop = new Tuple<bool, Vector3>(true, hitLeftTop.point);
            }
            // 右上角射线检测
            ray = camera.ScreenPointToRay(screenRightTopPoint);
            if (Physics.Raycast(ray, out RaycastHit hitRightTop, Mathf.Infinity, layerMask))
            {
                tupleRightTop = new Tuple<bool, Vector3>(true, hitRightTop.point);
            }
            // 右下角射线检测
            ray = camera.ScreenPointToRay(screenRightBottomPoint);
            if (Physics.Raycast(ray, out RaycastHit hitRightDown, Mathf.Infinity, layerMask))
            {
                tupleRightBottom = new Tuple<bool, Vector3>(true, hitRightDown.point);
            }

            return new Tuple<Tuple<bool, Vector3>, Tuple<bool, Vector3>, Tuple<bool, Vector3>, Tuple<bool, Vector3>>(tupleLeftBottom, tupleLeftTop, tupleRightTop, tupleRightBottom);
        }

        /// <summary>
        /// 获取两个坐标点连线上的随机一点
        /// </summary>
        /// <param name="fromPosition">起点</param>
        /// <param name="toPosition">终点</param>
        /// <returns></returns>
        public static Vector3 GetRandomPosition(Vector3 fromPosition, Vector3 toPosition)
        {
            float dis = Vector3.Distance(fromPosition, toPosition); // 计算距离
            Vector3 vector = (toPosition - fromPosition).normalized; // 向量单位化
            float rand = UnityEngine.Random.Range(0, dis); // 随机距离
            return fromPosition + vector * rand;
        }
    }
}