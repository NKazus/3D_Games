using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ActionSystem : MonoBehaviour
{
    [SerializeField] private int baseActionCount;

    [SerializeField] private Button switchButton;
    [SerializeField] private Image statusImage;
    [SerializeField] private Text baseUsesText;

    private int baseUses;
    private int freeUses;
    private bool freeActionEnabled;

    [Inject] private readonly AppResourceManager resources;

    private void OnEnable()
    {
        switchButton.onClick.AddListener(Switch);
    }

    private void OnDisable()
    {
        switchButton.onClick.RemoveListener(Switch);
    }

    private void Switch()
    {
        freeActionEnabled = !freeActionEnabled;
        ChangeStatus();
    }

    private void ChangeStatus()
    {
        statusImage.color = freeActionEnabled ? Color.green : Color.magenta;
    }

    private bool CheckFreeUses()
    {
        if (freeUses <= 0)
        {
            switchButton.interactable = false;
            return false;
        }
        switchButton.interactable = true;
        return true;
    }

    private bool UseFreeAction()
    {
        if (!freeActionEnabled)
        {
            return false;
        }

        resources.UpdateRes(PlayerRes.FreeAction, -1);
        freeUses--;

        if (!CheckFreeUses())
        {
            freeActionEnabled = false;
            ChangeStatus();
        }

        return true;
    }

    public void ResetActions()
    {
        baseUses = baseActionCount;
        baseUsesText.text = baseUses.ToString();

        freeUses = resources.GetResValue(PlayerRes.FreeAction);

        freeActionEnabled = false;
        ChangeStatus();
        CheckFreeUses();
    }

    public bool UseAction()
    {
        if (!UseFreeAction())
        {
            baseUses--;
            baseUsesText.text = baseUses.ToString();
            return false;
        }
        return true;
    }

    public bool CheckActions()
    {        
        return baseUses > 0;
    }
}
