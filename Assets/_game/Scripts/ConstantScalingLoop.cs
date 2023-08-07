using DG.Tweening;
using UnityEngine;

public class ConstantScalingLoop : MonoBehaviour
{
    private Transform localTransform;
    private Sequence scalingTween;

    private void Awake()
    {
        localTransform = transform;
    }

    private void OnEnable()
    {
        scalingTween = DOTween.Sequence()
            .Append(localTransform.DOScaleZ(0.4f, 0.5f))
            .Append(localTransform.DOScaleZ(0.3f, 0.5f))
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        scalingTween.Rewind();
    }
}
