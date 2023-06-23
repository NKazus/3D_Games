using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BonusCrystal : MonoBehaviour
{
    [SerializeField] private CrystalColor[] crystalColors;

    private ColorInstance colorInstance;
    private EventTrigger trigger;

    private int bonusId;

    private Action<int> result;

    private void Awake()
    {
        colorInstance = GetComponent<ColorInstance>();
        trigger = GetComponent<EventTrigger>();
    }

    private void OnDisable()
    {
        Deactivate();
    }

    public Color SetRandom()
    {
        int colorIndex = RandomGenerator.GenerateInt(1, crystalColors.Length);
        colorInstance.SetHue(crystalColors[colorIndex].color);

        return crystalColors[colorIndex].color;
    }

    public void SetHue(CrystalColor target)
    {
        colorInstance.SetHue(target.color);
        bonusId = target.id;
    }

    public void PickCrystal(PointerEventData data)
    {
        result(bonusId);
    }

    public void Activate(Action<int> crystalCallback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { PickCrystal((PointerEventData)data); });
        trigger.triggers.Add(entry);

        result = crystalCallback;
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        result = null;
    }
}
