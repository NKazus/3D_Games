using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsVisual : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Text onText;
    [SerializeField] private Text offText;
    [SerializeField] private Sprite onImage;
    [SerializeField] private Sprite offImage;

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
            handleRectTransform.anchoredPosition = toggle.isOn ? handlePosition * (-1) : handlePosition;
            onText.enabled = true;
            offText.enabled = false;
            handleImage.sprite = onImage;
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
        onText.enabled = false;
        offText.enabled = false;
        handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f)
            .SetId("game_settings")
            .SetEase(Ease.InOutBack)
            .OnKill(() =>
            {
                onText.enabled = isOn;
                offText.enabled = !isOn;
                handleImage.sprite = isOn ? onImage : offImage;
            });        
    }
}
