using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform coinTransform;
    private Tween rotationTween;

    private void Awake()
    {
        coinTransform = transform;
        rotationTween = coinTransform.DORotate(new Vector3(0, 50, 0), 1f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        rotationTween.Rewind();
    }

    private void OnEnable()
    {
        rotationTween.Play();
    }

    private void OnDisable()
    {
        rotationTween.Rewind();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //GlobalEventManager.CollectCoin();
            this.gameObject.SetActive(false);
        }
    }
}
