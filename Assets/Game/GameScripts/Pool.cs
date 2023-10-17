using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pool
{
    private Dictionary<string, LinkedList<GameObject>> poolDictionary = new Dictionary<string, LinkedList<GameObject>>();

    [Inject] private readonly DiContainer diContainer;

    public GameObject GetGameObjectFromPool(GameObject prefab, int cellId, Vector3 spawnPosition)
    {
        if (!poolDictionary.ContainsKey(prefab.name))
        {
            poolDictionary[prefab.name] = new LinkedList<GameObject>();
        }
        GameObject result;
        Buff targetComponent;
        if (poolDictionary[prefab.name].Count > 0)
        {
            result = poolDictionary[prefab.name].First.Value;
            poolDictionary[prefab.name].RemoveFirst();
            result.SetActive(true);
            targetComponent = result.GetComponent<Buff>();
            targetComponent.SetBuff(cellId, spawnPosition);
            return result;
        }

        result = diContainer.InstantiatePrefab(prefab);
        result.name = prefab.name;
        targetComponent = result.GetComponent<Buff>();
        targetComponent.SetBuff(cellId, spawnPosition);
        return result;
    }

    public void PutGameObjectToPool(GameObject target)
    {
        poolDictionary[target.name].AddFirst(target);
        target.SetActive(false);
    }
}
