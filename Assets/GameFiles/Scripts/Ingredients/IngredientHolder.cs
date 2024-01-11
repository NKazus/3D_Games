using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ActionType
{
    Unlock,
    Pick,
    Unpick
}

public class IngredientHolder : MonoBehaviour
{
    [SerializeField] private Image holderInfo;

    private Transform holderTransform;

    private Ingredient currentTarget;

    private EventTrigger trigger;

    private int holderId;

    private bool isPicked;
    private bool isLocked;

    private System.Action<ActionType, int, int> HolderCallback;

    private void Awake()
    {
        holderTransform = transform;

        trigger = GetComponent<EventTrigger>();
    }

    private void OnEnable()
    {
        AddTrigger();
        SwitchHolder(false);
    }

    private void OnDisable()
    {
        RemoveTrigger();
        currentTarget = null;
    }

    private void PickHolder(PointerEventData data)
    {
        if (isLocked)
        {
            HolderCallback(ActionType.Unlock, holderId, currentTarget.GetId());
            return;
        }

        isPicked = !isPicked;
        HolderCallback(isPicked ? ActionType.Pick : ActionType.Unpick, holderId, currentTarget.GetId());
    }

    private void AddTrigger()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { PickHolder((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    private void RemoveTrigger()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    private void SetNewTarget()
    {
        currentTarget.SetPosition(holderTransform.position);
        currentTarget.Rescale(true, FinishSetup);
    }

    private void FinishSetup()
    {
        SwitchHolder(true);
    }

    public void InitHolder(int id)
    {
        holderId = id;
    }

    public void SetHolderCallback(System.Action<ActionType, int, int> callback)
    {
        HolderCallback = callback;
    }

    public void SwitchHolder(bool active)
    {
        trigger.enabled = active;
    }

    public void SwitchHolderLock(bool locked)
    {
        isLocked = locked;
    }

    public void SetHolder(Ingredient target)
    {
        SwitchHolder(false);
        isPicked = false;

        if(currentTarget != null)
        {
            currentTarget.Rescale(false, SetNewTarget);
            currentTarget = target;
        }
        else
        {
            currentTarget = target;
            SetNewTarget();
        }
    }

    public void UpdateHolder(Sprite icon)
    {
        holderInfo.sprite = icon;
    }
}
