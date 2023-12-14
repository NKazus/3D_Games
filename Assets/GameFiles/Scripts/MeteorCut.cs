using DG.Tweening;
using UnityEngine;

public class MeteorCut : MonoBehaviour
{
    [SerializeField] private Trail trail;
    [SerializeField] private float time;
    [SerializeField] private bool explode;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 stop;

    private Transform meteorTransform;
    private Vector3 meteorScale;

    private void OnDisable()
    {
        meteorTransform.localScale = meteorScale;
        //meteorTransform.DOKill();
    }

    public void Init()
    {
        meteorTransform = transform;
        meteorScale = meteorTransform.localScale;
        meteorTransform.localPosition = start;
        trail.DoTrail(true);
    }

    public void MoveMeteor()
    {
        meteorTransform.localPosition = start;
        meteorTransform.localRotation = Random.rotation;
        meteorTransform.localScale = meteorScale;

        //trail.DoTrail(true);

        if (explode)
        {
            DOTween.Sequence()
                .Append(meteorTransform.DOLocalMove(stop, time).SetEase(Ease.Linear)
                    .OnComplete(() => { /*trail.DoTrail(false);*/ trail.DoExplosion(); }))
                .Append(meteorTransform.DOScale(0f, 0.1f))
                .AppendInterval(1f)
                .OnComplete(() => MoveMeteor());
        }
        else
        {
            meteorTransform.DOLocalMove(stop, time).OnComplete(() => MoveMeteor());
        }
    }


}
