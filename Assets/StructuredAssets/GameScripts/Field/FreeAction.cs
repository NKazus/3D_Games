using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FreeAction : MonoBehaviour
{
    [SerializeField] private Button switchButton;
    [SerializeField] private Image statusImage;
    [SerializeField] private Text usesText;

    private int uses;
    private bool freeActionEnabled;

    [Inject] private readonly AppResourceManager resources;

    private void OnEnable()
    {
        switchButton.onClick.AddListener(Switch);

        ResetUses();
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

    private bool CheckUses()
    {
        if (uses <= 0)
        {            
            switchButton.interactable = false;
            return false;
        }
        switchButton.interactable = true;
        return true;
    }

    public bool UseFreeAction()
    {
        if (!freeActionEnabled)
        {
            return false;
        }

        resources.UpdateRes(PlayerRes.FreeAction, -1);
        uses--;
        usesText.text = uses.ToString();

        if (!CheckUses())
        {
            freeActionEnabled = false;
            ChangeStatus();
        }

        return true;
    }

    public void ResetUses()
    {
        uses = resources.GetResValue(PlayerRes.FreeAction);
        usesText.text = uses.ToString();

        freeActionEnabled = false;
        ChangeStatus();
        CheckUses();
    }
}
