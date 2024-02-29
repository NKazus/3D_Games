using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private Transform wallParent;

    private SpawnPool pool = new SpawnPool();
    private List<GameObject> spawnedWalls = new List<GameObject>();

    private void OnDisable()
    {
        ResetWalls();
    }

    public void SpawnWall(Vector3 spawnPosition)
    {
        GameObject spawnedWall = pool.GetGameObjectFromPool(wallPrefab);
        spawnedWalls.Add(spawnedWall);

        Transform wallTransform = spawnedWall.transform;
        wallTransform.parent = wallParent;
        wallTransform.position = new Vector3(spawnPosition.x, 0.13f, spawnPosition.z);
    }

    public void ResetWalls()
    {
        while(spawnedWalls.Count > 0)
        {
            pool.PutGameObjectToPool(spawnedWalls[0]);
            spawnedWalls.RemoveAt(0);
        }
    }
}
