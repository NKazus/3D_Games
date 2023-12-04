using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 stopPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float movingTime;

    [SerializeField] private Engines engines;

    private Transform shipTransform;
    private bool init = false;

    [Inject] private readonly GameEventHandler events;

    private void Awake()
    {
        shipTransform = transform;
        init = true;
    }

    private void Shake()
    {
        DOTween.Sequence()
            .SetId("ship_shake")
            .Append(shipTransform.DOMoveY(stopPos.y + 0.1f, 1f))
            .Append(shipTransform.DOMoveY(stopPos.y, 1f))
            .SetLoops(-1);
    }

    public void HideShip(Action callback)
    {
        DOTween.Kill("ship_shake");
        events.PlaySound(AudioEffect.Engine);
        engines.BoostEngines();
        shipTransform.DOMove(endPos, 2f)
            .SetId("ship")
            .OnComplete(() => { engines.ShutEngines(false); callback(); });
    }

    public void ShowShip(Action callback)
    {
        shipTransform.position = startPos;
        engines.ShutEngines(true);
        engines.BoostEngines();
        shipTransform.DOMove(stopPos, 1.5f)
            .SetId("ship")
            .OnComplete(() => {
                engines.SlowEngines();
                Shake();
                callback(); });
    }

    public void ResetShip()
    {
        engines.ShutEngines(false);
        shipTransform.position = endPos;
        DOTween.Kill("ship");
        DOTween.Kill("ship_shake");
    }

    public void MoveShip()
    {        
        shipTransform.position = startPos;
        engines.ShutEngines(true);
        shipTransform.DOMove(endPos, movingTime)
            .SetId("ship_move")
            .OnComplete(() => {
                engines.ShutEngines(false);
                Invoke("MoveShip", 1f);
            });
    }

    public void Init()
    {
        if (!init)
        {
            shipTransform = transform;
            init = true;
        }
    }
}
