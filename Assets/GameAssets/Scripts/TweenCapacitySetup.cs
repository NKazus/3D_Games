using DG.Tweening;
using UnityEngine;

public class TweenCapacitySetup : MonoBehaviour
{
    void Awake()
    {
        DOTween.SetTweensCapacity(200, 60);
    }
}
