using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Stick : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3[] startPositions;

    private Transform stickTransform;
    private CapsuleCollider stickCollider;

    private int currentPosition;

    [Inject] private readonly InGameEvents eventManager;

    private void Awake()
    {
        stickTransform = transform;
        stickCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        stickTransform.localPosition = startPositions[0];
    }

    private void OnDisable()
    {
        DOTween.Kill("stick_move");
        DOTween.Kill("stick_set");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Pin"))
        {
            eventManager.BreakStick();
            stickCollider.enabled = false;
            DOTween.Kill("stick_move");
        }
    }

    public void SetStick(int stage, Action stickCallback = null)
    {
        currentPosition = stage;
        stickTransform.DOLocalMove(startPositions[currentPosition], 0.7f)
            .SetId("stick_set")
            .OnComplete(() => {
                stickCollider.enabled = true;
                if (stickCallback != null)
                {
                    stickCallback();
                }
            });
    }

    public void MoveStick(Action moveCallback)
    {
        stickTransform.DOLocalMove(targetPosition, 0.5f).SetId("stick_move")
            .OnComplete(() => {
                stickTransform.DOLocalMove(startPositions[currentPosition], 0.5f).SetId("stick_move")
                .OnComplete(() => { moveCallback(); });
                });
    }

    public void HideStick(bool hide)
    {
        gameObject.SetActive(hide);
    }
}
