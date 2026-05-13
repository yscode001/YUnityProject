using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : MonoBehaviour
{
    [SerializeField] private InputField UsernameIF;
    [SerializeField] private InputField PasswordIF;
    [SerializeField] private Text TipsText;
    [SerializeField] private Button LoginBtn;

    private void Start()
    {
        TipsText.text = "请输入用户名和密码";
        TipsText.color = Color.cyan;
    }
}