using DG.Tweening;
using UnityEngine;

public class DOTweenController : MonoBehaviour
{
    [SerializeField] private string tweenId;

    private void OnDisable()
    {
        DOTween.Kill(tweenId);
    }

    public string GetId()
    {
        return tweenId;
    }
}
