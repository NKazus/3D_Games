using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Cell : MonoBehaviour
{
    private Transform cellTransform;
    private MaterialInstance highlight;
    private EventTrigger trigger;

    private int cellId;
    private int routeId;

    private bool isGem;
    private Gem currentGem;

    [Inject] private readonly GlobalEventManager events;

    private void Awake()
    {
        cellTransform = transform;
        highlight = cellTransform.GetChild(0).GetComponent<MaterialInstance>();
        highlight.InitMaterial();
        trigger = GetComponent<EventTrigger>();
    }

    private void OnEnable()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { PickCell((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    private void OnDisable()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    private void PickCell(PointerEventData data)
    {
        events.ChooseCell(cellId, routeId);
    }

    public void InitCell()
    {
        cellTransform = transform;
        highlight = cellTransform.GetChild(0).GetComponent<MaterialInstance>();
        highlight.InitMaterial();
        trigger = GetComponent<EventTrigger>();
    }

    public void ActivateCell(bool active)
    {
        trigger.enabled = active;
    }

    public Vector3 GetCellPosition()
    {
        return cellTransform.position;
    }

    public void SetId(int id, int rId)
    {
        cellId = id;
        routeId = rId;
    }

    public void ResetCell()
    {
        isGem = false;
        currentGem = null;
        DoHighlight(false, false);        
    }

    public void SetGem(Gem gem)
    {
        isGem = true;
        currentGem = gem;
        currentGem.PlaceGem(cellTransform.position);
    }

    public void DoHighlight(bool activate, bool showGem)
    {
        highlight.Show(activate, showGem);
        if (isGem)
        {
            currentGem.ShowGem(showGem);
        }
    }
}
