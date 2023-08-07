using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ReagentHandler : MonoBehaviour
{
    [SerializeField] private int reagentID;
    [SerializeField] private ParticleSystem smoke;

    private EventTrigger trigger;
    private Transform localTransform;
    private float scaleZ;

    [Inject] private readonly GlobalEventManager eventManager;
    [Inject] private readonly DataHandler dataHandler;

    private void Awake()
    {
        trigger = GetComponent<EventTrigger>();
        localTransform = transform;
        scaleZ = localTransform.localScale.z;
    }

    private void OnEnable()
    {
        eventManager.GameStateEvent += Activate;
    }

    private void OnDisable()
    {
        eventManager.GameStateEvent -= Activate;
        DOTween.KillAll(this);
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { UseReagent((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        else
        {
            trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }
    }

    private void UseReagent(PointerEventData data)
    {
        if (dataHandler.CheckReagent(reagentID))
        {
            eventManager.UseReagent(dataHandler.GetReagent(reagentID));
            smoke.Play();
            DOTween.Sequence()
                .SetId(this)
                .Append(localTransform.DOJump(localTransform.position, 0.5f, 1, 0.3f))
                .Join(localTransform.DOScaleZ(scaleZ * 0.5f, 0.15f).OnComplete(() => { }))
                .Append(localTransform.DOScaleZ(scaleZ, 0.15f));
        }
        else
        {
            eventManager.PlayVibro();
        }        
    }
}
