using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EvacuationController : MonoBehaviour
{
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private WandererSystem wanderers;

    private int finished;
    private int bondCharges;

    [Inject] private readonly GameResourceHandler resources;
    [Inject] private readonly GameGlobalEvents globalEvents;
    [Inject] private readonly RandomProvider random;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        //resources.UpdateTreasure(0);
        //resources.UpdateSticks(0);
        globalEvents.EvacuationEvent += StartEvacuation;
    }

    private void OnDisable()
    {
        globalEvents.EvacuationEvent -= StartEvacuation;

        wanderers.StopWanderers();
        //kill all tweens for players, wanderers and obstacles
    }

    private void Complete(PlayerID id)
    {
        switch (id)
        {
            case PlayerID.Player1:
                player1.EnableCollisions(false);
                break;
            case PlayerID.Player2:
                player2.EnableCollisions(false);
                break;
            default: throw new System.NotSupportedException();
        }
        finished++;
        if(finished >= 2)
        {
            Debug.Log("WIN");
            globalEvents.FinishGame(FinishCondition.Finish);
            globalEvents.SwitchGame(false);
        }
    }

    private void StartEvacuation(bool activate)
    {
        if (activate)
        {
            player1.ResetPlayer();
            player2.ResetPlayer();

            wanderers.StopWanderers();
            wanderers.SetWanderersPath();
            wanderers.MoveWanderers();

            finished = 0;
            bondCharges = 2;

            globalEvents.FinishEvent += Complete;
            globalEvents.CollisionEvent += Collide;
        }
        else
        {
            globalEvents.FinishEvent -= Complete;
            globalEvents.CollisionEvent -= Collide;

            player1.DeactivatePlayer();
            player2.DeactivatePlayer();
        }
    }

    private void Collide(PlayerID id)
    {
        Debug.Log("collide");
        bondCharges--;
        //ui and res
        bool restore = bondCharges > 0;
        switch (id)
        {
            case PlayerID.Player1:
                player1.SwitchMesh(false, restore);
                break;
            case PlayerID.Player2:
                player2.SwitchMesh(false, restore);
                break;
            default: throw new System.NotSupportedException();
        }

        if (!restore)
        {
            Debug.Log("collision_death");
            globalEvents.FinishGame(FinishCondition.Collision);
            globalEvents.SwitchGame(false);
        }        
    }
}
