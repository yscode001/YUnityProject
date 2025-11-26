using UnityEngine;

namespace YUIControl
{
    public class I18N_Image : UnityEngine.UI.Image
    {
        [Header("简体")]
        [SerializeField] private Sprite Jian;

        [Header("繁体")]
        [SerializeField] private Sprite Fan;

        [Header("Editor预览：是否显示简体")]
        [SerializeField] private bool ShowJian = true;

        private bool IsSetedBySprite = false;

        public new Sprite sprite
        {
            get => base.sprite;
            set
            {
                IsSetedBySprite = true;
                base.sprite = value;
                SetAllDirty();
            }
        }

        public void Init(LanguageEnum languageEnum)
        {
            if (IsSetedBySprite == false)
            {
                sprite = languageEnum == LanguageEnum.zhch ? Jian : Fan;
            }
        }

        #region 默认禁用交互
        protected override void Reset()
        {
            base.Reset();
            raycastTarget = false;
        }
        #endregion

#if UNITY_EDITOR
        // 在编辑器中实时预览效果
        protected override void OnValidate()
        {
            base.OnValidate();
            if (UnityEditor.EditorApplication.isPlaying == false)
            {
                if (ShowJian && Jian != null && sprite != Jian)
                {
                    sprite = Jian;
                }
                else if (!ShowJian && Fan != null && sprite != Fan)
                {
                    sprite = Fan;
                }
            }
        }
#endif
    }
}