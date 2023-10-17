using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float interval;

    private Route route;
    private int objectNumber;

    private bool isSpawning;

    private IEnumerator spawnCoroutine;

    [Inject] private readonly Pool pool;
    [Inject] private readonly RandomValueProvider random;

    private IEnumerator Spawn()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(interval);

            if (isSpawning)
            {
                int newId = route.GenerateId();
                Vector3 pos = route.GetById(newId);
                pool.GetGameObjectFromPool(prefabs[random.GetInt(0, objectNumber)], newId, pos);     
            }
        }        
    }

    public void Initialize(Route current)
    {
        route = current;
        objectNumber = prefabs.Length;
    }

    public void StartSpawning()
    {
        isSpawning = true;
        spawnCoroutine = Spawn();
        StartCoroutine(spawnCoroutine);        
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if(spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }
}
