using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Meteor : MonoBehaviour
{
    private EventTrigger trigger;

    private Transform meteorTransform;
    private string goName;

    private bool isGrabActive;

    [Inject] private readonly EventManager events;
    [Inject] private readonly Pool pool;

    private void Awake()
    {
        meteorTransform = transform;
        goName = gameObject.name;
        trigger = GetComponent<EventTrigger>();
    }

    private void OnEnable()
    {
        isGrabActive = false;
        events.GameStateEvent += ChangeState;
        events.CollapseEvent += GrabMeteor;
        events.MeteorTriggerEvent += UpdateTriggers;
        ActivateTriggers();
    }

    private void OnDisable()
    {
        events.GameStateEvent -= ChangeState;
        events.CollapseEvent -= GrabMeteor;
        events.MeteorTriggerEvent -= UpdateTriggers;
        RemoveTriggers();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destruct"))
        {
            Debug.Log("destruct");
            if (isGrabActive)
            {
                events.DoTriggerEvent(true);
            }
            events.CalculateMeteor(true);

            meteorTransform.DOKill();
            pool.PutGameObjectToPool(gameObject);
            
            //animation and hide
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Explode"))
        {
            Debug.Log("explode");
            events.CalculateMeteor(false);

            meteorTransform.DOKill();
            pool.PutGameObjectToPool(gameObject);
            
            //animation and hide
        }
    }

    private void ChangeState(bool active)
    {
        if (!active)
        {
            Debug.Log("hide");
            meteorTransform.DOKill();
            pool.PutGameObjectToPool(gameObject);
        }
    }

    private void UpdateTriggers(bool active)
    {
        if (active)
        {
            ActivateTriggers();
        }
        else
        {
            RemoveTriggers();
        }
    }

    private void ActivateTriggers()
    {
        EventTrigger.Entry click = new EventTrigger.Entry();
        click.eventID = EventTriggerType.PointerClick;
        click.callback.AddListener((data) => { MeteorClick((PointerEventData)data); });
        trigger.triggers.Add(click);
    }

    private void RemoveTriggers()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);     
    }

    private void MeteorClick(PointerEventData data)
    {
        isGrabActive = true;
        events.MeteorTriggerEvent -= UpdateTriggers;
        events.DoTriggerEvent(false);
        GrabMeteor();
    }

    private void GrabMeteor()
    {
        events.MeteorTriggerEvent -= UpdateTriggers;
        events.CollapseEvent -= GrabMeteor;
        RemoveTriggers();
        meteorTransform.DOLocalMove(Vector3.zero, 2f).SetId(goName);
    }

    public void PushMeteor(Vector3 start, Vector3 stop)
    {
        meteorTransform.localPosition = start;
        meteorTransform.DOLocalMove(stop, 10f);
    }


}
