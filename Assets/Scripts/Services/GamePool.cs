using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FitTheSize.GameServices
{
    public class GamePool
    {
        private Dictionary<string, LinkedList<GameObject>> poolDictionary = new Dictionary<string, LinkedList<GameObject>>();

        [Inject] private readonly DiContainer diContainer;

        public GameObject GetGameObjectFromPool(GameObject prefab, out bool isInstantiated)
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
                isInstantiated = false;
                return result;
            }

            result = diContainer.InstantiatePrefab(prefab);
            result.name = prefab.name;
            isInstantiated = true;
            return result;
        }

        public void PutGameObjectToPool(GameObject target)
        {
            poolDictionary[target.name].AddFirst(target);
            target.SetActive(false);
        }
    }
}
