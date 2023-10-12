using DG.Tweening;
using UnityEngine;

public class ScannerElement : MonoBehaviour
{
    [SerializeField] private float lowerLimitZ;
    [SerializeField] private float upperLimitZ;

    private Transform elemTransform;
    private float pathLenght;

    private void Awake()
    {
        elemTransform = transform;
        pathLenght = upperLimitZ - lowerLimitZ;
    }

    private void OnEnable()
    {
        elemTransform.localPosition = new Vector3(elemTransform.localPosition.x, elemTransform.localPosition.y, 0f);
    }

    public void SetPosition(float value)//percentage
    {
        elemTransform.DOLocalMoveZ(lowerLimitZ + (value * pathLenght), 0.5f);
    }
}
