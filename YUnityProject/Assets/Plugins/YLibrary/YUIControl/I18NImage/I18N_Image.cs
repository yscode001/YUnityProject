using UnityEngine;
using UnityEngine.UI;

namespace YUIControl
{
    public class I18N_Image : Image
    {
        [Header("简体")]
        [SerializeField] private Sprite Jian;

        [Header("繁体")]
        [SerializeField] private Sprite Fan;

        [Header("Editor预览：是否显示简体")]
        [SerializeField] private bool ShowJian = true;

        public Sprite JianVal => Jian;
        public Sprite FanVal => Fan;
        public bool ShowJianVal => ShowJian;

        private bool IsSetedBySprite = false;

        public new Sprite sprite
        {
            get => base.sprite;
            set
            {
                IsSetedBySprite = true;
                base.sprite = value;
            }
        }

        public void Init(LanguageEnum languageEnum)
        {
            if (IsSetedBySprite == false)
            {
                sprite = languageEnum == LanguageEnum.zhch ? Jian : Fan;
            }
        }

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