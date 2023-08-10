using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private float[] growValues;

    private Transform target;
    private Transform flower;
    private Vector3 flowerScale;

    private Sequence shakingSequence;
    private Shaking shaking;

    private void Awake()
    {
        target = transform;
        flower = target.GetChild(0);
        flowerScale = flower.localScale;
        shakingSequence = DOTween.Sequence()
                .Append(target.DOShakeRotation(1.5f, 2, 10, 10))
                .SetAutoKill(false)
                .SetLoops(-1);
        shaking = GetComponent<Shaking>();
    }

    public void DoShake(bool active)
    {
        if (active)
        {
            shaking.enabled = false;
            shakingSequence.Play();
        }
        else
        {
            shakingSequence.Rewind();
            shaking.enabled = true;
        }
    }

    public void DoFlower(bool isFlower)
    {
        flower.DOScale(isFlower ? flowerScale : Vector3.zero, 0.5f);
    }

    public void Grow(int stage)
    {
        target.DOLocalMoveY(growValues[stage], 0.7f);
    }

    public int GetGrowIterations()
    {
        return growValues.Length;
    }
}
