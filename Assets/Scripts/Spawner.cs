using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector3[] startPos;
    [SerializeField] private Vector3[] stopPos;

    [SerializeField] private GameObject prefab;
    [SerializeField] private float interval;

    private int objectNumber;

    private bool isSpawning;

    private IEnumerator spawnCoroutine;

    [Inject] private readonly Pool pool;
    [Inject] private readonly Randomizer random;

    private IEnumerator Spawn()
    {
        GameObject target;
        while (isSpawning)
        {
            yield return new WaitForSeconds(interval);

            if (isSpawning)
            {
                target = pool.GetGameObjectFromPool(prefab);
                target.GetComponent<Meteor>().PushMeteor(startPos[random.GenerateInt(0, startPos.Length)], stopPos[random.GenerateInt(0, startPos.Length)]);
                objectNumber++;
            }
        }
    }

    public void StartSpawning()
    {
        pool.ResetCalls();
        objectNumber = 0;
        isSpawning = true;
        spawnCoroutine = Spawn();
        StartCoroutine(spawnCoroutine);
    }

    public void StopSpawning()
    {
        Debug.Log("sp:" + objectNumber);
        isSpawning = false;
        StopCoroutine(spawnCoroutine);
    }

    public bool CheckNumber()
    {
        return pool.CheckCalls();
    }
}
