using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct CellIndices
{
    public int cellX;
    public int cellZ;

    public override bool Equals(object obj)
    {
        return cellX == ((CellIndices)obj).cellX && cellZ == ((CellIndices)obj).cellZ;
    }
}
public class Cell : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material blockedMaterial;

    private CellIndices indices;    

    private Transform cellTransform;
    private bool isBlocked = false;

    private delegate void CellClick(PointerEventData data);
    private CellClick cellClick;

    private EventTrigger trigger;
    private MeshRenderer meshRenderer;

    private DataHandler dataHandler;

    private void Awake()
    {
        cellTransform = transform;
        trigger = GetComponent<EventTrigger>();
        meshRenderer = GetComponent<MeshRenderer>();
        ResetCell();
    }

    private void OnDisable()
    {
        Deactivate();
    }

    public void SetIndices(int x, int z, DataHandler data)
    {
        indices.cellX = x;
        indices.cellZ = z;

        dataHandler = data;
    }

    private void EnableCell(PointerEventData data)
    {
        GlobalEventManager.MovePlayer(indices);
    }

    private void UnlockCell(PointerEventData data)
    {
        if(dataHandler.Unlocks > 0)
        {
            trigger.triggers.RemoveRange(0, trigger.triggers.Count);
            isBlocked = false;

            dataHandler.RemoveBonus(false);
            GlobalEventManager.UpdateLocks();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { EnableCell((PointerEventData)data); });
            trigger.triggers.Add(entry);
            MarkCell();
        }        
    }

    public Vector3 GetCellPosition()
    {
        return cellTransform.position;
    }

    public void Block()
    {
        isBlocked = true;
        meshRenderer.material = blockedMaterial;
    }

    public void Activate()
    {
        if (!isBlocked)
        {
            cellClick = EnableCell;
            MarkCell();
        }
        else
        {
            cellClick = UnlockCell;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { cellClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        if (!isBlocked)
        {
            meshRenderer.material = defaultMaterial;
        }
    }

    public void ResetCell()
    {
        isBlocked = false;
        meshRenderer.material = defaultMaterial;
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    public void MarkCell()
    {
        meshRenderer.material = activeMaterial;
    }
}
