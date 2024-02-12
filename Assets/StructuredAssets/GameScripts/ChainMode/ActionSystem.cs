using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ActionSystem : MonoBehaviour
{
    [SerializeField] private int baseActionCount;

    [SerializeField] private Button switchButton;
    [SerializeField] private Image actionImage;
    [SerializeField] private Sprite freeEnabled;
    [SerializeField] private Sprite freeDisabled;
    [SerializeField] private Sprite freeActive;

    [SerializeField] private Text baseUsesText;
    [SerializeField] private Sprite actionEnabled;
    [SerializeField] private Sprite actionDisabled;

    private int baseUses;
    private int freeUses;
    private bool freeActionEnabled;

    private Image freeActionImage;

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
        freeActionImage.sprite = freeActionEnabled ? freeActive : freeEnabled;
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
            freeActionImage.sprite = freeDisabled;
        }

        return true;
    }

    public void Initialize()
    {
        freeActionImage = switchButton.image;
    }

    public void ResetActions()
    {
        baseUses = baseActionCount;
        baseUsesText.text = baseUses.ToString();
        actionImage.sprite = actionEnabled;

        freeUses = resources.GetResValue(PlayerRes.FreeAction);

        freeActionEnabled = false;
        ChangeStatus();
        if (!CheckFreeUses())
        {
            freeActionImage.sprite = freeDisabled;
        }
    }

    public bool UseAction()
    {
        if (!UseFreeAction())
        {
            baseUses--;
            baseUsesText.text = baseUses.ToString();
            if (!CheckActions())
            {
                actionImage.sprite = actionDisabled;
            }

            return false;
        }
        return true;
    }

    public bool CheckActions()
    {        
        return baseUses > 0;
    }
}
