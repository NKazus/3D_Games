using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using FitTheSize.GameServices;

public class RunManager : MonoBehaviour
{
    [SerializeField] private PlatformHandler route;
    [SerializeField] private Player player;
    [SerializeField] private Button startButton;

    [Header("Scaling parameters:")]
    [SerializeField] private float runSpeed;//move to data storage
    [SerializeField] private float boostMultiplyer;// ds
    [SerializeField] private float commonMultiplyer;// ds
    [SerializeField] private float scaleSpeed;

    [Inject] private readonly GameEventHandler eventHandler;
    [Inject] private readonly GameUpdateHandler updateHandler;

    private void Awake()
    {
        startButton.onClick.AddListener(Initialize);
        player.SetupPlayer(HandleCollision, updateHandler);
    }

    private void OnEnable()
    {
        route.ResetRoute();
        startButton.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        eventHandler.GameStateEvent -= Activate;
    }

    private void Initialize()
    {
        startButton.gameObject.SetActive(false);
        Activate(true);
        eventHandler.GameStateEvent += Activate;
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            Debug.Log("RESTART");
            route.SetRouteSpeed(runSpeed);
            route.ResetRoute();
            player.ResetPlayer();
            player.ActivateCollisions(true);
            route.MoveRoute();
        }
        else
        {
            route.StopRoute();
            player.ActivateCollisions(false);
            player.ActivateScaling(false);
        }
    }

    public void StopRoute()
    {
        eventHandler.SwitchGameState(false);
    }

    #region PLAYER
    private void HandleCollision(PlayerEvent playerEvent, bool enterCollision)
    {
        if(playerEvent == PlayerEvent.Wall)
        {
            Debug.Log("death");
            eventHandler.SwitchGameState(false);
            return;
        }

        if (!enterCollision)
        {
            Debug.Log("stop_scaling");
            player.ActivateScaling(false);
            return;
        }

        switch (playerEvent)
        {
            case PlayerEvent.Reduce:
                Debug.Log("reduce");
                player.ActivateScaling(true, -scaleSpeed * commonMultiplyer);
                //activate points -scaleSpeed * 1000;
                break;
            case PlayerEvent.Increase:
                Debug.Log("increase");
                player.ActivateScaling(true, scaleSpeed * commonMultiplyer);
                //
                break;
            case PlayerEvent.Boost:
                Debug.Log("boost");
                player.ActivateScaling(true, scaleSpeed * boostMultiplyer * commonMultiplyer);
                //
                break;
            default: throw new System.NotSupportedException();
        }
    }
    #endregion
}
