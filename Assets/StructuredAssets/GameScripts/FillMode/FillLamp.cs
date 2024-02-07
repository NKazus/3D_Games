using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class FillLamp : MonoBehaviour
{
    [SerializeField] private FillLamp[] neighbours;

    private bool isChain;
    private MeshRenderer highlight;
    private LampVisual visualComponent;

    private EventTrigger trigger;
    private FillCondition currentCondition;

    private bool isLinked;

    private System.Action LampCallback;

    [Inject] private readonly ValueGenerator valueGenerator;

    private void ClickLamp(PointerEventData data)
    {
        if (!isLinked)
        {
            return;
        }

        Ignite();

        if (LampCallback != null)
        {
            LampCallback();
        }
    }

    public void ChangeCondition(FillCondition target)
    {        
        if (target == currentCondition)
        {
            return; //already lit
        }

        if(currentCondition != FillCondition.Empty)
        {
            return; //bot
        }

        currentCondition = target;
        visualComponent.UpdateStatus(currentCondition);
        isLinked = (currentCondition == FillCondition.Player);
    }

    public void Initialize(System.Action callback)
    {
        visualComponent = transform.GetChild(0).GetComponent<LampVisual>();
        visualComponent.Init();

        highlight = transform.GetChild(1).GetComponent<MeshRenderer>();

        trigger = GetComponent<EventTrigger>();
        LampCallback = callback;
    }

    public void ResetLamp()
    {
        currentCondition = FillCondition.Empty;
        visualComponent.UpdateStatus(currentCondition);
        isLinked = false;
        isChain = false;
        highlight.enabled = false;
    }

    public void Activate()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { ClickLamp((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }

    public void SwitchLamp(bool active)
    {
        trigger.enabled = active;
    }

    public void Ignite()
    {
        if (!IsEmptyNeighbour())
        {
            return;
        }

        if (isChain)
        {
            for (int i = 0; i < neighbours.Length; i++)
            {
                neighbours[i].ChangeCondition(currentCondition);
            }
            return;
        }

        FillLamp targetLamp;
        do
        {
            targetLamp = neighbours[valueGenerator.GenerateInt(0, neighbours.Length)];
        }
        while (targetLamp.GetCondition() == currentCondition);

        targetLamp.ChangeCondition(currentCondition);
    }

    public void SetChained()
    {
        isChain = true;
        highlight.enabled = true;
    }

    public FillCondition GetCondition()
    {
        return currentCondition;
    }

    public bool IsEmptyNeighbour()
    {
        bool isEmpty = false;
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i].GetCondition() == FillCondition.Empty)
            {
                isEmpty = true;
                break;
            }
        }
        return isEmpty;
    }
}
