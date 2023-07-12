using DG.Tweening;
using UnityEngine;

public class Shaking : MonoBehaviour
{
    [SerializeField] private float moveValueY = 0.085f;
    [SerializeField] private float timeValue = 0.7f;

    private Transform localTransform;
    private float localPositionY;

    private Sequence shakingTween;

    private void Awake()
    {
        localTransform = transform;
        localPositionY = localTransform.localPosition.y;

        shakingTween = DOTween.Sequence()
            .SetId("shake")
            .Append(localTransform.DOLocalMoveY(moveValueY, timeValue))
            .Append(localTransform.DOLocalMoveY(localPositionY, timeValue))
            .SetLoops(-1);
    }

    private void OnEnable()
    {
        shakingTween.Play();
    }

    private void OnDisable()
    {
        shakingTween.Rewind();
    }
}
