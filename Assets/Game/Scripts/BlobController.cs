using DG.Tweening;
using UnityEngine;

public class BlobController : MonoBehaviour
{
    [SerializeField] private Transform shieldTransform;

    private Transform blobTransform;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private BlobEmission blobEmission;

    private float shieldRadius;

    private void Awake()
    {
        blobTransform = transform;
        initialPosition = blobTransform.localPosition;
        initialScale = blobTransform.localScale;
        shieldRadius = shieldTransform.localScale.x / 2;
        blobEmission = GetComponent<BlobEmission>();
    }

    private bool CheckBounds()
    {
        bool isInside = Vector3.Distance(blobTransform.localPosition, shieldTransform.localPosition) <= shieldRadius;
        if (!isInside)
        {
            DOTween.Sequence()
                .SetId("blob")
                .Append(blobTransform.DOShakeScale(0.5f, 1, 5, 60))
                .Append(blobTransform.DOScale(0f, 0.1f))
                .OnComplete(() => blobEmission.StopEmitting());
        }
        return isInside;
    }

    public void MoveBlob(Vector3 direction, System.Action<bool> callback)
    {
        blobTransform.DOLocalMove(blobTransform.localPosition + direction, 1f)
            .SetId("blob")
            .OnComplete(() => callback(CheckBounds()));
    }

    public void ResetBlob()
    {
        blobTransform.localPosition = initialPosition;
        blobTransform.localScale = initialScale;
        blobEmission.StartEmitting();
    }
}
