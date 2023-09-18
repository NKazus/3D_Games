using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    private Transform cardTransform;
    private Vector3 initialScale;
    private Text cardText;

    private bool isActive;

    private void Awake()
    {
        cardTransform = transform;
        initialScale = cardTransform.localScale;
        cardText = cardTransform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    private void OnEnable()
    {
        isActive = false;
        cardText.enabled = false;
        cardTransform.localScale = initialScale;
    }

    public void ResetCard()
    {
        if (!isActive)
        {
            return;
        }
        DOTween.Sequence()
            .SetId("balance_card")
            .Append(cardTransform.DOScale(Vector3.zero, 0.3f).OnComplete(() => cardText.enabled = false))
            .Append(cardTransform.DOScale(initialScale, 0.3f));
        isActive = false;
    }

    public void SetCard(int value, Action callback)
    {
        DOTween.Sequence()
            .SetId("balance_card")
            .Append(cardTransform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                cardText.text = value.ToString();
                cardText.enabled = true;
            }))
            .Append(cardTransform.DOScale(initialScale, 0.3f))
            .OnComplete(() => callback());
        isActive = true;
    }
}
