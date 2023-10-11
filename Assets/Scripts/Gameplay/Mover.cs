using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float[] movementStages;

    private Transform ringTransform;

    private int currentStage;

    private void Awake()
    {
        ringTransform = transform;
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

    public void ResetMover()
    {
        currentStage = 0;
        ringTransform.DOLocalMoveY(movementStages[currentStage], 0.5f);
    }
}
