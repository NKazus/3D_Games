using DG.Tweening;
using UnityEngine;

public class Shaking : MonoBehaviour
{
    [SerializeField] private float scaleValue;
    [SerializeField] private float timeValue;

    private Transform localTransform;
    private float scale;
    private float timeMultiplyer;

    private Sequence shakingTween;

    private void Awake()
    {
        localTransform = transform;
        scale = localTransform.localScale.z;
        timeMultiplyer = 1f;
    }

    private void OnEnable()
    {
        shakingTween = DOTween.Sequence()
            .SetId(this)
            .Append(localTransform.DOScaleZ(scale * scaleValue, timeValue * timeMultiplyer))
            .Append(localTransform.DOScaleZ(scale, timeValue * timeMultiplyer))
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        shakingTween.Rewind();
    }
}
