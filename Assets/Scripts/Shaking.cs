using DG.Tweening;
using UnityEngine;

public class Shaking : MonoBehaviour
{
    [SerializeField] private float scaleValue;
    [SerializeField] private float timeValue;

    private Transform localTransform;
    private float scale;

    private Sequence shakingTween;

    private void Awake()
    {
        localTransform = transform;
        scale = localTransform.localScale.z;
    }

    private void OnEnable()
    {
        shakingTween = DOTween.Sequence()
            .SetId(this)
            .Append(localTransform.DOScale(scale * scaleValue, timeValue))
            .Append(localTransform.DOScale(scale, timeValue))
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        shakingTween.Rewind();
    }

    public void UpdateShaking(float scale, float time)
    {
        scaleValue = scale;
        timeValue = time;
    }
}
