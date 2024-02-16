using DG.Tweening;
using UnityEngine;

public class DOTweenKiller : MonoBehaviour
{
    [SerializeField] private string[] tweenIds;

    private void OnDisable()
    {
        for(int i = 0; i < tweenIds.Length; i++)
        {
            DOTween.Kill(tweenIds[i]);
        }
    }
}
