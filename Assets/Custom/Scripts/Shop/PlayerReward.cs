using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CMGame.Setup
{
    public class PlayerReward : MonoBehaviour
    {
        [SerializeField] private RewardCooldown timer;
        [SerializeField] private int cooldownSeconds;
        [SerializeField] private int rewardValue;
        [SerializeField] private Text rewardText;
        [SerializeField] private Button getButton;

        [SerializeField] private string enableText;
        [SerializeField] private string disableText;

        private System.Action ClaimCallback;

        [Inject] private readonly GameDataManager resources;
        [Inject] private readonly GameEvents globalEvents;

        private void Awake()
        {
            timer.SetCallback(EnableReward);
        }

        private void OnEnable()
        {
            getButton.interactable = false;
            rewardText.text = disableText;

            CheckDaily();
        }

        private void OnDisable()
        {
            timer.StopTimer();
            getButton.onClick.AddListener(GetIncome);
        }

        private void CheckDaily()
        {
            System.TimeSpan span = System.DateTime.Now.Subtract(resources.GetRewardDate());
            int seconds = (int)span.TotalSeconds;
            bool rewardEnabled = (seconds >= cooldownSeconds);
            if (rewardEnabled)
            {
                timer.ResetTimer();
                EnableReward();
            }
            else
            {
                timer.StartTimer(cooldownSeconds - seconds);
            }
        }

        private void GetIncome()
        {
            getButton.onClick.RemoveListener(GetIncome);
            getButton.interactable = false;
            rewardText.text = disableText;

            resources.UpdateData(DataType.Points, rewardValue);
            resources.UpdateReward(System.DateTime.Now);

            globalEvents.PlayVibro();

            if (ClaimCallback != null)
            {
                ClaimCallback();
            }

            timer.StartTimer(cooldownSeconds);
        }

        private void EnableReward()
        {
            getButton.interactable = true;
            getButton.onClick.AddListener(GetIncome);
            rewardText.text = enableText;
        }

        public void SetClaimCallback(System.Action callback)
        {
            ClaimCallback = callback;
        }

    }
}
