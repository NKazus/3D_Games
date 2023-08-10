using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Market : MonoBehaviour
{
    [SerializeField] private Button propButton;
    [SerializeField] private Button seedButton;
    [SerializeField] private Button tradeButton;

    [SerializeField] private GameObject moneyText;
    [SerializeField] private Sprite seedIcon;
    [SerializeField] private Sprite propIcon;

    [SerializeField] private Transform propCrate;
    [SerializeField] private Transform seedCrate;

    private bool seed;
    private bool isTradeActive;
    private Image tradeIcon;

    [Inject] private readonly DataHandler dataHandler;
    [Inject] private readonly GlobalEventManager eventManager;

    private void Awake()
    {
        tradeIcon = tradeButton.transform.GetChild(0).GetComponent<Image>();
    }

    private void OnEnable()
    {
        CheckMoney();
        moneyText.SetActive(false);
        isTradeActive = false;
        tradeButton.image.DOFade(0.5f, 0.3f);
        tradeIcon.enabled = false;
    }

    private void OnDisable()
    {
        tradeButton.onClick.RemoveAllListeners();
    }

    private void PickOption(bool isSeed)
    {
        seed = isSeed;
        moneyText.SetActive(false);

        Transform target;

        if (isSeed)
        {
            target = seedCrate;
            tradeIcon.sprite = seedIcon;
        }
        else
        {
            target = propCrate;
            tradeIcon.sprite = propIcon;
        }
        tradeIcon.enabled = true;

        DOTween.Sequence()
            .Append(target.DOLocalMoveY(0.3f, 0.3f))
            .Append(target.DOLocalMoveY(0f, 0.3f))
            .Play();

        if (!isTradeActive)
        {
            SwitchTradeState(true);
        }
    }

    private void PickEmpty()
    {
        eventManager.PlayVibro();
        moneyText.SetActive(true);
        tradeIcon.enabled = false;

        if (isTradeActive)
        {
            SwitchTradeState(false);
        }
    }

    private void Trade()
    {
        int money = seed ? 1 : 2;
        dataHandler.UpdateMoney(-money);
        eventManager.PlayCoins();

        if (seed)
        {
            dataHandler.UpdateSeeds(1);
        }
        else
        {
            dataHandler.UpdateProps(1);
        }

        CheckMoney();
    }

    private void CheckMoney()
    {
        propButton.onClick.RemoveAllListeners();
        seedButton.onClick.RemoveAllListeners();

        int money = dataHandler.Money;

        if (money < 1)
        {
            propButton.onClick.AddListener(PickEmpty);
            seedButton.onClick.AddListener(PickEmpty);
            propButton.image.DOFade(0.5f, 0.3f);
            seedButton.image.DOFade(0.5f, 0.3f);

            if (isTradeActive)
            {
                SwitchTradeState(false);
                moneyText.SetActive(true);
            }
            return;
        }

        seedButton.onClick.AddListener(() => { PickOption(true); });
        seedButton.image.DOFade(1f, 0.3f);

        if (money < 2)
        {
            propButton.onClick.AddListener(PickEmpty);
            propButton.image.DOFade(0.5f, 0.3f);

            if (isTradeActive && !seed)
            {     
                SwitchTradeState(false);
                moneyText.SetActive(true);
            }
        }
        else
        {
            propButton.onClick.AddListener(() => { PickOption(false); });
            propButton.image.DOFade(1f, 0.3f);
        }
    }

    private void SwitchTradeState(bool activate)
    {
        if (activate)
        {
            tradeButton.onClick.AddListener(Trade);
            tradeButton.image.DOFade(1f, 0.4f);
            isTradeActive = true;
        }
        else
        {
            tradeButton.onClick.RemoveListener(Trade);
            tradeButton.image.DOFade(0.5f, 0.4f);
            isTradeActive = false;
            tradeIcon.enabled = false;
        }
    }
}
