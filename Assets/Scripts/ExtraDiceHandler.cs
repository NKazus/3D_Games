
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraDiceHandler : MonoBehaviour
{
    [SerializeField] private Vector3[] rotationValues;

    private Transform localTransform;
    private EventTrigger trigger;
    private Vector3 initialScale;

    private void Awake()
    {
        localTransform = transform;
        initialScale = localTransform.localScale;
        trigger = GetComponent<EventTrigger>();
    }

    private void Click(PointerEventData data, Action<int> callback)
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        Throw(callback, true);
    }

    public void Throw(Action<int> callback, bool player)
    {
        int currentDiceValue = RandomGenerator.GenerateInt(1, 7);
        localTransform.rotation = Quaternion.Euler(rotationValues[currentDiceValue - 1]);

        DOTween.Sequence()
            .SetId("Extra")
            .Append(localTransform.DOShakeScale(player ? 0.7f : 1f, 2, 7, 90, true))
            .OnComplete(() => { callback(currentDiceValue); });
    }

    public void Activate(Action<int> callback, bool player = false)
    {
        if (player)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { Click((PointerEventData)data, callback); });
            trigger.triggers.Add(entry);
        }
        else
        {
            Throw(callback, false);
        }
    }    

    public void ResetState()
    {
        if (trigger.triggers != null)
        {
            trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }
        DOTween.Kill("Extra");
        localTransform.localScale = initialScale;
    }
}
