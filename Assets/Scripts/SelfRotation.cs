using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationDelta;

    private Transform localTransform;
    private Vector3 currentRotation;

    private void OnEnable()
    {
        GlobalEventManager.RotationSpeedEvent += UpdateRotation;
        GlobalEventManager.GameStateEvent += ResetRotation;
    }

    private void Start()
    {
        localTransform = transform;
        currentRotation = rotationDelta;
    }

    private void Update()
    {
        localTransform.Rotate(currentRotation * Time.deltaTime);
    }

    private void OnDisable()
    {
        GlobalEventManager.RotationSpeedEvent -= UpdateRotation;
        GlobalEventManager.GameStateEvent -= ResetRotation;
        ResetRotation(true);
    }

    private void UpdateRotation(float delta)
    {
        currentRotation =
            new Vector3(currentRotation.x, currentRotation.y, currentRotation.z + Mathf.Sign(currentRotation.z) * delta);
    }

    private void ResetRotation(bool reset)
    {
        if (reset)
        {
            currentRotation = rotationDelta;
        }
    }
}
