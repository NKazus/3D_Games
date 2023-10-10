using System;
using DG.Tweening;
using UnityEngine;

public class StickComponent : MonoBehaviour
{
    [SerializeField] private Color componentColor;
    [SerializeField] private Material componentMaterial;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private bool isTarget;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetScale;

    private MeshRenderer stickRenderer;
    private Material stickMaterial;

    private Transform stickTransform;
    private Vector3 initialScale;
    private Vector3 initialPosition;

    private Action mergeCallback;

    private void Awake()
    {
        stickTransform = transform;
        initialScale = stickTransform.localScale;
        initialPosition = stickTransform.localPosition;

        stickRenderer = GetComponent<MeshRenderer>();
        stickMaterial = Instantiate(componentMaterial);
        stickMaterial.color = componentColor;
        stickRenderer.material = stickMaterial;
    }

    private void OnEnable()
    {
        stickTransform.localScale = initialScale;
        ResetComponent();
    }

    private void CompleteTarget()
    {
        stickTransform.DOShakeScale(1f, new Vector3(0.1f, 0.5f, 0.1f), 3, 50f)
            .OnComplete(() => {
                stickRenderer.material = targetMaterial;
                stickTransform.DOScale(targetScale, 0.5f)
                .OnComplete(() => mergeCallback());
            }) ;
    }

    private void HideStick()
    {
        stickTransform.localScale = Vector3.zero;
    }

    public void ResetComponent()
    {
        stickRenderer.material = stickMaterial;
        stickTransform.position = initialPosition;
    }

    public void SetScale(float scaleMultiplyer)
    {
        stickTransform.DOScale(new Vector3(initialScale.x, initialScale.y * scaleMultiplyer, initialScale.z), 0.5f);
    }

    public void MergeToTarget(Action callback)
    {
        mergeCallback = callback;
        Action completeAction;        
        completeAction = isTarget ? CompleteTarget : HideStick;
        stickTransform.DOLocalMove(targetPosition, 0.5f).OnComplete(() => completeAction());
    }
}
