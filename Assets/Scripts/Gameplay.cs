using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Timer timer;

    [SerializeField] private Button foodB;
    [SerializeField] private Button fuelB;
    [SerializeField] private Button toolsB;
    [SerializeField] private Button medsB;

    [SerializeField] private Button giveB;

    [SerializeField] private Text foodO;
    [SerializeField] private Text fuelO;
    [SerializeField] private Text toolsO;
    [SerializeField] private Text medsO;

    [SerializeField] private Text foodP;
    [SerializeField] private Text fuelP;
    [SerializeField] private Text toolsP;
    [SerializeField] private Text medsP;

    [SerializeField] private GameObject orderPanel;
    [SerializeField] private GameObject playerPanel;

    private int currentRep;
    private int currentIncome;

    [Inject] private readonly Randomizer randomizer;
    [Inject] private readonly EventManager events;
    [Inject] private readonly GameData data;

    private void OnEnable()
    {
        data.UpdateAllRes(true);
        data.UpdateRep(0);
        data.UpdateMoney(0);

        currentRep = 0;

        events.GameStateEvent += SwitchGame;
    }

    private void OnDisable()
    {
        data.UpdateRep(currentRep);
        orderPanel.SetActive(false);
        playerPanel.SetActive(false);


        foodB.onClick.RemoveAllListeners();
        fuelB.onClick.RemoveAllListeners();
        toolsB.onClick.RemoveAllListeners();
        medsB.onClick.RemoveAllListeners();

        events.GameStateEvent -= SwitchGame;
    }

    private void SwitchGame(bool activate)
    {
        if (activate)
        {

            events.TimeOutEvent += TriggerTimeout;

        }
        else
        {
            foodB.onClick.RemoveAllListeners();
            fuelB.onClick.RemoveAllListeners();
            toolsB.onClick.RemoveAllListeners();
            medsB.onClick.RemoveAllListeners();

            timer.Deactivate();
            events.TimeOutEvent -= TriggerTimeout;
        }
    }

    private void ShipCallback()
    {
        events.PlaySound(AudioEffect.Timer);
        orderPanel.transform.localScale = playerPanel.transform.localScale = Vector3.zero;
        orderPanel.SetActive(true);
        playerPanel.SetActive(true);
        DOTween.Sequence()
            .SetId("game_panel")
            .Append(orderPanel.transform.DOScale(new Vector3(1, 1, 1), 0.5f))
            .Join(playerPanel.transform.DOScale(new Vector3(1, 1, 1), 0.5f));        
        timer.Activate();
    }

    private void TriggerTimeout()
    {
        currentRep -= 50;
        events.PlaySound(AudioEffect.Timer);
    }

}
