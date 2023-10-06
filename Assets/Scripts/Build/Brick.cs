using UnityEngine;
using UnityEngine.EventSystems;

public class Brick : MonoBehaviour
{
    [SerializeField] private int shardId;

    private Transform brickTransform;
    private EventTrigger trigger;

    private System.Action<int> PickCallback;

    private void Awake()
    {
        brickTransform = transform;
        trigger = GetComponent<EventTrigger>();
    }

    private void OnEnable()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { PickShard((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    private void OnDisable()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    private void PickShard(PointerEventData data)
    {
        PickCallback(shardId);
    }

    public void ActivateBrick(bool active)
    {
        trigger.enabled = active;        
    }

    public void SetCallback(System.Action<int> callback)
    {
        PickCallback = callback;
    }

    public Vector3 GetScale()
    {
        return brickTransform.localScale;
    }

    public void SetScale(Vector3 newScale, bool init)
    {
        brickTransform.localScale = newScale;
    }
}
