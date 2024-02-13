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
    [SerializeField] private UnitCategory category;
    [SerializeField] private UnitType type;
    [SerializeField] private int damageValue;
    [SerializeField] private int hpValue;
    [SerializeField] private UnitVisuals visuals;

    private Transform unitTransform;
    private EventTrigger trigger;

    private int hp;
    private int damage;
    private int actions;

    private FieldCell linkedCell;
    private System.Action<Unit> PickCallback;
    private System.Action<Unit> DestroyCallback;

    private void CheckActions()
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
        hp = hpValue;
        damage = damageValue;
    }

    public void ResetActions(int count)
    {
        actions = count;
        visuals.SetMaterial(true);
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
        Debug.Log("Unit pick");
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

            if (DestroyCallback != null)
            {
                DestroyCallback(this);
            }
        }
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

    public void SetDestroyCallback(System.Action<Unit> callback)
    {
        DestroyCallback = callback;
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

    public abstract void Act();
}
