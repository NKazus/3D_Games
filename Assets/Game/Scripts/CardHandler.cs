using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [SerializeField] private Text cardText;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private int minCardValue = 10;
    [SerializeField] private int maxCardValue = 25;

    private Transform localTransform;
    private Vector3 startPosition;

    private int currentValue;

    private void Awake()
    {
        localTransform = transform;
        startPosition = localTransform.position;
    }

    private void OnEnable()
    {
        localTransform.position = startPosition;
    }

    public int Activate()
    {
        currentValue = RandomGenerator.GenerateInt(minCardValue, maxCardValue + 1);

        DOTween.Sequence()
            .SetId(this)
            .Append(localTransform.DOMove(startPosition, 1f))
                .AppendCallback(() => {
                    cardText.text = currentValue.ToString();
                })
            .Append(localTransform.DOMove(endPosition, 2f)).
            OnComplete(() => { GlobalEventManager.ActivateDices(); });

        return currentValue;
    }

}
