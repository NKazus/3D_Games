using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Meteor : MonoBehaviour
{
    [SerializeField] private Trail trail;

    private EventTrigger trigger;
    private SphereCollider meteorCollider;

    private Transform meteorTransform;
    private string goName;

    private bool isGrabActive;

    private float tScale;

    [Inject] private readonly EventManager events;
    [Inject] private readonly Pool pool;

    private void Awake()
    {
        meteorTransform = transform;
        goName = gameObject.name;
        trigger = GetComponent<EventTrigger>();
        meteorCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        Debug.Log("meteor_enabled");
        meteorTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        meteorTransform.localRotation = Random.rotation;
        isGrabActive = false;
        events.GameStateEvent += ChangeState;
        events.CollapseEvent += GrabMeteor;
        events.MeteorTriggerEvent += UpdateTriggers;
        ActivateTriggers();
    }

    private void OnDisable()
    {
        Debug.Log("meteor_disabled");
        meteorTransform.DOKill();
        RemoveTriggers();
        events.GameStateEvent -= ChangeState;
        events.CollapseEvent -= GrabMeteor;
        events.MeteorTriggerEvent -= UpdateTriggers;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destruct"))
        {
            meteorCollider.enabled = false;

            trail.DoExplosion();
            events.GameStateEvent -= ChangeState;
            if (isGrabActive)
            {
                events.DoTriggerEvent(true);
            }
            events.CalculateMeteor(true);

            DOTween.Sequence()
                .Append(meteorTransform.DOScale(0f, 0.3f))
                .AppendInterval(1f)
                .OnComplete(()=> pool.PutGameObjectToPool(gameObject));
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Explode"))
        {
            meteorCollider.enabled = false;

            events.GameStateEvent -= ChangeState;
            events.CalculateMeteor(false);

            pool.PutGameObjectToPool(gameObject);
        }
    }

    private void ChangeState(bool active)
    {
        if (!active)
        {
            meteorCollider.enabled = false;
            meteorTransform.DOKill();
            pool.PutGameObjectToPool(gameObject);
        }
    }

    private void UpdateTriggers(bool active)
    {
        trigger.enabled = active;
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
        trail.DoTrail(false);
        events.MeteorTriggerEvent -= UpdateTriggers;
        events.CollapseEvent -= GrabMeteor;
        RemoveTriggers();
        meteorTransform.DOLocalMove(Vector3.zero, 5f * tScale).SetId(goName);
    }

    public void PushMeteor(Vector3 start, Vector3 stop, float tweenScale)
    {
        meteorTransform.localPosition = start;
        meteorCollider.enabled = true;

        trail.DoTrail(true);
        meteorTransform.DOLocalMove(stop, 20f);
        tScale = tweenScale;
    }


}
