using UnityEngine;
using UnityEngine.UI;
using Zenject;
using MEGame.Interactions;

namespace MEGame.Setup
{
    public class DailyReward : MonoBehaviour
    {
        [SerializeField] private DailyTimer timer;
        [SerializeField] private int cooldownSeconds;
        [SerializeField] private int rewardValue;
        [SerializeField] private Text rewardText;
        [SerializeField] private Button getButton;

        [SerializeField] private string enableText;
        [SerializeField] private string disableText;

        [Inject] private readonly GameResourceHandler resources;

        private void Awake()
        {
            timer.SetCallback(EnableReward);
        }

        private void OnEnable()
        {
            getButton.image.color = Color.gray;
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
            Debug.Log(resources.RewardDate);
            System.TimeSpan span = System.DateTime.Now.Subtract(resources.RewardDate);
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
            getButton.image.color = Color.gray;
            rewardText.text = disableText;

            resources.ChangePointsValue(rewardValue);
            resources.ChangeRewardDate(System.DateTime.Now);

            timer.StartTimer(cooldownSeconds);
        }

        private void EnableReward()
        {
            getButton.image.color = Color.white;
            getButton.onClick.AddListener(GetIncome);
            rewardText.text = enableText;
        }
    }
}
