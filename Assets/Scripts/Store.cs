using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using FitTheSize.GameServices;

public class Store : MonoBehaviour
{
    [SerializeField] private Button foodB;
    [SerializeField] private Button fuelB;
    [SerializeField] private Button toolsB;
    [SerializeField] private Button medsB;

    [SerializeField] private int foodPrice;
    [SerializeField] private int fuelPrice;
    [SerializeField] private int toolsPrice;
    [SerializeField] private int medsPrice;

    [SerializeField] private Text foodText;
    [SerializeField] private Text fuelText;
    [SerializeField] private Text toolsText;
    [SerializeField] private Text medsText;

    private int foodP;
    private int fuelP;
    private int toolsP;
    private int medsP;

    [Inject] private readonly GameData data;
    [Inject] private readonly GameEventHandler events;

    private void OnEnable()
    {
        data.UpdateAllRes(false);
        data.UpdateRep(0);
        data.UpdateMoney(0);

        SetPrices();
        CheckBuffs();
        foodB.onClick.AddListener(() => BuyRes(GameResources.Food));
        fuelB.onClick.AddListener(() => BuyRes(GameResources.Fuel));
        toolsB.onClick.AddListener(() => BuyRes(GameResources.Tools));
        medsB.onClick.AddListener(() => BuyRes(GameResources.Meds));
    }

    private void OnDisable()
    {
        foodB.onClick.RemoveAllListeners();
        fuelB.onClick.RemoveAllListeners();
        toolsB.onClick.RemoveAllListeners();
        medsB.onClick.RemoveAllListeners();
    }

    private void SetPrices()
    {
        int rep = data.Reputation;
        float divider = rep > 0 ? 200f : 100f;
        float modifyer = 1f - (rep / divider);

        foodP = (int) (foodPrice * modifyer);
        fuelP = (int)(fuelPrice * modifyer);
        toolsP = (int)(toolsPrice * modifyer);
        medsP = (int)(medsPrice * modifyer);

        foodText.text = foodP.ToString();
        fuelText.text = fuelP.ToString();
        toolsText.text = toolsP.ToString();
        medsText.text = medsP.ToString();
    }

    private void CheckBuffs()
    {
        foodB.interactable = data.Money >= foodP;
        fuelB.interactable = data.Money >= fuelP;
        toolsB.interactable = data.Money >= toolsP;
        medsB.interactable = data.Money >= medsP;
    }

    private void BuyRes(GameResources resId)
    {
        data.UpdateRes(resId, 1, false);
        int price = resId switch
        {
            GameResources.Food => foodP,
            GameResources.Fuel => fuelP,
            GameResources.Tools => toolsP,
            GameResources.Meds => medsP,
            _ => throw new NotSupportedException()
        };
        events.PlaySound(AudioEffect.Resource);
        data.UpdateMoney(-price);
        CheckBuffs();
    }
}
