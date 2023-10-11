using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float[] rotationStages;

    private Transform ringTransform;

    private int currentStage;
    private bool isActive;

    private float rotationSpeed;

    [Inject] private UpdateController updateController;

    private void Awake()
    {
        ringTransform = transform;
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
        ringTransform.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0, Space.Self);
    }

    public int GetRotationStagesMax()
    {
        return rotationStages.Length;
    }

    public int GeRotationStagesCurrent()
    {
        return currentStage;
    }

    public int RotateFaster()
    {
        if(currentStage < rotationStages.Length - 1)
        {
            currentStage++;
            rotationSpeed = rotationStages[currentStage];
        }        
        return currentStage;
    }

    public int RotateSlower()
    {
        if(currentStage > 0)
        {
            currentStage--;
            rotationSpeed = rotationStages[currentStage];
        }
        return currentStage;
    }

    public void ResetRotator()
    {
        isActive = false;
        currentStage = 0;
        rotationSpeed = rotationStages[currentStage];
    }
}
