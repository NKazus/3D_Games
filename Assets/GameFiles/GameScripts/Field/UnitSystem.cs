using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    House,
    Shop,
    Park,
    Recreation,
    None
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

    public void HideUnit(Unit target)
    {
        activeUnits.Remove(target);
        pool.PutGameObjectToPool(target);
    }

    public void HideAllActive()
    {
        while(activeUnits.Count > 0)
        {
            pool.PutGameObjectToPool(activeUnits[0]);
            activeUnits.RemoveAt(0);
        }
    }
}
