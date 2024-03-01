using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBall : MonoBehaviour
{
    [SerializeField] private float initialVelocity;

    private Transform ballTransform;
    private Vector3 ballInitialPos;
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
        GameObject targetObject = collision.gameObject;
        if (targetObject.CompareTag("Wall"))
        {
            SwitchingWall targetWall = targetObject.GetComponent<SwitchingWall>();
            if (targetWall.IsWallActive())
            {
                Bounce(collision.GetContact(0).normal);
                if (CollisionCallback != null)
                {
                    CollisionCallback();
                }                
            }
            else
            {
                //targetWall.Activate();
                //trigger barrier path animation
                //if not needed -> delete all if else constr
            }            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            StopBall();
            //crash anim
            if (CrashCallback != null)
            {
                CrashCallback();
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
        ballInitialPos = transform.position;
        ballRigidbody = GetComponent<Rigidbody>();
        ballCollider = GetComponent<SphereCollider>();
    }

    public void StartBall()
    {
        currentVelocity = initialVelocity;
        ballTransform.position = ballInitialPos;

        Vector3 initDir = new Vector3(0, 0, 1f);

        ballCollider.enabled = true;
        lastFrameVelocity = ballRigidbody.velocity = initDir * currentVelocity;
    }

    public void ResetBall()
    {
        ballTransform.position = ballInitialPos;
        ballCollider.enabled = true;
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
