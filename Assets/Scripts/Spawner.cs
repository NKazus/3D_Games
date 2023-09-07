using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private Vector3[] startPos;
    [SerializeField] private Vector3[] stopPos;

    [SerializeField] private GameObject prefab;
    [SerializeField] private float interval;

    private float timeScale;

    private int spawnedObjects;

    private IEnumerator spawnCoroutine;

    [Inject] private readonly Pool pool;
    [Inject] private readonly Randomizer random;

    private IEnumerator Spawn()
    {
        GameObject target;
        int i = 0;
        while(i < spawnedObjects)
        {
            yield return new WaitForSeconds(interval);
            
            target = pool.GetGameObjectFromPool(prefab);
            target.GetComponent<Meteor>().PushMeteor(startPos[random.GenerateInt(0, startPos.Length)], stopPos[random.GenerateInt(0, startPos.Length)], timeScale);
            i++;
        }
    }

    public void StartSpawning(int number, bool timeC)
    {
        spawnedObjects = number;

        timeScale = timeC ? 0.6f : 1f;
        spawnCoroutine = Spawn();
        StartCoroutine(spawnCoroutine);

        timer.Activate(number * interval);
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
        timer.Deactivate();
    }
}
