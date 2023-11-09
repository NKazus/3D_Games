using UnityEngine;
using UnityEngine.UI;
using Zenject;
using MEGame.Interactions;

namespace MEGame.Setup
{
    public class BonusesSetup : MonoBehaviour
    {
        [SerializeField] private DailyReward rewardComponent;

        [SerializeField] private Button bondButton;
        [SerializeField] private Button speedButton;
        [SerializeField] private Button linkButton;

        [SerializeField] private int bondPrice;
        [SerializeField] private int speedPrice;
        [SerializeField] private int linkPrice;

        [SerializeField] private Text bondPriceText;
        [SerializeField] private Text speedPriceText;
        [SerializeField] private Text linkPriceText;

        [SerializeField] private Text linksNumber;
        [SerializeField] private Text bondEnabled;
        [SerializeField] private Text speedEnabled;

        [Inject] private readonly GameResourceHandler resources;
        [Inject] private readonly GameGlobalEvents globalEvents;

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

        private void Start()
        {
            rewardComponent.SetClaimCallback(CheckBonuses);
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
            bool bonusEnabled;
            switch (type)
            {
                case ResourceType.Speed:
                    cash = resources.PlayerPoints >= speedPrice;
                    speedPriceText.color = cash ? Color.white : Color.red;
                    bonusEnabled = resources.GrabSpeed;
                    speedEnabled.enabled = bonusEnabled;
                    speedButton.interactable = (!bonusEnabled && cash);
                    break;
                case ResourceType.Link:
                    cash = resources.PlayerPoints >= linkPrice;
                    linksNumber.text = resources.LinkCharges.ToString();
                    linkPriceText.color = cash ? Color.white : Color.red;
                    linkButton.interactable = cash;
                    break;
                case ResourceType.Bond:
                    cash = resources.PlayerPoints >= bondPrice;
                    bondPriceText.color = cash ? Color.white : Color.red;
                    bonusEnabled = resources.BondLength;
                    bondEnabled.enabled = bonusEnabled;
                    bondButton.interactable = (!bonusEnabled && cash);
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
                case ResourceType.Bond: price = bondPrice; break;
                case ResourceType.Speed: price = speedPrice; break;
                case ResourceType.Link: price = linkPrice; break;
                default: throw new System.NotSupportedException();
            }
            resources.ChangePointsValue(-price);
            resources.ChangeBonusValue(type, true, 1);
            globalEvents.PlayAudio(AudioType.Points);

            CheckBonuses();
        }

    }
}
