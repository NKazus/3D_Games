using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private Transform coinTransform;

    [Inject] private UpdateController updateController;

    private void Awake()
    {
        coinTransform = transform;
    }

    private void OnEnable()
    {
        updateController.FixedUpdateEvent += Rotate;
    }

    private void OnDisable()
    {
        updateController.FixedUpdateEvent -= Rotate;
    }

    private void Rotate()
    {
        coinTransform.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0, Space.Self);
    }
}
