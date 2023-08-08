using System;
using UnityEngine;

public class MenuEnvironment : MonoBehaviour
{
    [SerializeField] private Weather weather;
    [SerializeField] private Plant plant;

    private System.Random rand = new System.Random();

    private void OnEnable()
    {
        Invoke("TriggerEnvironment", 1f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("TriggerEnvironment");
        }
    }

    private void TriggerEnvironment()
    {
        int val = rand.Next(0, 3);
        switch (val)
        {
            case 0: weather.DoNormal(); break;
            case 1: weather.DoRain(); break;
            case 2: weather.DoHeat(); break;
            default: throw new NotSupportedException();
        }
        plant.Grow(plant.GetGrowIterations() - 1);
        plant.DoFlower(true);
    }
}
