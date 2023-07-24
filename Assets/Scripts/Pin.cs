using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Pin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float moveTime;

    private Transform[] gears;
    private Transform movingPart;
    private Quaternion[] gearsRotation;

    private float gearSpeed;

    private Sequence movingSequence;

    [Inject] private readonly UpdateManager updateManager;

    private void Awake()
    {
        Transform pinTransform = transform;
        gears = new Transform[2];
        gearsRotation = new Quaternion[2];
        gears[0] = pinTransform.GetChild(0);
        gearsRotation[0] = gears[0].rotation;
        gears[1] = pinTransform.GetChild(1);
        gearsRotation[1] = gears[1].rotation;
        movingPart = pinTransform.GetChild(2);
    }

    private void OnEnable()
    {
        gears[0].rotation = gearsRotation[0];
        gears[1].rotation = gearsRotation[1];
    }

    private void OnDisable()
    {
        RewindPin();
    }

    private void RotateGears()
    {
        gears[0].Rotate(new Vector3(0, 0, gearSpeed * Time.deltaTime));
        gears[1].Rotate(new Vector3(0, 0, gearSpeed * Time.deltaTime));
    }

    public void ResetPin()
    {
        gearSpeed = rotationSpeed;
        updateManager.UpdateEvent += RotateGears;
        movingPart.localPosition = Vector3.zero;
        RewindPin();
    }

    public void ActivatePin()
    {
        gearSpeed = rotationSpeed * 2f;

        movingPart.DOLocalMove(startPosition, moveTime / 2f).OnComplete(() => movingSequence.Play());
    }

    public void DeactivatePin()
    {
        updateManager.UpdateEvent -= RotateGears;
        movingSequence.Kill();
    }

    public void RewindPin()
    {
        movingSequence = DOTween.Sequence()
            .Append(movingPart.DOLocalMove(endPosition, moveTime))
            .Append(movingPart.DOLocalMove(startPosition, moveTime))
            .SetLoops(-1);
    }
}
