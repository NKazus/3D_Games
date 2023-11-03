using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BonusesSetup : MonoBehaviour
{
    [SerializeField] private Button bondButton;
    [SerializeField] private Button speedButton;// view already enabled status
    [SerializeField] private Button linkButton;

    [SerializeField] private int bondPrice;
    [SerializeField] private int speedPrice;
    [SerializeField] private int linkPrice;

    [SerializeField] private Text bondPriceText;
    [SerializeField] private Text speedPriceText;
    [SerializeField] private Text linkPriceText;

    //for links show current number from resources

    [Inject] private readonly GameResourceHandler resources;

    private void Awake()
    {
        bondPriceText.text = bondPrice.ToString();
        linkPriceText.text = linkPrice.ToString();
        speedPriceText.text = speedPrice.ToString();
    }

    private void OnEnable()
    {
        resources.ChangePointsValue(0);
        CheckBonuses();

        bondButton.onClick.AddListener(() => PickBonus(ResourceType.Bond));
        speedButton.onClick.AddListener(() => PickBonus(ResourceType.Speed));
        linkButton.onClick.AddListener(() => PickBonus(ResourceType.Link));
    }

    private void OnDisable()
    {
        bondButton.onClick.RemoveAllListeners();
        speedButton.onClick.RemoveAllListeners();
        linkButton.onClick.RemoveAllListeners();
    }

    private void CheckBonuses()
    {
        CheckBuff(ResourceType.Bond);
        CheckBuff(ResourceType.Speed);
        CheckBuff(ResourceType.Link);
    }

    private void CheckBuff(ResourceType type)
    {
        bool cash;
        switch (type)
        {
            case ResourceType.Speed:
                cash = resources.PlayerPoints >= speedPrice;
                speedPriceText.color = cash ? Color.white : Color.red;
                speedButton.interactable = (!resources.GrabSpeed && cash);
                break;
            case ResourceType.Link:
                cash = resources.PlayerPoints >= speedPrice;
                linkButton.interactable = cash;
                break;
            case ResourceType.Bond:
                cash = resources.PlayerPoints >= bondPrice;
                bondPriceText.color = cash ? Color.white : Color.red;
                bondButton.interactable = (!resources.BondLength && cash);
                break;
            default:
                throw new System.NotSupportedException();
        }
    }

    private void PickBonus(ResourceType type)
    {
        int price;
        switch (type)
        {
            case ResourceType.Bond: price = speedPrice; break;
            case ResourceType.Speed: price = linkPrice; break;
            case ResourceType.Link: price = bondPrice; break;
            default: throw new System.NotSupportedException();
        }
        resources.ChangePointsValue(-price);
        resources.ChangeBonusValue(type, true, 1);
        CheckBonuses();
    }

}
