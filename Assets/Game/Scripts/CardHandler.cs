using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CardSwitch))]
public class CardHandler : MonoBehaviour
{
    [SerializeField] private Vector3 showPosition;
    [SerializeField] private Vector3 endPosition;

    private Transform localTransform;
    private Vector3 startPosition;
    private Vector3 startScale;
    private CardSwitch cardSwitch;

    private bool init = false;

    private void Awake()
    {
        localTransform = transform;
        startPosition = localTransform.position;
        startScale = localTransform.localScale;

        if (!init)
        {
            cardSwitch = GetComponent<CardSwitch>();
            init = true;
        }
    }

    private void OnEnable()
    {
        localTransform.position = startPosition;
    }

    private void OnDisable()
    {
        DOTween.Kill("switch");
        DOTween.Kill("show");
    }

    public void SwitchCard(Card target, bool animate = false)
    {
        if (!animate)
        {
            if (!init)
            {
                cardSwitch = GetComponent<CardSwitch>();
                init = true;
            }
            cardSwitch.Switch(target.image);
            return;
        }
        DOTween.Sequence()
            .SetId("switch")
            .Append(localTransform.DOScale(Vector3.zero, 0.5f)
                .OnComplete(() => { cardSwitch.Switch(target.image); }))
            .Append(localTransform.DOScale(startScale, 0.5f));
    }

    public void ShowCard(Action callback)
    {
        localTransform.position = startPosition;
        DOTween.Sequence()
            .SetId("show")
            .Append(localTransform.DOMove(showPosition, 0.8f))
            .Append(localTransform.DOScale(startScale * 1.5f, 0.5f))
            .Append(localTransform.DOScale(startScale, 0.5f))
            .Append(localTransform.DOMove(endPosition, 0.8f))
            .OnComplete(() => callback());
    }

    public void ResetCardPosition()
    {
        localTransform.position = startPosition;
    }
}
