using UnityEngine;

namespace MEGame.Interactions
{
    public enum ResourceType
    {
        Points,
        Bond, //bond length
        Speed, //player grabbing
        Link //restoration
    }

    public class GameResourceHandler : MonoBehaviour
    {
        [SerializeField] private GameResourceHolder resourceHolder;
        [SerializeField] private int playerPoints;

        private bool bondLength;
        private bool grabSpeed;

        private int linkCharges;
        private System.DateTime rewardDate;

        public int PlayerPoints => playerPoints;
        public int LinkCharges => linkCharges;
        public System.DateTime RewardDate => rewardDate;

        public bool BondLength => bondLength;
        public bool GrabSpeed => grabSpeed;

        private void OnEnable()
        {
            playerPoints = 10;// PlayerPrefs.HasKey("INT_PlayerPoints") ? PlayerPrefs.GetInt("INT_PlayerPoints") : playerPoints;
            linkCharges = 5;// PlayerPrefs.HasKey("INT_LinkCharges") ? PlayerPrefs.GetInt("INT_LinkCharges") : linkCharges;

            bondLength = false;// true; //PlayerPrefs.HasKey("BOOL_BondLength") ? (PlayerPrefs.GetInt("BOOL_BondLength") == 1) : false;
            grabSpeed = false; // true;// PlayerPrefs.HasKey("BOOL_GrabSpeed") ? (PlayerPrefs.GetInt("BOOL_GrabSpeed") == 1) : false;

            rewardDate = PlayerPrefs.HasKey("INT_RewardDate") ? new System.DateTime(
               System.Convert.ToInt64(PlayerPrefs.GetString("INT_RewardDate")))
               .ToLocalTime() : System.DateTime.Now.AddDays(-2);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("INT_PlayerPoints", playerPoints);
            PlayerPrefs.SetInt("INT_LinkCharges", linkCharges);

            PlayerPrefs.SetInt("BOOL_BondLength", bondLength ? 1 : 0);
            PlayerPrefs.SetInt("BOOL_GrabSpeed", grabSpeed ? 1 : 0);

            PlayerPrefs.SetString("INT_RewardDate", "" + rewardDate.ToUniversalTime().Ticks);
        }

        public void ChangeBonusValue(ResourceType id, bool activate, int bonusValue = 0)
        {
            bool value = activate;
            switch (id)
            {
                case ResourceType.Bond: bondLength = activate; break;
                case ResourceType.Speed: grabSpeed = activate; break;
                case ResourceType.Link:
                    linkCharges += bonusValue;
                    if (linkCharges < 0)
                    {
                        linkCharges = 0;
                    }
                    resourceHolder.UpdateText(id, linkCharges);
                    value = linkCharges > 0;
                    break;
                default: throw new System.NotSupportedException();
            }
            resourceHolder.UpdateImages(id, value);
        }

        public void RefreshBonuses(bool reset = false)
        {
            if (reset)
            {
                ChangeBonusValue(ResourceType.Bond, false);
                ChangeBonusValue(ResourceType.Speed, false);
                return;
            }
            resourceHolder.UpdateImages(ResourceType.Bond, bondLength);
            resourceHolder.UpdateImages(ResourceType.Speed, grabSpeed);
            ChangeBonusValue(ResourceType.Link, false);
        }

        public void ChangePointsValue(int value)
        {
            playerPoints += value;
            if (playerPoints < 0)
            {
                playerPoints = 0;
            }
            resourceHolder.UpdateText(ResourceType.Points, playerPoints);
        }

        public void ChangeRewardDate(System.DateTime newDate)
        {
            rewardDate = newDate;
        }
    }
}
