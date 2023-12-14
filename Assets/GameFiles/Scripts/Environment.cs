using DG.Tweening;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private MeteorCut[] meteors;

    private void OnEnable()
    {
        for (int i = 0; i < meteors.Length; i++)
        {
            meteors[i].Init();
            meteors[i].MoveMeteor();
        }
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }
}
