using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BonusItem : MonoBehaviour
{
    [SerializeField] protected int itemID;
    [SerializeField] protected int itemPrice;
    [SerializeField] protected Text priceUI;
    [SerializeField] protected DataHandler dataHandler;

    protected EventTrigger trigger;
    protected Transform localTransform;
    protected float scale;

    protected virtual void Awake()
    {
        trigger = GetComponent<EventTrigger>();
        localTransform = transform;
        scale = localTransform.localScale.z;

        priceUI.text = itemPrice.ToString();
    }

    protected virtual void OnEnable()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { PickItem((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    protected void OnDisable()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    protected virtual void PickItem(PointerEventData data)
    {
        
    }
}
