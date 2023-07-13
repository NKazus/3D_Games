using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceHandler : MonoBehaviour
{
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Vector3 middlePosition;
    [SerializeField] private Material[] diceMaterials;
    [SerializeField] private Vector3[] rotationValues;

    private Transform localTransform;
    private Vector3 startPosition;
    private MeshRenderer meshRenderer;
    private EventTrigger trigger;

    private int currentDiceValue;
    private bool locked;

    private void Awake()
    {
        localTransform = transform;
        startPosition = localTransform.position;

        meshRenderer = GetComponent<MeshRenderer>();
        trigger = GetComponent<EventTrigger>();
    }

    public void Throw(Action callback)
    {
        if (locked)
        {
            callback();
            return;
        }

        localTransform.position = startPosition;
        currentDiceValue = RandomGenerator.GenerateInt(1, 7);
        localTransform.rotation = Quaternion.Euler(rotationValues[currentDiceValue - 1]);

        DOTween.Sequence()
            .SetId(this)
            .Append(localTransform.DOJump(middlePosition, 0.15f, 1, 1f))
            .Append(localTransform.DOMove(endPosition, 0.7f))
            .OnComplete(() => { callback(); });
    }

    private void SwitchDice(PointerEventData data)
    {
        SetLock(!locked);
    }

    private void PickDice(PointerEventData data, Action<int> callback)
    {
        callback(currentDiceValue);
    }

    public int GetDiceValue()
    {
        return currentDiceValue;
    }

    public void SetLock(bool doLock)
    {
        locked = doLock;
        meshRenderer.material = locked ? diceMaterials[1] : diceMaterials[0];
    }

    public void ResetDice()
    {
        localTransform.position = startPosition;
        locked = false;
        meshRenderer.material = diceMaterials[0];
        DeactivateDice();
    }

    public void ActivateDice(Action<int> pickCallback = null)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        if(pickCallback == null)
        {
            entry.callback.AddListener((data) => { SwitchDice((PointerEventData)data); });
        }
        else
        {
            entry.callback.AddListener((data) => { PickDice((PointerEventData)data, pickCallback); });
        }
        trigger.triggers.Add(entry);

    }

    public void DeactivateDice()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
    }
}
