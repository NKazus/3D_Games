using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum LampCondition
{
    Off = 0,
    Single = 1,
    Pair = 2,
    Chain = 3
}

public class LampSystem : MonoBehaviour
{
    [Header("!! Start - 1st, destination - last")]
    [SerializeField] private Lamp[] lamps;

    [Inject] private readonly ValueGenerator valueGenerator;

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
        lamps[0].Ignite(null);
    }

    public void ActivateLamps()
    {
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].Activate();
        }
    }

    public void DeactivateLamps()
    {
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].Deactivate();
        }
    }

    public void SwitchLamps(bool active)
    {
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].SwitchLamp(active);
        }
    }

    public void ExtinguishRandom()
    {
        Debug.Log("random lamp pick");
        List<Lamp> eventLamps = new List<Lamp>();

        for (int i = 0; i < lamps.Length - 1; i++)//exclude destination
        {
            if (lamps[i].GetCondition() == LampCondition.Single || lamps[i].GetCondition() == LampCondition.Off)
            {
                eventLamps.Add(lamps[i]);
            }
        }

        eventLamps[valueGenerator.GenerateInt(0, eventLamps.Count)].Extinguish(null);
    }

    public bool CheckComplete()
    {
        return lamps[lamps.Length - 1].GetCondition() != LampCondition.Off;
    }
}
