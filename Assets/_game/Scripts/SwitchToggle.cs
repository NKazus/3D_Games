using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private Image toggleImage;
    [SerializeField] private Sprite toggleOn;
    [SerializeField] private Sprite toggleOff;

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
        toggleImage.sprite = isOn ? toggleOn : toggleOff;        
    }
}
