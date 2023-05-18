using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static Dictionary<string, LinkedList<GameObject>> poolDictionary = new Dictionary<string, LinkedList<GameObject>>();
    private static List<GameObject> currentActiveGameObjects = new List<GameObject>();

    public static GameObject GetGameObjectFromPool(GameObject prefab)
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
            currentActiveGameObjects.Add(result);
            return result;
        }

        result = GameObject.Instantiate(prefab);
        result.name = prefab.name;
        currentActiveGameObjects.Add(result);
        return result;
    }

    public static void PutGameObjectToPool(GameObject target)
    {
        poolDictionary[target.name].AddFirst(target);
        currentActiveGameObjects.Remove(target);
        target.SetActive(false);
    }

}
