using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pool
{
    private Dictionary<string, LinkedList<GameObject>> poolDictionary = new Dictionary<string, LinkedList<GameObject>>();

    private int calls;
    private int puts;

    [Inject] private readonly DiContainer diContainer;

    public void ResetCalls()
    {
        calls = 0;
        puts = 0;
    }

    public bool CheckCalls()
    {
        Debug.Log("c:"+calls + " p:"+puts) ;
        return calls <= puts;
    }

    public GameObject GetGameObjectFromPool(GameObject prefab)
    {
        calls++;
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

        result = diContainer.InstantiatePrefab(prefab);
        result.name = prefab.name;
        return result;
    }

    public void PutGameObjectToPool(GameObject target)
    {
        puts++;
        poolDictionary[target.name].AddFirst(target);
        target.SetActive(false);
    }
}
