using DG.Tweening;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    private Transform wandererTransform;
    private Vector3 initialPosition;
    private CustomPath currentPath;
    private int maxSteps;
    private int currentStep;

    private void Awake()
    {
        wandererTransform = transform;
        initialPosition = wandererTransform.position;
    }

    private void OnEnable()
    {
        wandererTransform.position = initialPosition;
    }

    //when follow the loop, go from 1 to n
    //0 - initial position out of screen

    public void SetPath(CustomPath targetPath)
    {
        currentPath = targetPath;
        maxSteps = targetPath.wayPoints.Length;
        wandererTransform.position = targetPath.wayPoints[0].position;
        currentStep = 0;
    }

    public void MovePath()
    {
        currentStep++;
        if(currentStep >= maxSteps)
        {
            currentStep = 1;
        }

        wandererTransform.DOMove(currentPath.wayPoints[currentStep].position, currentPath.movingTime)
            .SetId("wanderer")
            .OnComplete(() => MovePath());
    }
}
