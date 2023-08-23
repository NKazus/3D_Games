using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text reputationUI;
    [SerializeField] private Text moneyUI;

    [SerializeField] private Text foodUI;
    [SerializeField] private Text fuelUI;
    [SerializeField] private Text toolsUI;
    [SerializeField] private Text medsUI;

    [SerializeField] private Text storeFoodUI;
    [SerializeField] private Text storeFuelUI;
    [SerializeField] private Text storeToolsUI;
    [SerializeField] private Text storeMedsUI;

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }

    public void UpdateGlobal(bool rep, int value)
    {
        Text target;
        target = rep ? reputationUI : moneyUI;

        UpdateText(target, value.ToString());
    }    

    public void UpdateResources(GameResources id, int value, bool isGameplay)
    {
        Text target;
        switch (id)
        {
            case GameResources.Food: target = isGameplay ? foodUI : storeFoodUI; break;
            case GameResources.Fuel: target = isGameplay ? fuelUI : storeFuelUI; break;
            case GameResources.Tools: target = isGameplay ? toolsUI : storeToolsUI; break;
            case GameResources.Meds: target = isGameplay ? medsUI : storeMedsUI; break;
            default: throw new NotSupportedException();
        }
        UpdateText(target, value.ToString());
    }

}
