using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsVisual : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;

    private Toggle settingsToggle;
    private Vector2 handlePosition;

    private void Awake()
    {
        settingsToggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;
        if (settingsToggle.isOn)
        {
            SwitchVisual(true);
        }
    }

    private void OnEnable()
    {
        settingsToggle.onValueChanged.AddListener((bool isOn) => { SwitchVisual(isOn, true); });
    }

    private void OnDisable()
    {
        settingsToggle.onValueChanged.RemoveAllListeners();
    }

    private void SwitchVisual(bool isOn, bool animate = false)
    {
        if (animate)
        {
            handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f).SetId(this).SetEase(Ease.InOutBack);
        }
        else
        {
            handleRectTransform.anchoredPosition = isOn ? handlePosition * (-1) : handlePosition;
        }
    }
}
