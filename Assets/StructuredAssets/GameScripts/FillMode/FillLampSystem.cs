using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum FillCondition
{
    Empty,
    Player,
    Bot
}

public class FillLampSystem : MonoBehaviour
{
    [Header("!! Player start - 1st, bot start - last")]
    [SerializeField] private FillLamp[] lamps;
    [SerializeField] private int chainLampsNumber = 3;
    [SerializeField] private FillLamp[] chainLampsVariants;

    [Inject] private readonly ValueGenerator valueGenerator;

    public void SetChainLamps()
    {
        List<FillLamp> targetLamps = new List<FillLamp>();
        FillLamp target;
        for (int i = 0; i < chainLampsNumber; i++)
        {
            do
            {
                target = chainLampsVariants[valueGenerator.GenerateInt(0, chainLampsVariants.Length)];
            }
            while (targetLamps.Contains(target));
            target.SetChained();
            targetLamps.Add(target);
        }
    }

    public void Initialize(System.Action callback)
    {
        for (int i = 0; i < lamps.Length; i++)
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
        lamps[0].ChangeCondition(FillCondition.Player);
        lamps[lamps.Length - 1].ChangeCondition(FillCondition.Bot);
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

    public void PickRandom()
    {
        List<FillLamp> eventLamps = new List<FillLamp>();

        for (int i = 0; i < lamps.Length; i++)
        {
            if (lamps[i].GetCondition() == FillCondition.Bot && lamps[i].IsEmptyNeighbour())
            {
                eventLamps.Add(lamps[i]);
            }
        }
        if(eventLamps.Count > 0)
        {
            eventLamps[valueGenerator.GenerateInt(0, eventLamps.Count)].Ignite();
        }
    }

    public bool CheckComplete()
    {
        bool isPickAvailable = false;

        for (int i = 0; i < lamps.Length; i++)
        {
            if (lamps[i].GetCondition() == FillCondition.Player && lamps[i].IsEmptyNeighbour())
            {
                isPickAvailable = true;
            }
        }
        return !isPickAvailable;
    }

    public bool CalculateVictory()
    {
        int playerLamps = 0;
        int botLamps = 0;
        for (int i = 0; i < lamps.Length; i++)
        {
            if (lamps[i].GetCondition() == FillCondition.Player)
            {
                playerLamps++;
            }
            if (lamps[i].GetCondition() == FillCondition.Bot)
            {
                botLamps++;
            }
        }
        return playerLamps > botLamps;
    }
}
