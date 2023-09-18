using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [SerializeField] private Text cardText;
    [SerializeField] private ParticleSystem effect;

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

    public void Activate(int value, Action cardCallback)
    {
        effect.Stop();
        DOTween.Sequence()
            .SetId("card")
            .Append(localTransform.DOScale(Vector3.zero, 0.7f))
                .AppendCallback(() => {
                    cardText.text = value.ToString();
                })
            .Append(localTransform.DOScale(initialScale, 0.7f)).
            OnComplete(() => { effect.Play(); cardCallback(); });
    }

}
