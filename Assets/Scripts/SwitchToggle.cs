using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Image toggleImage;
    [SerializeField] private Sprite onIcon;
    [SerializeField] private Sprite offIcon;

    private Toggle toggle;
    private Vector2 handlePosition;

    #region MONO
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
        toggle.onValueChanged.AddListener((bool isOn) => { Switch(isOn, true); });
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
    #endregion

    private void Switch(bool isOn, bool animate = false)
    {
        toggleImage.sprite = isOn ? onIcon : offIcon;
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
