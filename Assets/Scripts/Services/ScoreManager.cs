using System;
using System.Collections.Generic;
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

    public void UpdateResources(GameResources id, int value)
    {
        Text target;
        switch (id)
        {
            case GameResources.Food: target = foodUI; break;
            case GameResources.Fuel: target = fuelUI; break;
            case GameResources.Tools: target = toolsUI; break;
            case GameResources.Meds: target = medsUI; break;
            default: throw new NotSupportedException();
        }
        UpdateText(target, value.ToString());
    }

}
