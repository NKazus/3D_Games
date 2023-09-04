using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationState : MonoBehaviour
{
    [SerializeField] private Vector3[] rotations;

    private int arrayLenght;
    private int currentElem;

    private void Awake()
    {
        arrayLenght = rotations.Length;
    }

    public void Init()
    {
        currentElem = 0;
    }

    public Vector3 DoNext()
    {
        if (++currentElem >= arrayLenght)
        {
            currentElem = 0;
        }
        return rotations[currentElem];
    }

    public Vector3 DoPrev()
    {
        if (--currentElem < 0)
        {
            currentElem = arrayLenght - 1;
        }
        return rotations[currentElem];
    }
}
