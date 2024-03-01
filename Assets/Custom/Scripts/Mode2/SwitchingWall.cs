using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchingWall : MonoBehaviour
{
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activeMaterial;

    private MeshRenderer wallRenderer;
    private BoxCollider boxCollider;
    private EventTrigger trigger;

    private bool isActive;

    private void SwitchWall(PointerEventData data)
    {
        isActive = !isActive;
        wallRenderer.material = isActive ? activeMaterial : inactiveMaterial;
        boxCollider.isTrigger = !isActive;
    }

    public void Init()
    {
        wallRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        trigger = GetComponent<EventTrigger>();

        isActive = true;
        wallRenderer.material = activeMaterial;
    }

    public bool IsWallActive()
    {
        return isActive;
    }

    public void ResetWall()
    {
        isActive = false;
        wallRenderer.material = inactiveMaterial;
        boxCollider.isTrigger = true;
    }

    public void Activate()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { SwitchWall((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    public void SwitchTrigger(bool active)
    {
        trigger.enabled = active;
    }

}
