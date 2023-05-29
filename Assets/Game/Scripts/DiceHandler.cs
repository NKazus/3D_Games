using System;
using DG.Tweening;
using UnityEngine;

public class DiceHandler : MonoBehaviour
{
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Material[] diceMaterials;
    [SerializeField] private Vector3[] rotationValues;

    private Transform localTransform;
    private Vector3 startPosition;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        localTransform = transform;
        startPosition = localTransform.position;

        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Throw(Action callback, int materialID, int value)
    {
        localTransform.position = startPosition;
        localTransform.rotation = Quaternion.Euler(rotationValues[value - 1]);
        meshRenderer.material = diceMaterials[materialID];

        DOTween.Sequence()
            .SetId(this)
            .Append(localTransform.DOMove(endPosition, 2f))
            .Join(localTransform.DOShakeRotation(2f, 65, 7, 90, true))
            .OnComplete(() => { callback(); });
    }

    public void ResetDice()
    {
        localTransform.position = startPosition;
    }
}
