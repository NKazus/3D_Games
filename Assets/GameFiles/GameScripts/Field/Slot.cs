using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotCondition
{
    Bad,
    Normal,
    Good
}
public class Slot : MonoBehaviour
{
    [SerializeField] private Slot[] neighbours;

    private MaterialInstance statusMat;
    private MaterialInstance conditionMat;
    private Transform slotTransform;
    private EventTrigger trigger;

    private Unit currentTarget;
    private SlotCondition currentCondition;

    private bool isEmpty;

    private System.Action<Slot> SlotCallback;
    private System.Action<Unit, bool, Vector3> UnitCallback;

    private void Awake()
    {
        slotTransform = transform;
        statusMat = slotTransform.GetChild(0).GetComponent<MaterialInstance>();
        conditionMat = slotTransform.GetChild(1).GetComponent<MaterialInstance>();
        trigger = GetComponent<EventTrigger>();
    }

    private void ClickSlot(PointerEventData data)
    {
        SlotCallback(this);
    }

    public void SetUnit(Unit targetUnit = null)
    {
        if(targetUnit == null)
        {
            isEmpty = true;
            UnitCallback(currentTarget, false, Vector3.zero);
            currentTarget = null;
            return;
        }
        currentTarget = targetUnit;
        UnitCallback(currentTarget, true, slotTransform.position);
    }

    public void ResetSlot()
    {
        currentTarget = null;
        currentCondition = SlotCondition.Bad;
        conditionMat.SetColor(Color.red);
        isEmpty = true;
        statusMat.SetColor(Color.gray);
    }

    public void Activate()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { ClickSlot((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    public void SwitchSlot(bool active)
    {
        trigger.enabled = active;
    }

    public void PickSlot(bool pick)
    {
        if (pick)
        {
            statusMat.SetColor(Color.white);
        }
        else
        {
            statusMat.SetColor(Color.grey);
        }
    }

    public UnitType GetTargetUnitType()
    {
        if (isEmpty)
        {
            return UnitType.None;
        }
        return currentTarget.GetUnitType();
    }

    public void SetSlotCallback(System.Action<Slot> callback)
    {
        SlotCallback = callback;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public SlotCondition CalculateCondition()
    {
        int empty = 0;
        int repeats = 0;
        int unique = 0;

        for(int i = 0; i < neighbours.Length; i++)
        {
            if(neighbours[i].GetTargetUnitType() == UnitType.None)
            {
                empty++;
                continue;
            }
            if(currentTarget.GetUnitType() == neighbours[i].GetTargetUnitType())
            {
                unique++;
            }
            else
            {
                repeats++;
            }
        }

        float defaultMult = 1f / neighbours.Length;

        float result = defaultMult * (empty * 0.2f + repeats * 0.4f + unique);
        if(result > 0.65f)
        {
            currentCondition = SlotCondition.Good;
            conditionMat.SetColor(Color.green);
        }
        if(result > 0.35f)
        {
            currentCondition = SlotCondition.Normal;
            conditionMat.SetColor(Color.yellow);
        }
        else
        {
            currentCondition = SlotCondition.Bad;
            conditionMat.SetColor(Color.red);
        }
        return currentCondition;
    }
}
