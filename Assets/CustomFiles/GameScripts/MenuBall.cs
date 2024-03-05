using UnityEngine;

public class MenuBall : MonoBehaviour
{
    [SerializeField] private float initialVelocity;

    private Transform ballTransform;
    private Vector3 ballInitialPos;
    private Vector3 lastFrameVelocity;
    private Rigidbody ballRigidbody;
    private SphereCollider ballCollider;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        StartBall();
    }

    private void OnDisable()
    {
        StopBall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject targetObject = collision.gameObject;
        if (targetObject.CompareTag("Wall"))
        {           
            Bounce(collision.GetContact(0).normal);
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        Vector3 direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        lastFrameVelocity = ballRigidbody.velocity = direction * initialVelocity;
    }

    private void StopBall()
    {
        ballCollider.enabled = false;
        ballRigidbody.velocity = Vector3.zero;
    }

    public void Init()
    {
        ballTransform = transform;
        ballInitialPos = transform.position;
        ballRigidbody = GetComponent<Rigidbody>();
        ballCollider = GetComponent<SphereCollider>();
    }

    public void StartBall()
    {
        ballTransform.position = ballInitialPos;

        Vector3 randDir = Random.insideUnitSphere;
        randDir.y = 0f;
        randDir = randDir.normalized;

        ballCollider.enabled = true;
        lastFrameVelocity = ballRigidbody.velocity = randDir * initialVelocity;
    }
}
