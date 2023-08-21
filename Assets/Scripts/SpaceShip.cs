using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 stopPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private Engines engines;

    private Transform shipTransform;
    private Vector3 initialPos;

    private void Awake()
    {
        shipTransform = transform;
        initialPos = shipTransform.localPosition;
    }

    private void Shake()
    {
        DOTween.Sequence()
            .SetId("ship_shake")
            .Append(shipTransform.DOLocalMoveY(initialPos.y + 0.1f, 1f))
            .Append(shipTransform.DOLocalMoveY(initialPos.y, 1f))
            .SetLoops(-1);
    }

    public void HideShip(Action callback)
    {
        DOTween.Kill("ship_shake");
        //engines.BoostEngines();
        shipTransform.DOLocalMove(endPos, 1f)
            .SetId("ship")
            .OnComplete(() => callback());
    }

    public void ShowShip(Action callback)
    {
        shipTransform.localPosition = startPos;
        //engines.BoostEngines();
        shipTransform.DOLocalMove(stopPos, 1f)
            .SetId("ship")
            .OnComplete(() => {
                //engines.SlowEngines();
                Shake();
                callback(); });
    }

    public void ResetShip()
    {
        shipTransform.localPosition = initialPos;
        DOTween.Kill("ship");
        DOTween.Kill("ship_shake");
    }
}
