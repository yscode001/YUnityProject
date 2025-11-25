using TMPro;
using UnityEngine;

namespace YUIControl
{
    public class I18N_TMP_UGUI : TextMeshProUGUI
    {
        [Header("简体")]
        [TextArea(3, 10)]
        [SerializeField] private string Jian;

        [Header("繁体")]
        [TextArea(3, 10)]
        [SerializeField] private string Fan;

        [Header("Editor预览：是否显示简体")]
        [SerializeField] private bool ShowJian = true;

        private bool IsSetedByText = false;

        public override string text
        {
            get => base.text;
            set
            {
                IsSetedByText = true;
                base.text = value;
            }
        }

        public void Init(bool is_zhcn)
        {
            font = Resources.Load<TMP_FontAsset>("Fonts/Font SDF");
            if (IsSetedByText == false)
            {
                text = is_zhcn ? Jian : Fan;
            }
        }

#if UNITY_EDITOR
        // 在编辑器中实时预览效果
        protected override void OnValidate()
        {
            base.OnValidate();
            if (UnityEditor.EditorApplication.isPlaying == false)
            {
                if (ShowJian && !string.IsNullOrWhiteSpace(Jian) && text != Jian)
                {
                    text = Jian;
                }
                else if (!ShowJian && !string.IsNullOrWhiteSpace(Fan) && text != Fan)
                {
                    text = Fan;
                }
            }
        }
#endif

        #region 以下临时使用，用于Text转换为TMP
        public void SetJianStr(string jian) { Jian = jian; }
        public void SetFanStr(string fan) { Fan = fan; }
        public void SetShowJian(bool showJian) { ShowJian = showJian; }
        #endregion
    }
}