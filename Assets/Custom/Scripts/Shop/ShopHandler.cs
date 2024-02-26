using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CMGame.Setup
{
    public class ShopHandler : MonoBehaviour
    {
        [SerializeField] private PlayerReward rewardComponent;

        [SerializeField] private Button addButton;
        [SerializeField] private Button switchButton;

        [SerializeField] private int addPrice;
        [SerializeField] private int switchPrice;

        [SerializeField] private Text addPriceText;
        [SerializeField] private Text switchPriceText;

        [Inject] private readonly GameDataManager resources;
        [Inject] private readonly GameEvents globalEvents;

        private void Awake()
        {
            addPriceText.text = addPrice.ToString();
            switchPriceText.text = switchPrice.ToString();
        }

        private void OnEnable()
        {
            resources.RefreshData();
            CheckBonuses();

            addButton.onClick.AddListener(() => PickBonus(DataType.Adds));
            switchButton.onClick.AddListener(() => PickBonus(DataType.Switches));
        }

        private void Start()
        {
            rewardComponent.SetClaimCallback(CheckBonuses);
        }

        private void OnDisable()
        {
            addButton.onClick.RemoveAllListeners();
            switchButton.onClick.RemoveAllListeners();
        }

        private void CheckBonuses()
        {
            CheckBuff(DataType.Adds);
            CheckBuff(DataType.Switches);
        }

        private void CheckBuff(DataType type)
        {
            bool cash;
            int currentPointsValue = resources.GetData(DataType.Points);

            switch (type)
            {
                case DataType.Adds:
                    cash = currentPointsValue >= addPrice;
                    addPriceText.color = cash ? Color.gray : Color.red;
                    addButton.interactable = cash;
                    break;
                case DataType.Switches:
                    cash = currentPointsValue >= switchPrice;
                    switchPriceText.color = cash ? Color.gray : Color.red;
                    switchButton.interactable = cash;
                    break;
                default:
                    throw new System.NotSupportedException();
            }
        }

        private void PickBonus(DataType type)
        {
            int price;
            switch (type)
            {
                case DataType.Adds: price = addPrice; break;
                case DataType.Switches: price = switchPrice; break;
                default: throw new System.NotSupportedException();
            }
            resources.UpdateData(DataType.Points, -price);
            resources.UpdateData(type, 1);
            globalEvents.PlaySound(GameAudio.UI);

            CheckBonuses();
        }

    }
}
