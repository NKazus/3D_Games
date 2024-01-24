using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;

    private Toggle toggle;
    private Vector2 handlePosition;

    #region MONO
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;
        if (toggle.isOn)
        {
            handleRectTransform.anchoredPosition = toggle.isOn ? handlePosition * (-1) : handlePosition;
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
    #endregion

    private void Switch(bool isOn)
    {
        handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f).SetId(this).SetEase(Ease.InOutBack);        
    }
}
