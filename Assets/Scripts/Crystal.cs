using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public struct CrystalColor
{
    public Color color;
    public int id;
}
public class Crystal : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private CrystalColor[] crystalColors;

    private Transform localTransform;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private MeshRenderer meshRenderer;
    private EventTrigger trigger;

    private int currentColorIndex;
    private CrystalColor[] colorQueue;

    private Action<int> result;

    private void Awake()
    {
        localTransform = transform;
        startPosition = localTransform.position;
        startRotation = localTransform.rotation;

        meshRenderer = GetComponent<MeshRenderer>();
        trigger = GetComponent<EventTrigger>();

        colorQueue = new CrystalColor[crystalColors.Length];
        for (int i = 0; i < crystalColors.Length; i++)
        {
            colorQueue[i] = crystalColors[i];
        }
    }

    private void OnEnable()
    {
        localTransform.position = startPosition;
        ResetCrystal();
    }

    private void OnDisable()
    {
        Deactivate();
    }

    public void Roll(Action<int> callback)
    {
        localTransform.position = startPosition;
        localTransform.rotation = startRotation;

        int colorIndex = RandomGenerator.GenerateInt(1, crystalColors.Length);
        meshRenderer.material.color = crystalColors[colorIndex].color;

        DOTween.Sequence()
            .SetId(this)
            .Append(localTransform.DOMove(targetPosition, 2f))
            .Join(localTransform.DOLocalRotate(new Vector3(-90, 90, 90), 2f))
            .OnComplete(() => { callback(crystalColors[colorIndex].id); });
    }

    public void SwitchCrystal(PointerEventData data)
    {
        currentColorIndex = (currentColorIndex < colorQueue.Length - 1) ? ++currentColorIndex : 0;

        DOTween.Sequence()
            .SetId(this)
            .Append(meshRenderer.material.DOColor(colorQueue[currentColorIndex].color, 0.5f))
            .Join(localTransform.DOShakeRotation(0.5f, new Vector3(20, 0, 0), 0, 90, true));

        result(colorQueue[currentColorIndex].id);
    }

    public void ResetCrystal()
    {
        currentColorIndex = 0;
        meshRenderer.material.color = crystalColors[0].color;
    }

    public void Activate(Action<int> crystalCallback)
    {
        RandomGenerator.RandomizeArray(colorQueue);

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { SwitchCrystal((PointerEventData)data); });
        trigger.triggers.Add(entry);

        result = crystalCallback;
    }

    public void Deactivate()
    {
        trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        result = null;
    }
}
