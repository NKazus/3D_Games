using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CustomPath
{
    public Transform[] wayPoints;
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

    }

    public void StopWanderers()
    {
        //dotween kill
    }
}
