using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Sprite onIcon;
    [SerializeField] private Sprite offIcon;

    private Toggle toggle;
    private Vector2 handlePosition;
    private Image handleImage;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;
        handleImage = handleRectTransform.GetComponent<Image>();

        if (toggle.isOn)
        {
            handleRectTransform.anchoredPosition = handlePosition * (-1);
            handleImage.sprite = onIcon;
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
        handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f)
            .SetId(this)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                handleImage.sprite = isOn ? onIcon : offIcon;
            });
    }
}
