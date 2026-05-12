using UnityEngine;
using UnityEngine.UI;
using YUnity;

public class LoginWnd : UIStackBaseWnd
{
    [SerializeField] private InputField UsernameIF;
    [SerializeField] private InputField PasswordIF;
    [SerializeField] private Text TipsText;
    [SerializeField] private Button LoginBtn;

    public override void BeforePush()
    {
        base.BeforePush();
        TipsText.text = "请输入用户名和密码";
        TipsText.color = Color.green;
    }
}