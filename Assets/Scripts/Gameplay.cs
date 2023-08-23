using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private Ships ships;

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

    private Order sourceOrder;
    private Order playerOrder;

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

        giveB.onClick.RemoveListener(Give);

        foodB.onClick.RemoveAllListeners();
        fuelB.onClick.RemoveAllListeners();
        toolsB.onClick.RemoveAllListeners();
        medsB.onClick.RemoveAllListeners();

        events.GameStateEvent -= SwitchGame;
        ships.ResetShip();
    }

    private void SwitchGame(bool activate)
    {
        if (activate)
        {
            playerOrder.ResetOrder();
            UpdateOrder(true);

            ships.ActivateShip(ShipCallback);
            events.TimeOutEvent += TriggerTimeout;

            foodB.onClick.AddListener(() => PickRes(GameResources.Food));
            fuelB.onClick.AddListener(() => PickRes(GameResources.Fuel));
            toolsB.onClick.AddListener(() => PickRes(GameResources.Tools));
            medsB.onClick.AddListener(() => PickRes(GameResources.Meds));

            giveB.onClick.AddListener(Give);
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
        GenerateOrder();
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

    private void GenerateOrder()
    {
        currentIncome = 0;
        sourceOrder.food = randomizer.GenerateInt(0, 5);
        sourceOrder.fuel = randomizer.GenerateInt(0, 5);
        sourceOrder.tools = randomizer.GenerateInt(0, 5);
        sourceOrder.meds = randomizer.GenerateInt(0, 5);

        UpdateOrder();
    }

    private void Give()
    {
        giveB.onClick.RemoveListener(Give);

        timer.Deactivate();
        currentIncome = playerOrder.food + playerOrder.fuel + playerOrder.tools + playerOrder.meds;
        if(currentRep < 0)
        {
            currentIncome /= 2;
        }

        if (sourceOrder.Equals(playerOrder))
        {
            currentIncome += 5;
        }
        int misses = sourceOrder.GetMisses();
        currentRep += 10 * (4 - misses);
        currentRep -= 20 * misses;
        
        events.DoResult(currentRep, currentIncome);
        data.UpdateRep(currentRep);
        data.UpdateMoney(currentIncome);

        currentRep = 0;

        DOTween.Sequence()
            .SetId("game_panel")
            .Append(orderPanel.transform.DOScale(Vector3.zero, 0.5f))
            .Join(playerPanel.transform.DOScale(Vector3.zero, 0.5f))
            .OnKill(() => { playerPanel.SetActive(false); orderPanel.SetActive(false); });

        events.PlaySound(AudioEffect.Reward);
        events.SwitchGameState(false);
    }

    private void PickRes(GameResources id)
    {
        switch (id)
        {
            case GameResources.Food:
                if (data.Food > 0)
                {
                    playerOrder.food++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            case GameResources.Fuel:
                if (data.Fuel > 0)
                {
                    playerOrder.fuel++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            case GameResources.Tools:
                if (data.Tools > 0)
                {
                    playerOrder.tools++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            case GameResources.Meds:
                if (data.Meds > 0)
                {
                    playerOrder.meds++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            default: throw new NotSupportedException();
        }
        events.PlaySound(AudioEffect.Resource);
        UpdateOrder(true);
    }

    private void UpdateOrder(bool player = false)
    {
        if (player)
        {
            foodP.text = playerOrder.food.ToString();
            fuelP.text = playerOrder.fuel.ToString();
            toolsP.text = playerOrder.tools.ToString();
            medsP.text = playerOrder.meds.ToString();
        }
        else
        {
            foodO.text = sourceOrder.food.ToString();
            fuelO.text = sourceOrder.fuel.ToString();
            toolsO.text = sourceOrder.tools.ToString();
            medsO.text = sourceOrder.meds.ToString();
        }
    }
}
