using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Setup : MonoBehaviour
{
    [SerializeField] private Button timeB;
    [SerializeField] private Button chargesB;
    [SerializeField] private int chargeP;
    [SerializeField] private int timeP;
    [SerializeField] private Text timePText;
    [SerializeField] private Text chargePText;


    [SerializeField] private Text timeText;
    [SerializeField] private string activeText;
    [SerializeField] private string inactiveText;

    [Inject] private readonly GameData data;
    [Inject] private readonly EventManager events;

    private void Awake()
    {
        timePText.text = timeP.ToString();
        chargePText.text = chargeP.ToString();
    }

    private void OnEnable()
    {
        data.UpdateCharges(0);
        data.UpdateMoney(0);

        chargesB.onClick.AddListener(UpgradeCharge);
        timeB.onClick.AddListener(UpgradeTime);

        CheckSetup();
    }

    private void OnDisable()
    {
        chargesB.onClick.RemoveAllListeners();
        timeB.onClick.RemoveAllListeners();
    }

    private void CheckSetup()
    {
        timeB.interactable = data.Money >= timeP && !data.TimeScale;
        chargesB.interactable = data.Money >= chargeP;
        timeText.text = data.TimeScale ? activeText : inactiveText;
    }

    private void UpgradeTime()
    {
        events.PlaySound(AudioEffect.Resource);
        data.UpdateMoney(-timeP);
        data.UpdateTime(true);
        CheckSetup();
    }

    private void UpgradeCharge()
    {
        events.PlaySound(AudioEffect.Resource);
        data.UpdateMoney(-chargeP);
        data.UpdateCharges(1);
        CheckSetup();
    }
}
