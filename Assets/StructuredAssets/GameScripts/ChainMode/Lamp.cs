using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Lamp : MonoBehaviour
{
    [SerializeField] private Lamp[] neighbours;

    private Lamp inputNeighbour;
    private LampVisual visualComponent;

    private EventTrigger trigger;
    private LampCondition currentCondition;

    private bool isLinked;

    private System.Action<Lamp> LampCallback;

    [Inject] private readonly ValueGenerator valueGenerator;

    private void ClickLamp(PointerEventData data)
    {
        if (!isLinked)
        {
            return;
        }

        Ignite(null);

        if(LampCallback != null)
        {
            LampCallback(this);
        }
    }

    private bool ChangeCondition(bool up)
    {
        int currentConditionId = (int) currentCondition;
        int conditions = System.Enum.GetNames(typeof(LampCondition)).Length;
        currentConditionId += up ? 1 : -1;

        LampCondition newCondition = (LampCondition) Mathf.Clamp(currentConditionId, 0, conditions - 1);
        if (newCondition == currentCondition)
        {
            return false;
        }

        currentCondition = newCondition;
        visualComponent.UpdateStatus(currentCondition);
        isLinked = (currentCondition == LampCondition.Off) ? false : true;
        return true;
    }

    public void Initialize(System.Action<Lamp> callback)
    {
        visualComponent = transform.GetChild(0).GetComponent<LampVisual>();
        visualComponent.Init();

        trigger = GetComponent<EventTrigger>();
        LampCallback = callback;
    }

    public void ResetLamp()
    {
        currentCondition = LampCondition.Off;
        visualComponent.UpdateStatus(currentCondition);
        inputNeighbour = null;
        isLinked = false;
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
        if(input == null)//player
        {
            if (ChangeCondition(true))//if can be ingnited further
            {
                switch (currentCondition)
                {
                    case LampCondition.Single:
                        break;
                    case LampCondition.Pair:
                        if(neighbours.Length < 2 && neighbours[0] == inputNeighbour)
                        {
                            break;
                        }
                        Lamp targetLamp;
                        do
                        {
                            targetLamp = neighbours[valueGenerator.GenerateInt(0, neighbours.Length)];
                        }
                        while (targetLamp == inputNeighbour);

                        targetLamp.Ignite(this);
                        break;
                    case LampCondition.Chain:
                        for (int i = 0; i < neighbours.Length; i++)
                        {
                            if(neighbours[i] == inputNeighbour)
                            {
                                continue;
                            }
                            neighbours[i].Ignite(this);
                        }
                        break;
                    default: break;
                }
            }
        }
        else//chain
        {
            if(currentCondition == LampCondition.Off)
            {
                inputNeighbour = input;
                ChangeCondition(true);
            }            
        }

        //надо проверять (мб флаг в параметр для различия игрока и цепной работы, как зажигаем,
        //чтобы при переходе с синего на зеленый не активировать уже зажженную лампу еще сильнее)

        //детям тоже
        //родителю не шлем       
    }

    public void Extinguish(Lamp input)
    {
        if(currentCondition == LampCondition.Off)
        {
            return;
        }

        if(input == null)//single
        {
            Debug.Log("Event target: "+gameObject.name);
            ChangeCondition(false);
            inputNeighbour.Extinguish(this);
            inputNeighbour = null;
        }
        else// pair/chain
        {
            Debug.Log("Event parent: " + gameObject.name);
            if (currentCondition == LampCondition.Chain)
            {
                ChangeCondition(false);
            }
            else//if pair
            {
                bool isActiveNeighbour = false;
                for (int i = 0; i < neighbours.Length; i++)
                {
                    if (neighbours[i] == inputNeighbour || neighbours[i] == input)
                    {
                        continue;
                    }
                    if(neighbours[i].GetCondition() != LampCondition.Off)
                    {
                        isActiveNeighbour = true;
                    }
                }
                if (isActiveNeighbour)
                {
                    return;
                }
                ChangeCondition(false);
            }
        }
        //отсылаем детям то же
        //если инпут параметр не нул, то всем детям кроме старого родителя и этого свежего инпута

        //если тухнем то и родителю, но не свежему инпуту
        //мб сделать доп флаг, чтобы отслеживать команду родителям, чтобы оно не тушило остальных сиблингов при снижении уровня
    }

    public LampCondition GetCondition()
    {
        return currentCondition;
    }
}
