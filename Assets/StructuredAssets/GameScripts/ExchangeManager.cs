using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ExchangeManager : MonoBehaviour
{
    [SerializeField] private Button luxButton;
    [SerializeField] private Button ordinaryButton;
    [SerializeField] private Button poorButton;

    [SerializeField] private Button exchangeButton;

    [SerializeField] private Text actionsText;

    [SerializeField] private int luxExchange;
    [SerializeField] private int ordinaryExchange;
    [SerializeField] private int poorExchange;

    [SerializeField] private Text optionText;
    [SerializeField] private string medalText;

    [SerializeField] private Image optionTextBg;

    [SerializeField] private Color poorColor;
    [SerializeField] private Color ordinaryColor;
    [SerializeField] private Color luxColor;
    [SerializeField] private Color medalColor;

    private bool isTradeActive;

    private PlayerRes pickedMedal;
    private int pickedValue;

    [Inject] private readonly AppResourceManager resourceManager;
    [Inject] private readonly AppEvents events;

    private void OnEnable()
    {
        resourceManager.Refresh();
        CheckMedals();
        optionText.enabled = false;
        optionTextBg.enabled = false;
        isTradeActive = false;
        pickedMedal = PlayerRes.FreeAction;
        exchangeButton.image.color = Color.gray;
    }

    private void OnDisable()
    {
        exchangeButton.onClick.RemoveAllListeners();
    }

    private void PickOption(PlayerRes type, int value)
    {
        pickedMedal = type;
        pickedValue = value;

        optionText.color = type switch
        {
            PlayerRes.PoorMedal => poorColor,
            PlayerRes.OrdinaryMedal => ordinaryColor,
            PlayerRes.LuxMedal => luxColor,
            _ => throw new System.NotSupportedException()
        };

        optionText.text = $"You'll exchange {value} medal(s) for 1 free actions!";

        optionText.enabled = true;
        optionTextBg.enabled = true;

        if (!isTradeActive)
        {
            SwitchTradeState(true);
        }
    }

    private void PickEmpty()
    {
        optionText.text = medalText;
        optionText.color = medalColor;
        optionText.enabled = true;

        if (isTradeActive)
        {
            SwitchTradeState(false);
        }
    }

    private void Trade()
    {
        resourceManager.UpdateRes(pickedMedal, -pickedValue);
        resourceManager.UpdateRes(PlayerRes.FreeAction, 1);
        events.PlaySound(AppSound.FreeAction);

        CheckMedals();
    }

    private void CheckMedals()
    {
        actionsText.text = resourceManager.GetResValue(PlayerRes.FreeAction).ToString();

        SwitchButton(luxButton, PlayerRes.LuxMedal, luxExchange);
        SwitchButton(ordinaryButton, PlayerRes.OrdinaryMedal, ordinaryExchange);
        SwitchButton(poorButton, PlayerRes.PoorMedal, poorExchange);
    }

    private bool SwitchButton(Button target, PlayerRes type, int value)
    {
        target.onClick.RemoveAllListeners();
        if (resourceManager.GetResValue(type) >= value)
        {
            target.onClick.AddListener(() => PickOption(type, value));
            target.image.DOColor(Color.white, 0.3f);
            return true;
        }
        else
        {
            target.onClick.AddListener(() => PickEmpty());
            target.image.DOColor(Color.gray, 0.3f);
            if(pickedMedal == type)
            {
                PickEmpty();
            }
            return false;
        }
    }

    private void SwitchTradeState(bool activate)
    {
        if (activate)
        {
            exchangeButton.onClick.AddListener(Trade);
            exchangeButton.image.DOColor(Color.white, 0.4f);
            isTradeActive = true;
        }
        else
        {
            exchangeButton.onClick.RemoveListener(Trade);
            exchangeButton.image.DOColor(Color.gray, 0.4f);
            isTradeActive = false;
        }
    }
}
