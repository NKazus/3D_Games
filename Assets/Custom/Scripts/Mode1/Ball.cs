using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float initialVelocity;

    private Transform ballTransform;
    private Vector3 ballInitialPos;
    private Vector3 ballInitialScale;
    private Vector3 lastFrameVelocity;
    private Rigidbody ballRigidbody;
    private SphereCollider ballCollider;

    private float currentVelocity;

    private System.Action<bool> CollisionCallback;

    private void OnEnable()
    {
        ballTransform.position = ballInitialPos;
        ballCollider.enabled = true;
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
            Wall targetWall = targetObject.GetComponent<Wall>();
            bool isCrashing = targetWall.IsWallActive();
            if (isCrashing)
            {
                StopBall();
                ballTransform.DOScale(Vector3.zero, 0.4f)
                    .SetId("main")
                    .OnComplete(() => {
                        if (CollisionCallback != null)
                        {
                            CollisionCallback(isCrashing);
                        }
                    });
            }
            else
            {                
                targetWall.Activate();
                Bounce(collision.GetContact(0).normal);
                if (CollisionCallback != null)
                {
                    CollisionCallback(isCrashing);
                }
            }                     
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        Vector3 direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        currentVelocity += 0.01f;

        lastFrameVelocity = ballRigidbody.velocity = direction * currentVelocity;
    }

    private void StopBall()
    {
        ballCollider.enabled = false;
        ballRigidbody.velocity = Vector3.zero;
    }

    public void Init()
    {
        ballTransform = transform;
        ballInitialPos = ballTransform.position;
        ballInitialScale = ballTransform.localScale;
        ballRigidbody = GetComponent<Rigidbody>();
        ballCollider = GetComponent<SphereCollider>();
    }

    public void StartBall()
    {
        currentVelocity = initialVelocity;

        Vector3 randDir = Random.insideUnitSphere;
        randDir.y = 0f;
        randDir = randDir.normalized;

        ballCollider.enabled = true;
        lastFrameVelocity = ballRigidbody.velocity = randDir * currentVelocity;
    }

    public void ResetBall()
    {
        ballTransform.position = ballInitialPos;
        ballTransform.DOScale(ballInitialScale, 0.4f)
            .SetId("main");
    }

    public void SetCollisionCallback(System.Action<bool> callback)
    {
        CollisionCallback = callback;
    }
}
