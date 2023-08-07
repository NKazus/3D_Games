using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private Transform target;
    private Transform flower;
    private Vector3 flowerScale;

    private void Awake()
    {
        target = transform;
        flower = target.GetChild(0);
        flowerScale = flower.localScale;
    }

    public void DoShake(bool active)
    {

    }

    public void DoFlower(bool isFlower)
    {
        flower.DOScale(isFlower ? flowerScale : Vector3.zero, 0.5f);
    }
}
