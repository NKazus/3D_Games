using DG.Tweening;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    [SerializeField] private float rotatingValue = 360;
    [SerializeField] private float timeValue = 1f;

    private Transform localTransform;

    private Sequence spinningTween;

    private void Awake()
    {
        localTransform = transform;

        spinningTween = DOTween.Sequence()
            .SetId("spin")
            .Append(localTransform.DOLocalRotate(new Vector3(0, rotatingValue, 0), timeValue))
            .SetLoops(-1, LoopType.Incremental);
    }

    private void OnEnable()
    {
        spinningTween.Play();
    }

    private void OnDisable()
    {
        spinningTween.Rewind();
    }
}
