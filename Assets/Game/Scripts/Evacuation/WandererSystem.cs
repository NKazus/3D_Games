using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct CustomPath
{
    public Transform[] wayPoints;
    public float movingTime;
}

public class WandererSystem : MonoBehaviour
{
    [SerializeField] private Wanderer[] wanderTargets;
    [SerializeField] private Wanderer[] obstacleTargets;
    [SerializeField] private CustomPath[] obstaclePaths;
    [SerializeField] private CustomPath[] wanderPaths;
    //safe zones

    private List<int> currentPathIndexes = new List<int>();
    private System.Random random = new System.Random();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int t = 0; t < wanderPaths.Length; t++)
        {
            for (int i = 0; i < wanderPaths[t].wayPoints.Length; i++)
            {
                Gizmos.DrawSphere(wanderPaths[t].wayPoints[i].position, 0.01f);
            }
        }
        Gizmos.color = Color.yellow;
        for (int t = 0; t < obstaclePaths.Length; t++)
        {
            for (int i = 0; i < obstaclePaths[t].wayPoints.Length; i++)
            {
                Gizmos.DrawSphere(obstaclePaths[t].wayPoints[i].position, 0.01f);
            }
        }
    }


    public void SetWanderersPath()
    {
        int index;

        //wanderers
        currentPathIndexes.Clear();        
        for (int i = 0; i < wanderTargets.Length; i++)
        {
            do
            {
                index = random.Next(0, wanderPaths.Length);
            }
            while (currentPathIndexes.Contains(index));
            wanderTargets[i].SetPath(wanderPaths[index]);
            currentPathIndexes.Add(index);
        }

        //obstacles
        currentPathIndexes.Clear();
        for (int i = 0; i < obstacleTargets.Length; i++)
        {
            do
            {
                index = random.Next(0, obstaclePaths.Length);
            }
            while (currentPathIndexes.Contains(index));
            obstacleTargets[i].SetPath(obstaclePaths[index]);
            currentPathIndexes.Add(index);
        }
    }

    public void MoveWanderers()
    {
        for (int i = 0; i < wanderTargets.Length; i++)
        {
            wanderTargets[i].MovePath();
        }
        for (int i = 0; i < obstacleTargets.Length; i++)
        {
            obstacleTargets[i].MovePath();
        }
    }

    public void StopWanderers()
    {
        DOTween.Kill("wanderer");
    }
}
