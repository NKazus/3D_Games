using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum SetupOption
{
    ShieldAdd,
    PollenAdd,
    ShieldUpgrade,
    PollenUpgrade
}

public class Setup : MonoBehaviour
{
    [SerializeField] private Button shieldButton;
    [SerializeField] private Button pollenButton;
    [SerializeField] private Button shieldUpgradeButton;
    [SerializeField] private Button pollenUpgradeButton;

    [SerializeField] private int shieldPrice;
    [SerializeField] private int pollenPrice;
    [SerializeField] private int shieldUpgradePrice;
    [SerializeField] private int pollenUpgradePrice;

    [SerializeField] private Text shieldText;
    [SerializeField] private Text pollenText;
    [SerializeField] private Text shieldUpgradeText;
    [SerializeField] private Text pollenUpgradeText;

    [Inject] private readonly InGameResources resources;
    [Inject] private readonly InGameEvents events;

    private void Awake()
    {
        shieldText.text = shieldPrice.ToString();
        pollenText.text = pollenPrice.ToString();
        shieldUpgradeText.text = shieldUpgradePrice.ToString();
        pollenUpgradeText.text = pollenUpgradePrice.ToString();
    }

    private void OnEnable()
    {
        resources.UpdatePlayerIncome(0);
        CheckBuffs();

        shieldButton.onClick.AddListener(() => PickBuff(SetupOption.ShieldAdd));
        pollenButton.onClick.AddListener(() => PickBuff(SetupOption.PollenAdd));
        shieldUpgradeButton.onClick.AddListener(() => PickBuff(SetupOption.ShieldUpgrade));
        pollenUpgradeButton.onClick.AddListener(() => PickBuff(SetupOption.PollenUpgrade));

    }

    private void OnDisable()
    {
        shieldButton.onClick.RemoveAllListeners();
        pollenButton.onClick.RemoveAllListeners();
        shieldUpgradeButton.onClick.RemoveAllListeners();
        pollenUpgradeButton.onClick.RemoveAllListeners();
    }

    private void CheckBuffs()
    {
        CheckBuff(SetupOption.ShieldAdd);
        CheckBuff(SetupOption.PollenAdd);
        CheckBuff(SetupOption.ShieldUpgrade);
        CheckBuff(SetupOption.PollenUpgrade);
    }

    private void CheckBuff(SetupOption option)
    {
        bool cash;
        switch (option)
        {
            case SetupOption.ShieldAdd:                
                cash = resources.PlayerIncome >= shieldPrice;
                shieldText.color = cash ? Color.white : Color.red;
                shieldButton.interactable = cash;
                break;
            case SetupOption.PollenAdd:
                cash = resources.PlayerIncome >= pollenPrice;
                pollenText.color = cash ? Color.white : Color.red;
                pollenButton.interactable = cash;
                break;
            case SetupOption.ShieldUpgrade:
                cash = resources.PlayerIncome >= shieldUpgradePrice;
                shieldUpgradeText.color = cash ? Color.white : Color.red;
                shieldUpgradeButton.interactable = cash && !resources.ShieldUpgrade;
                break;
            case SetupOption.PollenUpgrade:
                cash = resources.PlayerIncome >= pollenUpgradePrice;
                pollenUpgradeText.color = cash ? Color.white : Color.red;
                pollenUpgradeButton.interactable = cash && !resources.PollenUpgrade;
                break;
            default: throw new System.NotSupportedException();
        }
    }

    private void PickBuff(SetupOption option)
    {
        int price;
        switch (option)
        {
            case SetupOption.ShieldAdd:
                price = shieldPrice;
                resources.UpdateResource(AbilityType.Shield, 1);
                break;
            case SetupOption.PollenAdd:
                price = pollenPrice;
                resources.UpdateResource(AbilityType.Pollen, 1);
                break;
            case SetupOption.ShieldUpgrade:
                price = shieldUpgradePrice;
                resources.UpgradeTool(AbilityType.Shield, true);
                break;
            case SetupOption.PollenUpgrade:
                price = pollenUpgradePrice;
                resources.UpgradeTool(AbilityType.Pollen, true);
                break;
            default: throw new System.NotSupportedException();
        }
        resources.UpdatePlayerIncome(-price);

        events.PlaySFX(SFXType.CoinSound);
        CheckBuffs();
    }
}
