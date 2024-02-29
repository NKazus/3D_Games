using System.Collections.Generic;
using UnityEngine;

public class SpawnPool
{
    private Dictionary<string, LinkedList<GameObject>> poolDictionary = new Dictionary<string, LinkedList<GameObject>>();

    public GameObject GetGameObjectFromPool(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab.name))
        {
            poolDictionary[prefab.name] = new LinkedList<GameObject>();
        }
        GameObject result;
        if (poolDictionary[prefab.name].Count > 0)
        {
            result = poolDictionary[prefab.name].First.Value;
            poolDictionary[prefab.name].RemoveFirst();
            result.SetActive(true);
            return result;
        }

        result = Object.Instantiate(prefab);
        result.name = prefab.name;
        return result;
    }

    public void PutGameObjectToPool(GameObject target)
    {
        poolDictionary[target.name].AddFirst(target);
        target.SetActive(false);
    }

}
