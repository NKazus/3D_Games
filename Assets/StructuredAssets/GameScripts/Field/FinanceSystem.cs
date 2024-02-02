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

    public void CalculateMoney(UnitType targetType)
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
        }

        progress.UpdateValue(currentMoney / maxMoney);
    }

    public void ResetMoney()
    {
        currentMoney = maxMoney;
        progress.UpdateValue(1f);
    }

    public bool CheckMoney()
    {
        return currentMoney > 0;
    }
}
