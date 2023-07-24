using UnityEngine;
using Zenject;

public class Gear : MonoBehaviour
{
    [SerializeField] private float gearSpeed;

    private Transform gearTransform;
    private Quaternion gearRotation;

    bool isRotating;

    [Inject] private readonly EventManager eventManager;
    [Inject] private readonly UpdateManager updateManager;

    private void Awake()
    {
        gearTransform = transform;
        gearRotation = gearTransform.rotation;
    }

    private void OnEnable()
    {
        gearTransform.rotation = gearRotation;
        updateManager.UpdateEvent += RotateGear;
        isRotating = true;
    }

    private void OnDisable()
    {
        updateManager.UpdateEvent -= RotateGear;
        isRotating = false;
    }

    private void RotateGear()
    {
        gearTransform.Rotate(new Vector3(0, 0, gearSpeed * Time.deltaTime));
    }

    private void UpdateGear()
    {
        if (isRotating)
        {
            updateManager.UpdateEvent -= RotateGear;
        }
        else
        {
            updateManager.UpdateEvent += RotateGear;
        }

        isRotating = !isRotating;
    }

    public void ActivateGear(bool activate)
    {
        if (activate == !isRotating)
        {
            UpdateGear();
        }
    }
}
