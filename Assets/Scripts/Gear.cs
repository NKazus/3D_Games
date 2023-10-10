using UnityEngine;
using Zenject;

public class Gear : MonoBehaviour
{
    [SerializeField] private float gearSpeed;

    private Transform gearTransform;
    private Quaternion gearRotation;

    bool isRotating;

    [Inject] private readonly UpdateController updateController;

    private void Awake()
    {
        gearTransform = transform;
        gearRotation = gearTransform.rotation;
    }

    private void OnEnable()
    {
        gearTransform.rotation = gearRotation;
        updateController.UpdateEvent += RotateGear;
        isRotating = true;
    }

    private void OnDisable()
    {
        updateController.UpdateEvent -= RotateGear;
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
            updateController.UpdateEvent -= RotateGear;
        }
        else
        {
            updateController.UpdateEvent += RotateGear;
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
