using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CupHandler : MonoBehaviour
{
    private Transform localTransform;
    private Vector3 initialPosition;
    private bool isTreasure = false;
    private EventTrigger trigger;

    private Action<bool> result;

    private float jumpPower;

    private void Awake()
    {
        localTransform = transform;
        initialPosition = localTransform.position;
        trigger = GetComponent<EventTrigger>();
    }

    private void OnDisable()
    {
        DeactivateCup();
    }

    private void PickCup(PointerEventData data)
    {
        result(isTreasure);
    }

    public void MoveCup(Vector3 destination, Action shiftCallback)
    {
        localTransform.DOJump(destination, jumpPower, 1, 1f).OnComplete(() => { shiftCallback(); });
    }

    public void ResetCup()
    {
        localTransform.position = initialPosition;
        isTreasure = false;
    }

    public void AttachDice(Transform diceTransform)
    {
        diceTransform.SetParent(localTransform);
        isTreasure = true;
    }

    public void ActivateCup(Action<bool> diceCallback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { PickCup((PointerEventData)data); });
        trigger.triggers.Add(entry);

        result = diceCallback;
    }

    public void DeactivateCup()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        result = null;
    }

    public void SetJumpForce(float force)
    {
        jumpPower = force;
    }
}
