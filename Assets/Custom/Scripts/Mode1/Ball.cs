using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float initialVelocity;

    private Transform ballTransform;
    private Vector3 ballInitialPos;
    private Vector3 lastFrameVelocity;
    private Rigidbody ballRigidbody;
    private SphereCollider ballCollider;

    private float currentVelocity;

    private System.Action<bool> CollisionCallback;

    [Inject] private readonly GameUpdateHandler updateHandler;

    private void Awake()
    {
        ballTransform = transform;
        ballInitialPos = transform.position;
        ballRigidbody = GetComponent<Rigidbody>();
        ballCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        ballTransform.position = ballInitialPos;
        ballCollider.enabled = true;
    }

    private void OnDisable()
    {
        StopBall();
    }

    private void LocalFixedUpdate()
    {
        lastFrameVelocity = ballRigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject targetObject = collision.gameObject;
        if (targetObject.CompareTag("Wall"))
        {
            Wall targetWall = targetObject.GetComponent<Wall>();
            bool isCrashing = targetWall.IsWallActive();
            if (isCrashing)
            {
                StopBall();
                //crash anim
            }
            else
            {                
                targetWall.Activate();
                Bounce(collision.GetContact(0).normal);
            }

            if (CollisionCallback != null)
            {
                CollisionCallback(isCrashing);
            }            
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        Vector3 direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        currentVelocity += 0.01f;

        ballRigidbody.velocity = direction * currentVelocity;
    }

    private void StopBall()
    {
        ballCollider.enabled = false;
        ballRigidbody.velocity = Vector3.zero;
        updateHandler.GameFixedUpdateEvent -= LocalFixedUpdate;
    }

    public void StartBall()
    {
        currentVelocity = initialVelocity;
        ballTransform.position = ballInitialPos;

        Vector3 randDir = Random.insideUnitSphere.normalized;
        randDir.y = 0f;

        updateHandler.GameFixedUpdateEvent += LocalFixedUpdate;

        ballCollider.enabled = true;
        ballRigidbody.velocity = randDir * currentVelocity;
    }

    public void SetCollisionCallback(System.Action<bool> callback)
    {
        CollisionCallback = callback;
    }
}
