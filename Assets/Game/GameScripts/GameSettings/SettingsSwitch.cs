using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSwitch : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;

    private Toggle toggle;
    private float handlePositionX;

    #region MONO
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePositionX = handleRectTransform.anchoredPosition.x;
        if (toggle.isOn)
        {
            Switch(true);
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

    private void Switch(bool isOn, bool animate = false)
    {
        if (animate)
        {
            handleRectTransform.DOAnchorPosX(isOn ? handlePositionX * (-1) : handlePositionX, .4f).SetId(this).SetEase(Ease.InOutBack).Play();
        }
        else
        {
            handleRectTransform.anchoredPosition = new Vector2(isOn ? handlePositionX * (-1) : handlePositionX, handleRectTransform.anchoredPosition.y);
        }
    }
}
