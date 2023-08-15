using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Buff : MonoBehaviour
{
    [SerializeField] private string targetTag;
    [SerializeField] private int buffId;

    private EventTrigger trigger;

    private MaterialInstance buffMat;
    private Transform buffTransform;
    private int cellId;

    private bool isActive;
    private Vector3 buffPos;

    [Inject] private readonly GlobalEventManager events;
    [Inject] private readonly Pool pool;

    private void Awake()
    {
        buffMat = GetComponent<MaterialInstance>();
        buffTransform = transform;
        trigger = GetComponent<EventTrigger>();
    }

    private void OnEnable()
    {
        isActive = false;
        events.GameStateEvent += ChangeState;
        ActivateDragTriggers();
    }

    private void OnDisable()
    {
        events.GameStateEvent -= ChangeState;
        RemoveDragTriggers();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag(targetTag))
        {
            RemoveDragTriggers();

            events.RemoveBuff(cellId);
            MaterialInstance targetMat = target.GetComponent<MaterialInstance>();
            if (targetMat.ChangeColor())
            {
                pool.PutGameObjectToPool(gameObject);
                return;
            }

            Vector3 newPos = target.transform.position;
            buffTransform.position = new Vector3(newPos.x, buffTransform.position.y, newPos.z);
            buffMat.ChangeColor();
            isActive = true;
            
        }
        if (target.CompareTag("Player"))
        {
            events.CalculateBuff(buffId, isActive);
        }
    }

    private void ChangeState(bool active)
    {
        if (!active)
        {
            DOTween.Kill("buff");
            pool.PutGameObjectToPool(gameObject);
        }
    }

    private void ActivateDragTriggers()
    {
        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((data) => { Dragging((PointerEventData)data); });
        trigger.triggers.Add(drag);

        EventTrigger.Entry dragEnd = new EventTrigger.Entry();
        dragEnd.eventID = EventTriggerType.EndDrag;
        dragEnd.callback.AddListener((data) => { StopDragging((PointerEventData)data); });
        trigger.triggers.Add(dragEnd);
    }

    private void RemoveDragTriggers()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    private void Dragging(PointerEventData data)
    {
        Vector3 pos =
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(buffTransform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        buffTransform.position = new Vector3(worldPos.x, buffTransform.position.y, worldPos.z);
    }

    private void StopDragging(PointerEventData data)
    {
        if (!isActive)
        {
            buffTransform.DOLocalMove(buffPos, 0.5f).SetId("buff");
        }
    }

    public void SetBuff(int id, Vector3 pos)
    {
        cellId = id;
        buffTransform.position = new Vector3(pos.x, buffTransform.position.y, pos.z);
        buffPos = buffTransform.localPosition;
    }
}
