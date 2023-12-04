using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

public class PlatformHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private TriggerTag[] tags;

    [Inject] private readonly GamePool gamePool;

    public void MoveRoute()
    {
        //invoke move event
    }

    public void StopRoute()
    {

    }

    //+init triggers with callbacks
    private void DoTrigger(System.Action<TriggerTag> target)
    {
        //triggers
    }

    private void SpawnNext()//when spawn trigger
    {
        //cycle indices and spawn platforms in order from pool
        //when spawned set pos and call init through RunnerPlatform script
    }

    //set to pool when despawn trigger
}
