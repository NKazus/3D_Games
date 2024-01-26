using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SlotSystem : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private RateSystem rateSystem;

    private Slot activeSlot;

    [Inject] private readonly ValueGenerator valueGenerator;

    private void RecalculateCondition()
    {
        //Debug.Log("recalculate");
        List<SlotCondition> currentConditions = new List<SlotCondition>();

        for (int i = 0; i < slots.Length; i++)
        {
            currentConditions.Add(slots[i].CalculateCondition());
        }

        rateSystem.CalculateRate(currentConditions);
    }

    public bool CheckComplete()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                return false;
            }
        }
        rateSystem.CompleteRate();
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

    public void DeactivateCurrent()
    {
        SetActiveSlot(activeSlot);
    }

    public void BindUnit(Unit targetUnit)
    {
        activeSlot.SetUnit(targetUnit);
        RecalculateCondition();
    }

    public void InitSlots(System.Action<Slot> pickCallback, System.Action<Unit, bool, Vector3> bindCallback)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init();
            slots[i].SetPickCallback(pickCallback);
            slots[i].SetBindCallback(bindCallback);
        }

       rateSystem.CalculateMaxRate(slots.Length);
    }

    public void ResetSlots()
    {
        activeSlot = null;

        rateSystem.ResetRate();
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

    public RateType GetSlotsRate()
    {
        return rateSystem.GetRate();
    }

    public Slot GetRandomSlot()
    {
        return slots[valueGenerator.GenerateInt(0, slots.Length)];
    }
}   

