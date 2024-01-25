using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct RateValue
{
    public SlotCondition condition;
    public float rate;
}

public class RateSystem : MonoBehaviour
{
    [SerializeField] private RateValue[] values;
    [SerializeField] private ScaleUI progress;

    private float maxRate;
    private float currentRate;

    public void CalculateMaxRate(int slotsNumber)
    {
        float bestRate = values[0].rate;

        for(int i = 0; i < values.Length; i++)
        {
            if(values[i].rate > bestRate)
            {
                bestRate = values[i].rate;
            }
        }
        maxRate = slotsNumber * bestRate;
    }

    public void CalculateRate(List<SlotCondition> targetConditions)
    {
        currentRate = 0f;
        for(int i = 0; i < targetConditions.Count; i++)
        {
            for(int j = 0; j < values.Length; j++)
            {
                if(targetConditions[i] == values[j].condition)
                {
                    currentRate += values[j].rate;
                }
            }
        }
        progress.UpdateValue(currentRate / maxRate);
    }

    public void ResetRate()
    {
        currentRate = 0f;
        progress.UpdateValue(0f);
    }
}
