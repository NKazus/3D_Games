using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Text onText;
    [SerializeField] private Text offText;

    private Toggle toggle;
    private Vector2 handlePosition;

    #region MONO
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;
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
    #endregion

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
                });        
        }
        else
        {
            handleRectTransform.anchoredPosition = isOn ? handlePosition * (-1) : handlePosition;
            onText.enabled = isOn;
            offText.enabled = !isOn;
        }
    }
}
