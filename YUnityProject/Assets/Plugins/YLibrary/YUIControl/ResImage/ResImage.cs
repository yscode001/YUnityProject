using UnityEngine;
using UnityEngine.UI;

namespace YUIControl
{
    /// <summary>
    /// 资源图片，用于加载本地和服务器的图片
    /// </summary>
    public class ResImage : Image
    {
        public string Identifier { get; private set; } = null;

        #region 按钮交互
        private Button _btn = null;
        /// <summary>
        /// 按钮交互，访问时如果没有Button则会进行添加操作
        /// </summary>
        public Button Btn
        {
            get
            {
                if (_btn != null) { return _btn; }
                raycastTarget = true;
                _btn = gameObject.GetComponent<Button>();
                if (_btn != null) { return _btn; }
                _btn = gameObject.AddComponent<Button>();
                return _btn;
            }
        }
        #endregion

        /// <summary>
        /// 设置标识符和精灵图片
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="spriteAsset"></param>
        /// <param name="forceSet"></param>
        public void SetIdentifierAndSprite(string identifier, Sprite spriteAsset, bool forceSet = false)
        {
            if (forceSet || Identifier != identifier)
            {
                Identifier = identifier;
                sprite = spriteAsset;
            }
        }

        /// <summary>
        /// 加载Resources里面的精灵图片
        /// </summary>
        /// <param name="resourcesPath"></param>
        public void LoadResources(string resourcesPath)
        {
            if (Identifier != resourcesPath)
            {
                Identifier = resourcesPath;
                sprite = Resources.Load<Sprite>(resourcesPath);
            }
        }

        /// <summary>
        /// 加载Server的精灵图片
        /// </summary>
        /// <param name="serverURL"></param>
        public void LoadServer(string serverURL)
        {
            if (Identifier != serverURL)
            {
                Identifier = serverURL;
                // todo 加载server sprite
                // 注意，加载是异步的，加载成功后，需再次判断Identifier
            }
        }

#if UNITY_EDITOR
        // 默认禁用交互
        protected override void Reset()
        {
            base.Reset();
            raycastTarget = false;
        }
#endif
    }
}