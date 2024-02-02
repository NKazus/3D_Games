using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lamp : MonoBehaviour
{
    [SerializeField] private Lamp[] neighbours;

    private Lamp inputNeighbour;
    private LampVisual visualComponent;

    private EventTrigger trigger;
    private LampCondition currentCondition;

    private bool isLinked;
    private bool isDestination;

    private System.Action<Lamp> LampCallback;
    private System.Action DestinationCallback;

    private void ClickLamp(PointerEventData data)
    {
        if (!isLinked)
        {
            return;
        }

        if(LampCallback != null)
        {
            LampCallback(this);
        }
    }

    private void ChangeCondition(bool up)
    {
        int currentConditionId = (int) currentCondition;
        int conditions = System.Enum.GetNames(typeof(SlotCondition)).Length;
        currentConditionId += up ? 1 : -1;

        LampCondition newCondition = (LampCondition) Mathf.Clamp(currentConditionId, 0, conditions - 1);
        if (newCondition == currentCondition)
        {
            return;
        }

        currentCondition = newCondition;
        isLinked = (currentCondition == LampCondition.Off) ? false : true;

        TriggerNeighboursChange();
    }

    private void TriggerNeighboursChange()
    {
        switch (currentCondition)
        {
            
        }
    }

    public void Initialize(System.Action<Lamp> callback)
    {
        visualComponent = transform.GetComponent<LampVisual>();
        trigger = GetComponent<EventTrigger>();
        LampCallback = callback;
    }

    public void ResetLamp()
    {
        currentCondition = LampCondition.Off;
        inputNeighbour = null;
        isLinked = false;
        isDestination = false;
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

    public void Ignite(Lamp input)
    {
        inputNeighbour = input;
        ChangeCondition(true);

        //надо проверять (мб флаг в параметр для различия игрока и цепной работы, как зажигаем,
        //чтобы при переходе с синего на зеленый не активировать уже зажженную лампу еще сильнее)

        //детям тоже
        //родителю не шлем
        if (isDestination && DestinationCallback != null)
        {
            DestinationCallback();
        }        
    }

    public void Extinguish(Lamp input)
    {
        ChangeCondition(false);
        //отсылаем детям то же
        //если инпут параметр не нул, то всем детям кроме старого родителя и этого свежего инпута

        //если тухнем то и родителю, но не свежему инпуту
        //мб сделать доп флаг, чтобы отслеживать команду родителям, чтобы оно не тушило остальных сиблингов при снижении уровня


    }

    public LampCondition GetCondition()
    {
        return currentCondition;
    }

    public void SetDestination()
    {
        isDestination = true;
    }
}
