using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [SerializeField] private Text cardText;
    [SerializeField] private Vector3 endPosition;

    private Transform localTransform;
    private Vector3 startPosition;

    private void Awake()
    {
        localTransform = transform;
        startPosition = localTransform.position;
    }

    private void OnEnable()
    {
        localTransform.position = startPosition;
    }

    private void OnDisable()
    {
        DOTween.Kill("card");
    }

    public void Activate(int value, Action cardCallback)
    {     
        DOTween.Sequence()
            .SetId("card")
            .Append(localTransform.DOMove(startPosition, 1f))
                .AppendCallback(() => {
                    cardText.text = value.ToString();
                })
            .Append(localTransform.DOMove(endPosition, 2f)).
            OnComplete(() => { cardCallback(); });
    }

}
