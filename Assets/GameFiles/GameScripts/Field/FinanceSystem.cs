using UnityEngine;

[System.Serializable]
public struct UnitCost
{
    public UnitType type;
    public float cost;
}

public class FinanceSystem : MonoBehaviour
{
    [SerializeField] private UnitCost[] costs;
    [SerializeField] private ScaleUI progress;
    [SerializeField] private float maxMoney;

    private float currentMoney;

    public void CalculateRate(UnitType targetType)
    {
        for (int j = 0; j < costs.Length; j++)
        {
            if (targetType == costs[j].type)
            {
                currentMoney -= costs[j].cost;
                break;
            }
        }
        
        if(currentMoney <= 0)
        {
            currentMoney = 0;
            //event;
        }

        progress.UpdateValue(currentMoney / maxMoney);
    }

    public void ResetRate()
    {
        currentMoney = maxMoney;
        progress.UpdateValue(1f);
    }
}
