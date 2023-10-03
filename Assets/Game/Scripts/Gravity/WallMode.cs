using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WallMode : MonoBehaviour
{
    [SerializeField] private Image staticBg;
    [SerializeField] private Color movingColor;
    [SerializeField] private Color staticColor;
    [SerializeField] private Text staticText;
    [SerializeField] private string staticMessage;
    [SerializeField] private string movingMessage;

    public void SwitchMode(bool isStatic)
    {
        staticBg.DOColor(isStatic ? staticColor : movingColor, 0.4f);
        staticText.DOColor(isStatic ? staticColor : movingColor, 0.4f);
        staticText.text = isStatic ? staticMessage : movingMessage;
    }
}
