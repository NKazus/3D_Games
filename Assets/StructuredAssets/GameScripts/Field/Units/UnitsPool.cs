using System.Collections.Generic;
using UnityEngine;
//using Zenject;

public class UnitsPool
{
    private Dictionary<string, LinkedList<GameObject>> poolDictionary = new Dictionary<string, LinkedList<GameObject>>();

    //[Inject] private readonly DiContainer diContainer;

    public GameObject GetGameObjectFromPool(GameObject prefab, Transform parentTransform)
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

        //result = diContainer.InstantiatePrefab(prefab);
        result = Object.Instantiate(prefab);
        result.name = prefab.name;
        result.transform.parent = parentTransform;
        return result;
    }

    public void PutGameObjectToPool(Unit target)
    {
        GameObject targetGO = target.gameObject;
        poolDictionary[targetGO.name].AddFirst(targetGO);
        targetGO.SetActive(false);
    }

}
