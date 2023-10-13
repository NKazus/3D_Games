using UnityEngine;
using UnityEngine.UI;

public class SettingsSwitcher : MonoBehaviour
{
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
            Switch(true);
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
        toggleImage.sprite = isOn ? onIcon : offIcon;
    }
}
