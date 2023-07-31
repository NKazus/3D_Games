using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image toggleImage;
    [SerializeField] private Sprite onIcon;
    [SerializeField] private Sprite offIcon;

    private Toggle toggle;

    #region MONO
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
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
        toggleImage.sprite = isOn ? onIcon : offIcon;
        if (animate)
        {
            fillImage.DOFillAmount(isOn ? 1f : 0f, 0.4f);
        }
        else
        {
            fillImage.fillAmount = isOn ? 1f : 0f;
        }       
    }
}
