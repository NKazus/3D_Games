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

    private bool isEmpty;

    private System.Action<Slot> PickCallback;
    private System.Action<Unit, bool, Vector3> BindCallback;

    private void ClickSlot(PointerEventData data)
    {
        PickCallback(this);
    }

    public void Init()
    {
        slotTransform = transform;
        statusMat = slotTransform.GetChild(0).GetComponent<MaterialInstance>();
        conditionMat = slotTransform.GetChild(1).GetComponent<MaterialInstance>();
        statusMat.Init();
        conditionMat.Init();
        trigger = GetComponent<EventTrigger>();
    }

    public void SetUnit(Unit targetUnit = null)
    {
        if (targetUnit == null)
        {
            isEmpty = true;
            BindCallback(currentTarget, false, Vector3.zero);
            currentTarget = null;
            return;
        }
        isEmpty = false;
        currentTarget = targetUnit;
        BindCallback(currentTarget, true, slotTransform.position);
    }

    public void ResetSlot()
    {
        currentTarget = null;
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
        //Debug.Log($"Name: {this.gameObject.name} is empty: {isEmpty}");
        if (isEmpty)
        {
            return UnitType.None;
        }
        return currentTarget.GetUnitType();
    }

    public void SetPickCallback(System.Action<Slot> callback)
    {
        PickCallback = callback;
    }

    public void SetBindCallback(System.Action<Unit, bool, Vector3> callback)
    {
        BindCallback = callback;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public SlotCondition CalculateCondition()
    {
        if (isEmpty)
        {
            conditionMat.SetColor(Color.red);
            return SlotCondition.Bad;
        }
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
            if (GetTargetUnitType() == neighbours[i].GetTargetUnitType())
            {
                repeats++;
            }
            else
            {
                unique++;
            }
        }

        float defaultMult = 1f / neighbours.Length;

        float result = defaultMult * (empty * 0.1f + repeats * 0.3f + unique);
        if(result > 0.85f)
        {            
            conditionMat.SetColor(Color.green);
            return SlotCondition.Good;
        }
        if(result > 0.45f)
        {
            conditionMat.SetColor(Color.yellow);
            return SlotCondition.Normal;
        }
        else
        {
            conditionMat.SetColor(Color.red);
            return SlotCondition.Bad;        
        }
    }
}
