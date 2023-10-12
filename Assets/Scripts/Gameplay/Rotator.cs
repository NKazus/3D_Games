using DG.Tweening;
using UnityEngine;
using Zenject;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float[] rotationStages;

    private Transform ringTransform;
    private Quaternion ringRotation;

    private int currentStage;

    private float rotationSpeed;
    private System.Action<int> rotationCallback;

    [Inject] private UpdateController updateController;

    private void Awake()
    {
        ringTransform = transform;
        ringRotation = transform.rotation;
    }

    private void OnEnable()
    {
        ringTransform.rotation = ringRotation;
        updateController.FixedUpdateEvent += Rotate;
    }

    private void OnDisable()
    {
        updateController.FixedUpdateEvent -= Rotate;
    }

    private void Rotate()
    {
        ringTransform.RotateAround(new Vector3(0, ringTransform.position.y, 0), Vector3.up, rotationSpeed * Time.fixedDeltaTime);
    }

    public void SetCallback(System.Action<int> callback)
    {
        rotationCallback = callback;
    }

    public int GetRotationStagesMax()
    {
        return rotationStages.Length;
    }

    public int GetRotationStagesCurrent()
    {
        return currentStage;
    }

    public void RotateFaster()
    {
        if(currentStage < rotationStages.Length - 1)
        {
            currentStage++;
            DOTween.To(() => rotationSpeed, x => rotationSpeed = x, rotationStages[currentStage], 1f)
                .OnComplete(() => rotationCallback(currentStage));
        }
    }

    public void RotateSlower()
    {
        if(currentStage > 0)
        {
            currentStage--;
            DOTween.To(() => rotationSpeed, x => rotationSpeed = x, rotationStages[currentStage], 1f)
                .OnComplete(() => rotationCallback(currentStage));
        }
        else
        {
            rotationCallback(currentStage);
        }
    }

    public void ResetRotator(bool hard)
    {
        currentStage = 0;
        rotationSpeed = rotationStages[currentStage];
        if (hard)
        {
            ringTransform.rotation = ringRotation;
        }
    }
}
