using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    private Transform localTransform;
    private Vector3 initialScale;

    private void Awake()
    {
        localTransform = transform;
        initialScale = localTransform.localScale;
    }

    private void OnEnable()
    {
        localTransform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        DOTween.Kill("card");
    }

    public void Activate(Color value, Action cardCallback)
    {        
        DOTween.Sequence()
            .SetId("card")
            .Append(localTransform.DOScale(Vector3.zero, 0.5f))
                .AppendCallback(() => {
                    cardImage.color = value;
                })
            .Append(localTransform.DOScale(initialScale, 0.5f)).
            OnComplete(() => cardCallback());
    }

    public void SetColor()
    {
        cardImage.color = Color.gray;
    }
}
