using UnityEngine;
using YUnity;

public class Test : MonoBehaviour
{

    [SerializeField] private RectTransform rt;

    private void Start()
    {
        UILayoutUtil.StretchVertical_HorizontalCenter(rt, HorizontalType.Right, 100, 200, 60, 300);
    }
}