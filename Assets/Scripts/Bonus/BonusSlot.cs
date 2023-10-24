using UnityEngine;
using UnityEngine.UI;

public class BonusSlot : MonoBehaviour
{
    [SerializeField] private Sprite defaultButtonImage;
    [SerializeField] private Sprite clickedButtonImage;
    [SerializeField] private Sprite defaultIcon;

    private Button button;
    private Image icon;

    public void Initialize()
    {
        button = transform.GetChild(0).GetComponent<Button>();
        icon = transform.GetChild(1).GetComponent<Image>();
    }

    public void Refresh()
    {
        button.image.sprite = defaultButtonImage;
        icon.sprite = defaultIcon;
        icon.SetNativeSize();
        icon.enabled = false;
    }

    public void SetRewardIcon(Sprite reward)
    {
        icon.sprite = reward;
        icon.SetNativeSize();
    }

    public void SetListener(System.Action<BonusSlot> action)
    {
        button.onClick.AddListener(ClickButton);
        button.onClick.AddListener(delegate { action(this); });
    }

    public void ResetListener()
    {
        button.onClick.RemoveAllListeners();
    }

    public void SetInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

    public void SetBaseColor(bool interactable)
    {
        button.image.color = interactable ? Color.white : Color.gray;
    }

    private void ClickButton()
    {
        button.image.sprite = clickedButtonImage;
        button.interactable = false;
        icon.enabled = true;
    }
}
