using System.Collections.Generic;
using UnityEngine;

public class SlotSystem : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private RateSystem rateSystem;

    private Slot activeSlot;

    private bool CheckComplete()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public bool SetActiveSlot(Slot target)
    {
        if(activeSlot != null)
        {
            activeSlot.PickSlot(false);
        }

        if (activeSlot == target)
        {
            activeSlot = null;
            return false; //diactivated previous
        }

        activeSlot = target;
        activeSlot.PickSlot(true);
        return true;
    }

    public void BindUnit(Unit targetUnit)
    {
        activeSlot.SetUnit(targetUnit);
        if (CheckComplete())
        {
            //endgame event
        }
    }

    public void InitSlots(System.Action<Slot> callback)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlotCallback(callback);
        }

        //rateSystem.CalculateMaxRate(slots.Length);
    }

    public void ResetSlots()
    {
        activeSlot = null;

        //rateSystem.ResetRate();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ResetSlot();
        }
    }

    public void ActivateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Activate();
        }
    }

    public void DeactivateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Deactivate();
        }
    }

    public void SwitchSlots(bool active)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SwitchSlot(active);
        }
    }

    public void RecalculateCondition()
    {
        List<SlotCondition> currentConditions = new List<SlotCondition>();

        for (int i = 0; i < slots.Length; i++)
        {
            currentConditions.Add(slots[i].CalculateCondition());
        }

        rateSystem.CalculateRate(currentConditions);
    }
}   

