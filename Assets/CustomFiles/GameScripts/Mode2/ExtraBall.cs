using DG.Tweening;
using UnityEngine;

public class ExtraBall : MonoBehaviour
{
    [SerializeField] private float initialVelocity;

    private Transform ballTransform;
    private Vector3 ballInitialPos;
    private Vector3 ballInitialScale;
    private Vector3 lastFrameVelocity;
    private Rigidbody ballRigidbody;
    private SphereCollider ballCollider;

    private float currentVelocity;

    private System.Action CollisionCallback;
    private System.Action CrashCallback;

    private void OnDisable()
    {
        StopBall();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Bounce(collision.GetContact(0).normal);
            if (CollisionCallback != null)
            {
                CollisionCallback();
            }                  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            StopBall();
            ballTransform.DOScale(Vector3.zero, 0.4f)
            .SetId("extra")
            .OnComplete(() => {
                if (CrashCallback != null)
                {
                    CrashCallback();
                }
            });            
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
        Vector3 initDir = new Vector3(0, 0, 1f);

        ballCollider.enabled = true;
        lastFrameVelocity = ballRigidbody.velocity = initDir * currentVelocity;
    }

    public void ResetBall()
    {
        ballTransform.position = ballInitialPos;
        ballTransform.DOScale(ballInitialScale, 0.4f)
            .SetId("extra");
    }

    public void SetCollisionCallback(System.Action callback)
    {
        CollisionCallback = callback;
    }

    public void SetCrashCallback(System.Action callback)
    {
        CrashCallback = callback;
    }
}
