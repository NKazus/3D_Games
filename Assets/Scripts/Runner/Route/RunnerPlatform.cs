using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

public abstract class RunnerPlatform : MonoBehaviour
{
    [Inject] private readonly GameEventHandler eventHandler;
    [Inject] private readonly GameUpdateHandler updateHandler;

    //has rigidbody

    protected void Start()
    {
        //subscribe to platform movement event (spawn from container) GameEventHandler
    }

    protected void OnDisable()
    {
        SwitchMovement(false);
    }

    //(platform only have colliders and tags)

    //options: 1. set pos
    //2. init
    public abstract void DoPlatform();

    protected void SwitchMovement(bool doMove)
    {
        if (doMove)
        {
            updateHandler.GlobalUpdateEvent += Move;
        }
        else
        {
            updateHandler.GlobalUpdateEvent -= Move;
        }
    }

    protected void Move()
    {
        //update
    }
}
