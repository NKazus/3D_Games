using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Image toggleBg;
    [SerializeField] private Text onText;
    [SerializeField] private Text offText;

    private Toggle toggle;
    private Vector2 handlePosition;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;

        if (toggle.isOn)
        {
            handleRectTransform.anchoredPosition = handlePosition * (-1);
            toggleBg.color = Color.yellow;
            onText.enabled = true;
            offText.enabled = false;
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
        onText.enabled = offText.enabled = false;
        handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f)
            .SetId(this)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                toggleBg.color = isOn ? Color.yellow : Color.red;
                onText.enabled = isOn;
                offText.enabled = !isOn;
            });
    }
}
