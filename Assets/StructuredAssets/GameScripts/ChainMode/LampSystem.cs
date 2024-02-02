using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LampCondition
{
    Off = 0,
    Single = 1,
    Pair = 2,
    Chain = 3
}

public class LampSystem : MonoBehaviour
{
    [SerializeField] private Lamp[] lamps;

    public void Initialize(System.Action<Lamp> callback)
    {
        for(int i = 0; i < lamps.Length; i++)
        {
            lamps[i].Initialize(callback);
        }
    }

    public void ResetLamps()
    {
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].ResetLamp();
        }
    }
}
