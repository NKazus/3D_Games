using DG.Tweening;
using UnityEngine;

public class BonusPlayer : MonoBehaviour
{
    private Transform localTransform;
    private Vector3 initialPosition;

    private void Awake()
    {
        localTransform = transform;
        initialPosition = localTransform.position;
    }

    private void OnEnable()
    {
        GlobalEventManager.MovePlayerEvent += Move;
    }

    private void OnDisable()
    {
        localTransform.position = initialPosition;
        GlobalEventManager.MovePlayerEvent -= Move;
    }

    private void Move(Vector3 pos)
    {
        localTransform.DOMove(pos, 1f);
    }

}
