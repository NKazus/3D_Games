using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Transform initialParent;
    [SerializeField] private float pushSpeed = 50f;
    [SerializeField] private BoxCollider bounds;
    [SerializeField] private GlobalUpdateManager updateManager;

    private Transform localTransform;
    private Rigidbody playerRB;

    private Vector3 initialPosition;
    private Vector3 direction;

    private void Awake()
    {
        localTransform = transform;
        initialPosition = localTransform.localPosition;

        playerRB = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        localTransform.rotation = Quaternion.identity;
        ResetPlayer();
        GlobalEventManager.GameStateEvent += ChangeState;
    }

    private void OnDisable()
    {
        updateManager.GlobalUpdateEvent -= LocalUpdate;
        updateManager.GlobalFixedUpdateEvent -= LocalFixedUpdate;
        GlobalEventManager.PushPlayerEvent -= Push;
        GlobalEventManager.GameStateEvent -= ChangeState;        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dot"))
        {
            updateManager.GlobalUpdateEvent -= LocalUpdate;
            updateManager.GlobalFixedUpdateEvent -= LocalFixedUpdate;
            localTransform.SetParent(collision.gameObject.transform);
            GlobalEventManager.PushPlayerEvent += Push;
        }            
    }

    private void Push()
    {
        GlobalEventManager.PushPlayerEvent -= Push;
        updateManager.GlobalUpdateEvent += LocalUpdate;
        Vector3 parentPosition = localTransform.parent.position;
        localTransform.SetParent(null);
        direction = (localTransform.position - parentPosition).normalized;
        updateManager.GlobalFixedUpdateEvent += LocalFixedUpdate;
    }

    private void ChangeState(bool isActive)
    {
        if (isActive)
        {
            ResetPlayer();
        }
        else
        {
            GlobalEventManager.PushPlayerEvent -= Push;
        }
    }

    private void ResetPlayer()
    {
        localTransform.SetParent(initialParent);
        localTransform.localPosition = initialPosition;
    }

    private void LocalFixedUpdate()
    {
        playerRB.MovePosition(localTransform.position + direction * pushSpeed * Time.fixedDeltaTime);
    }

    private void LocalUpdate()
    {
        if (!bounds.bounds.Contains(localTransform.position))
        {
            updateManager.GlobalUpdateEvent -= LocalUpdate;
            GlobalEventManager.PlayVibro();
            GlobalEventManager.SwitchGameState(false);
        }
    }
}
