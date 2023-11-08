using UnityEngine;
using Zenject;
using MEGame.Interactions;

public enum PlayerID
{
    Player1,
    Player2
}

namespace MEGame.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerID playerID;
        [SerializeField] private string targetTraceTag;
        [SerializeField] private string targetFinishTag;
        [SerializeField] private Transform defaultParent;

        private BoxCollider playerCollider;
        private PlayerMovement movement;
        private PlayerMesh mesh;

        [Inject] private readonly GameInput input;
        [Inject] private readonly GameGlobalEvents globalEvents;

        private void Awake()
        {
            playerCollider = GetComponent<BoxCollider>();
            movement = GetComponent<PlayerMovement>();
            movement.Init();

            mesh = transform.GetChild(0).GetComponent<PlayerMesh>();
        }

        private void OnEnable()
        {
            movement.ResetPosition();
        }

        private void Start()
        {
            input.LinkInput(playerID, movement.GetMovementCallback());
        }

        private void OnDisable()
        {
            if (input != null)
            {
                input.DeactivateInput(playerID);
            }
            EnableCollisions(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetTraceTag))
            {
                Debug.Log("input enter:" + playerID);
                input.ActivateInput(playerID);
            }
            if (other.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("collision:" + playerID);
                globalEvents.CollidePlayer(playerID);
            }
            if (other.gameObject.CompareTag("Wanderer"))
            {
                Debug.Log("wander enter:" + playerID);
                movement.SwitchParent(other.gameObject.transform);
            }
            if (other.gameObject.CompareTag(targetFinishTag))
            {
                Debug.Log("win:" + playerID);
                DeactivatePlayer();
                globalEvents.FinishPlayer(playerID);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(targetTraceTag))
            {
                Debug.Log("input exit:" + playerID);
                DeactivatePlayer();
            }
            if (other.gameObject.CompareTag("Wanderer"))
            {
                Debug.Log("wander exit:" + playerID);
                movement.SwitchParent(defaultParent);
            }
        }

        public void EnableCollisions(bool enable)
        {
            Debug.Log("collisions:" + enable + " target:" + playerID);
            playerCollider.enabled = enable;
        }

        public void SwitchMesh(bool isOn, bool restore)
        {
            if (isOn)
            {
                mesh.Show();
            }
            else
            {
                mesh.Hide(restore);
            }
        }

        public void ResetPlayer()
        {
            movement.SwitchParent(defaultParent);
            movement.ResetPosition();
            EnableCollisions(true);
            input.ActivateInput(playerID);
            mesh.Show();
        }

        public void DeactivatePlayer()
        {
            input.DeactivateInput(playerID);
            movement.SwitchMovement(false);
        }

        public void SetMovementSpeed(float modValue)
        {
            movement.SetSpeed(modValue);
        }
    }
}
