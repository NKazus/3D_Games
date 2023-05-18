using System.Collections.Generic;
using UnityEngine;

public class DotPositionRandomizer : MonoBehaviour
{
    [SerializeField] private Vector3[] dotsSpawnPoints;
    [SerializeField] private Transform[] dots;

    private List<int> dotIndices = new List<int>();

    private void Awake()
    {
        for(int i = 0; i < dotsSpawnPoints.Length; i++)
        {
            dotIndices.Add(i);
        }
    }

    private void UpdateDots(bool update)
    {
        if (update)
        {
            dotIndices.Shuffle();

            for (int i = 0; i< dots.Length; i++)
            {
                dots[i].localPosition = dotsSpawnPoints[dotIndices[i]];
            }
        }
    }

    public void InitializeDots()
    {
        GlobalEventManager.GameStateEvent += UpdateDots;
    }

    public void ResetDots()
    {
        GlobalEventManager.GameStateEvent -= UpdateDots;
    }
}
