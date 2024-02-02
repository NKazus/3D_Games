using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    House = 0,
    Shop = 1,
    Park = 2,
    Recreation = 3,
    None = 4
}

[System.Serializable]
public struct UnitPreset
{
    public UnitType type;
    public GameObject prefab;
}

public class UnitSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private Transform parentTransform;

    private UnitsPool pool = new UnitsPool();

    private List<Unit> activeUnits = new List<Unit>();

    private void PutToPool(Unit target)
    {
        pool.PutGameObjectToPool(target);
    }

    public Unit GenerateUnit(UnitType targetType)
    {
        for(int i = 0; i < unitPrefabs.Length; i++)
        {
            if(unitPrefabs[i].GetComponent<Unit>().GetUnitType() == targetType)
            {
                Unit newActive = pool.GetGameObjectFromPool(unitPrefabs[i], parentTransform).GetComponent<Unit>();
                activeUnits.Add(newActive);
                return newActive;
            }
        }
        return null;
    }

    public void ShowUnit(Unit target, Vector3 targetPosition, System.Action callback)
    {
        target.Show(targetPosition, callback);
    }

    public void HideUnit(Unit target, System.Action callback)
    {
        activeUnits.Remove(target);
        
        target.Hide(callback, PutToPool);
    }

    public void HideAllActive()
    {
        while(activeUnits.Count > 0)
        {
            PutToPool(activeUnits[0]);
            activeUnits.RemoveAt(0);
        }
    }
}
