using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TradeManager : MonoBehaviour
{
    [SerializeField] private Button museumButton;
    [SerializeField] private Button marketButton;
    [SerializeField] private Button tradeButton;

    [SerializeField] private Text optionText;
    [SerializeField] private string shovelText;
    [SerializeField] private string insightText;
    [SerializeField] private string oreText;

    [SerializeField] private Color emptyColor;
    [SerializeField] private Color shovelColor;
    [SerializeField] private Color insightColor;

    [SerializeField] private OreSample[] oreSamples;

    private bool shovelTool;
    private bool isTradeActive;
    private Image museumIcon;
    private Image marketIcon;

    [Inject] private readonly GameDataHandler dataHandler;
    [Inject] private readonly GlobalEventManager eventManager;

    private void Awake()
    {
        museumIcon = museumButton.transform.GetChild(0).GetComponent<Image>();
        marketIcon = marketButton.transform.GetChild(0).GetComponent<Image>();

        for (int i = 0; i < oreSamples.Length; i++)
        {
            oreSamples[i].Init();
        }
    }

    private void OnEnable()
    {
        CheckOre();
        optionText.enabled = false;
        isTradeActive = false;
        tradeButton.image.color = Color.gray;
    }

    private void OnDisable()
    {
        tradeButton.onClick.RemoveAllListeners();
    }

    private void PickOption(bool isShovel)
    {
        shovelTool = isShovel;
        optionText.text = isShovel ? shovelText : insightText;
        optionText.color = isShovel ? shovelColor : insightColor;
        optionText.enabled = true;

        if (isShovel)
        {
            for (int i = 1; i < oreSamples.Length; i++)
            {
                oreSamples[i].Hide();
            }
        }
        else
        {
            for (int i = 1; i < oreSamples.Length; i++)
            {
                oreSamples[i].Show();
            }
        }        

        if (!isTradeActive)
        {
            SwitchTradeState(true);
        }
    }

    private void PickEmpty()
    {
        optionText.text = oreText;
        optionText.color = emptyColor;
        optionText.enabled = true;

        if (isTradeActive)
        {
            SwitchTradeState(false);
        }
    }

    private void Trade()
    {
        int oreCount = shovelTool ? 1 : 3;
        dataHandler.UpdateGlobalScore(-oreCount);
        dataHandler.AddBonus(shovelTool);

        if (shovelTool)
        {
            eventManager.PlayMarket();
        }
        else
        {
            eventManager.PlayMuseum();
        }

        CheckOre();
    }

    private void CheckOre()
    {
        museumButton.onClick.RemoveAllListeners();
        marketButton.onClick.RemoveAllListeners();

        int ore = dataHandler.OreCount;

        if(ore < 1)
        {
            museumButton.onClick.AddListener(PickEmpty);
            marketButton.onClick.AddListener(PickEmpty);
            museumIcon.DOColor(Color.gray, 0.3f);
            marketIcon.DOColor(Color.gray, 0.3f);

            for (int i = 0; i < oreSamples.Length; i++)
            {
                oreSamples[i].Hide();
            }

            if (isTradeActive)
            {
                SwitchTradeState(false);
                optionText.text = oreText;
                optionText.color = emptyColor;
            }
            return;
        }

        marketButton.onClick.AddListener(() => { PickOption(true); });
        marketIcon.DOColor(Color.white, 0.3f);
        oreSamples[0].Show();

        if (ore < 3)
        {
            museumButton.onClick.AddListener(PickEmpty);
            museumIcon.DOColor(Color.gray, 0.3f);

            if (isTradeActive && !shovelTool)
            {
                for (int i = 1; i < oreSamples.Length; i++)
                {
                    oreSamples[i].Hide();
                }

                SwitchTradeState(false);
                optionText.text = oreText;
                optionText.color = emptyColor;
            }
        }
        else
        {
            museumButton.onClick.AddListener(() => { PickOption(false); });
            museumIcon.DOColor(Color.white, 0.3f);
        }
    }

    private void SwitchTradeState(bool activate)
    {
        if (activate)
        {
            tradeButton.onClick.AddListener(Trade);
            tradeButton.image.DOColor(Color.white, 0.4f);
            isTradeActive = true;
        }
        else
        {
            tradeButton.onClick.RemoveListener(Trade);
            tradeButton.image.DOColor(Color.gray, 0.4f);
            isTradeActive = false;
        }
    }
}
