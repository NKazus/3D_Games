using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitType type;

    private Transform unitTransform;

    private void Awake()
    {
        unitTransform = transform;
    }

    public UnitType GetUnitType()
    {
        return type;
    }
}
