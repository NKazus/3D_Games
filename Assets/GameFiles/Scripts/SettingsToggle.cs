using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Image toggleImage;
    [SerializeField] private Sprite onIcon;
    [SerializeField] private Sprite offIcon;

    private Toggle toggle;
    private Vector2 handlePosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;
        if (toggle.isOn)
        {
            Switch(true);
        }
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(Switch);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(Switch);
    }

    private void Switch(bool isOn)
    {
        toggleImage.sprite = isOn ? onIcon : offIcon;
        handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f).SetId(this).SetEase(Ease.InOutBack);        
    }
}
