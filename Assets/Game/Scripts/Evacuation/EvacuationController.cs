using DG.Tweening;
using UnityEngine;
using Zenject;

public class EvacuationController : MonoBehaviour
{
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private ObjectsBond bond;
    [SerializeField] private WandererSystem wanderers;

    [Header("Effects:")]
    [SerializeField] private float grabSpeedBoosted;
    [SerializeField] private float bondLenghtBoosted;

    private int finished;
    private int bondCharges;

    [Inject] private readonly GameResourceHandler resources;
    [Inject] private readonly GameGlobalEvents globalEvents;

    private void OnEnable()
    {
        resources.ChangePointsValue(0);
        resources.RefreshBonuses();

        globalEvents.EvacuationEvent += StartEvacuation;
    }

    private void OnDisable()
    {
        globalEvents.EvacuationEvent -= StartEvacuation;

        wanderers.StopWanderers();
        DOTween.Kill("player");
    }

    private void StartEvacuation(bool activate)
    {
        if (activate)
        {
            player1.ResetPlayer();
            player2.ResetPlayer();

            float currentGrabSpeed = resources.GrabSpeed ? 1.5f : 1f;
            Debug.Log("speed:"+currentGrabSpeed);
            player1.SetMovementSpeed(currentGrabSpeed);
            player2.SetMovementSpeed(currentGrabSpeed);
            bond.UpdateBondLength(resources.BondLength ? 0.1f : 0f);
            bondCharges = resources.LinkCharges;
            Debug.Log("links:" + bondCharges);

            wanderers.SetWanderersPath();
            wanderers.MoveWanderers();

            finished = 0;

            globalEvents.FinishEvent += Complete;
            globalEvents.CollisionEvent += Collide;
        }
        else
        {
            globalEvents.FinishEvent -= Complete;
            globalEvents.CollisionEvent -= Collide;

            wanderers.StopWanderers();            

            player1.DeactivatePlayer();
            player2.DeactivatePlayer();
        }
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
        if (finished >= 2)
        {
            Debug.Log("WIN");
            resources.RefreshBonuses(true);
            globalEvents.FinishGame(FinishCondition.Finish);
            globalEvents.SwitchGame(false);
        }
    }

    private void Collide(PlayerID id)
    {
        Debug.Log("collide");
        bondCharges--;
        resources.ChangeBonusValue(ResourceType.Link, false, -1);

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
