using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Image onText;
    [SerializeField] private Image offText;

    private Toggle toggle;
    private Vector2 handlePositionOff;
    private Vector2 handlePositionOn;

    #region MONO
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePositionOff = handleRectTransform.anchoredPosition;
        handlePositionOn = new Vector2(-handlePositionOff.x, handlePositionOff.y);
        if (toggle.isOn)
        {
            Switch(true, false);
        }
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener((isOn) => Switch(isOn));
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
    #endregion

    private void Switch(bool isOn, bool animate = true)
    {
        if (animate)
        {
            onText.enabled = offText.enabled = false;
            handleRectTransform.DOAnchorPos(isOn ? handlePositionOn : handlePositionOff, .4f)
                .SetId(this)
                .SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    onText.enabled = isOn;
                    offText.enabled = !isOn;
                });
        }
        else
        {
            handleRectTransform.anchoredPosition = isOn ? handlePositionOn : handlePositionOff;
            onText.enabled = isOn;
            offText.enabled = !isOn;
        }
    }
}
