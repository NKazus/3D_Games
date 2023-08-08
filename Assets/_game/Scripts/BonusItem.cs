using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class BonusItem : MonoBehaviour
{
    [SerializeField] protected int itemID;
    [SerializeField] protected int itemPrice;
    [SerializeField] protected Text priceUI;
   

    protected EventTrigger trigger;
    protected Transform localTransform;
    protected float scale;

    [Inject] protected readonly GlobalEventManager eventManager;
    [Inject] protected readonly DataHandler dataHandler;

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
        if(dataHandler.Seeds >= itemPrice)
        {
            //dataHandler.TradePotions(itemPrice, itemID);
            DOTween.Sequence()
                .SetId(this)
                .Append(localTransform.DOScaleZ(scale * 0.8f, 0.2f))
                .Append(localTransform.DOScaleZ(scale, 0.2f));
            eventManager.PlayCoins();
        }
        else
        {
            eventManager.PlayVibro();
        }
    }
}
