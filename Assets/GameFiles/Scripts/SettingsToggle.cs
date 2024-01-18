using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Sprite onIcon;
    [SerializeField] private Sprite offIcon;
    [SerializeField] private Image onText;
    [SerializeField] private Image offText;

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
            Switch(true, false);
        }
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener((bool isOn) => Switch(isOn, true));
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    private void Switch(bool isOn, bool animate)
    {
        if (animate)
        {
            onText.enabled = offText.enabled = false;
            handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f)
                .SetId(this)
                .SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    onText.enabled = isOn;
                    offText.enabled = !isOn;
                    handleImage.sprite = isOn ? onIcon : offIcon;
                });
        }
        else
        {
            handleRectTransform.anchoredPosition = isOn ? handlePosition * (-1) : handlePosition;
            onText.enabled = isOn;
            offText.enabled = !isOn;
            handleImage.sprite = isOn ? onIcon : offIcon;
        }
    }
}
