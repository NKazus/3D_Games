using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[System.Serializable]
public struct RewardPreset
{
    public ResData type;
    public Sprite icon;
}

public class DailyReward : MonoBehaviour
{
    [SerializeField] private RewardTimer timer;
    [SerializeField] private int cooldownSeconds;
    [SerializeField] private RewardPreset[] presets;
    [SerializeField] private int rewardValue;//3
    [SerializeField] private Text rewardValueText;//x reward value
    [SerializeField] private Text rewardText;//enabled/disabled
    [SerializeField] private Button getButton;

    [SerializeField] private Image rewardImage;
    [SerializeField] private Sprite defaultIcon;

    [SerializeField] private string enableText;
    [SerializeField] private string disableText;

    [Inject] private readonly GameData gameData;
    [Inject] private readonly EventHandler events;
    [Inject] private readonly ValueProvider rand;

    private void Awake()
    {
        timer.SetCallback(EnableReward);
        rewardValueText.text = "X" + rewardValue;
    }

    private void OnEnable()
    {
        getButton.interactable = false;
        rewardText.text = disableText;

        rewardImage.sprite = defaultIcon;
        rewardImage.SetNativeSize();

        rewardValueText.enabled = false;

        CheckDaily();
    }

    private void OnDisable()
    {
        timer.StopTimer();
        getButton.onClick.AddListener(GetRes);
    }

    private void CheckDaily()
    {
        System.TimeSpan span = System.DateTime.Now.Subtract(gameData.RewardDate);
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

    private void GetRes()
    {
        getButton.onClick.RemoveListener(GetRes);
        getButton.interactable = false;
        rewardText.text = disableText;

        int presetId = rand.GenerateInt(0, presets.Length);

        gameData.UpdateResData(presets[presetId].type, rewardValue);
        gameData.UpdateRewardDate(System.DateTime.Now);

        rewardImage.sprite = presets[presetId].icon;
        rewardImage.SetNativeSize();

        rewardValueText.enabled = true;

        events.PlayVibro();

        timer.StartTimer(cooldownSeconds);
    }

    private void EnableReward()
    {
        getButton.interactable = true;
        getButton.onClick.AddListener(GetRes);
        rewardText.text = enableText;
    }
}
