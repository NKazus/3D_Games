using UnityEngine;

public class DotRotationHandler : MonoBehaviour
{
    [SerializeField] private float deltaSpeed = 20f;

    private string dotId;
    private Transform localTransform;
    private Vector3 initialDotPosition;

    private void Awake()
    {
        localTransform = transform;
        initialDotPosition = localTransform.position;

        dotId = gameObject.name;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GlobalEventManager.UpdateRotation(deltaSpeed);
            GlobalEventManager.ChangeScore(dotId);
        }
    }
}
