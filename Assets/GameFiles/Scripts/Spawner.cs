using System.Collections;
using UnityEngine;
using Zenject;

[System.Serializable]
public struct MovementPoints
{
    public Vector3 startPos;
    public Vector3 stopPos;
}

public class Spawner : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private MovementPoints[] movePoints;

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
            int movePointsId = random.GenerateInt(0, movePoints.Length);
            target.GetComponent<Meteor>().PushMeteor(movePoints[movePointsId].startPos, movePoints[movePointsId].stopPos, timeScale);
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
