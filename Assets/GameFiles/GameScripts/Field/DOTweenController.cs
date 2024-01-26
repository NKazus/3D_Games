using DG.Tweening;
using UnityEngine;

public class DOTweenController : MonoBehaviour
{
    private void OnDisable()
    {
        DOTween.Kill("submenu");
        DOTween.Kill("unit");
    }
}
