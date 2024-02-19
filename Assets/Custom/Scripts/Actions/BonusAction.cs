using UnityEngine;
using UnityEngine.UI;

public class BonusAction : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Button addButton;

    private bool isEnabled;

    public void Init(System.Action callback)
    {
        addButton.onClick.AddListener(() => callback());
    }

    public void ResetBonusMenu(bool enable)
    {
        isEnabled = enable;
        Deactivate();
    }

    public void ShowBonusMenu(bool show)
    {
        targetObject.SetActive(show);
    }

    public void SwitchButton()
    {
        if (!isEnabled)
        {
            return;
        }

        addButton.interactable = !addButton.interactable;
    }

    public void Deactivate()
    {
        addButton.interactable = false;
    }
}
