using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float[] movementStages;

    private Transform ringTransform;
    private Vector3 ringPosition;

    private int currentStage;

    private void Awake()
    {
        ringTransform = transform;
        ringPosition = ringTransform.position;
    }

    private void OnEnable()
    {
        ringTransform.position = ringPosition;
    }

    public int GetMovementStagesMax()
    {
        return movementStages.Length;
    }

    public int GetMovementStageCurrent()
    {
        return currentStage;
    }

    public int MoveUp()
    {
        if(currentStage < movementStages.Length - 1)
        {
            currentStage++;
            ringTransform.DOLocalMoveY(movementStages[currentStage], 0.5f);
        }
        return currentStage;
    }

    public int MoveDown()
    {
        if (currentStage > 0)
        {
            currentStage--;
            ringTransform.DOLocalMoveY(movementStages[currentStage], 0.5f);
        }
        return currentStage;
    }

    public void ResetMover(bool hard)
    {
        if (hard)
        {
            ringTransform.position = ringPosition;
        }
        currentStage = 0;
        ringTransform.DOLocalMoveY(movementStages[currentStage], 0.5f);
    }
}
