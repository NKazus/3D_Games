using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UnitCategory
{
    Player,
    Bot
}

public enum UnitType
{
    Attack,
    Defence,
    Buff
}

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected UnitCategory category;
    [SerializeField] private UnitType type;
    [SerializeField] private int damageValue;
    [SerializeField] private int hpValue;
    [SerializeField] private UnitVisuals visuals;

    private Transform unitTransform;
    private EventTrigger trigger;

    protected int hp;
    protected int damage;
    protected int actions;

    private Vector3 offScreenPos;
    private bool isEnabled;

    private FieldCell linkedCell;
    private System.Action<Unit> PickCallback;
    private System.Action<Unit> DestroyCallback;

    protected void CheckActions()
    {
        if(actions <= 0)
        {
            SwitchUnit(false);
        }
    }

    public void Init()
    {
        unitTransform = transform;
        trigger = GetComponent<EventTrigger>();
        visuals.Init();
    }

    public void ResetUnit()
    {
        //scale
        isEnabled = true;

        hp = hpValue;
        damage = damageValue;
    }

    public void RefreshUnit(int count)
    {
        actions = count;
        damage = Mathf.Clamp(damage, 0, damageValue);
        hp = Mathf.Clamp(hp, 0, hpValue);
        visuals.SetMaterial(true);
    }

    public void UpdateActions()
    {
        actions++;
    }

    public void UpdateHp()
    {
        hp++;
    }

    public void UpdateDamage()
    {
        damage++;
    }

    public void Activate()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { PickUnit((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    public void SwitchUnit(bool active)
    {
        trigger.enabled = active;
        visuals.SetMaterial(active);
    }

    public void PickUnit(PointerEventData data)
    {
        if(PickCallback != null)
        {
            PickCallback(this);
        }
    }

    public void DamageUnit(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            isEnabled = false;
            unitTransform.position = offScreenPos;
            if (DestroyCallback != null)
            {
                DestroyCallback(this);
            }
        }
    }

    public bool IsUnitEnabled()
    {
        return isEnabled;
    }

    public UnitCategory GetUnitCategory()
    {
        return category;
    }

    public UnitType GetUnitType()
    {
        return type;
    }

    public FieldCell GetUnitCell()
    {
        return linkedCell;
    }

    public void SetPickCallback(System.Action<Unit> callback)
    {
        PickCallback = callback;
    }

    public void SetDestroyCallback(System.Action<Unit> callback, Vector3 hiddenPos)
    {
        DestroyCallback = callback;
        offScreenPos = new Vector3(hiddenPos.x, unitTransform.position.y, hiddenPos.z);
    }

    public void PlaceUnit(FieldCell targetCell)
    {
        Vector3 targetPos = targetCell.GetCellPosition();
        unitTransform.position = new Vector3(targetPos.x, unitTransform.position.y, targetPos.z);
        linkedCell = targetCell;
    }

    public void Move(FieldCell targetCell)
    {
        PlaceUnit(targetCell);

        actions--;
        CheckActions();
    }

    public void Attack(Unit targetUnit)
    {
        targetUnit.DamageUnit(damage);

        actions--;
        CheckActions();
    }

    public abstract void Act(List<Unit> targets);
}
