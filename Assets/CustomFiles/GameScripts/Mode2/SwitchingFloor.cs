using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorCondition
{
    Inactive,
    Warning,
    Active
}

public class SwitchingFloor : MonoBehaviour
{
    [SerializeField] private Material inactiveMat;
    [SerializeField] private Material warningMat;
    [SerializeField] private Material activeMat;

    private BoxCollider floorCollider;
    private MeshRenderer floorRenderer;

    public void Init()
    {
        floorCollider = transform.GetChild(0).GetComponent<BoxCollider>();
        floorRenderer = GetComponent<MeshRenderer>();
    }

    public void SwitchFloor(FloorCondition cond)
    {
        switch (cond)
        {
            case FloorCondition.Inactive: floorRenderer.material = inactiveMat; floorCollider.enabled = false; break;
            case FloorCondition.Warning: floorRenderer.material = warningMat; floorCollider.enabled = false; break;
            case FloorCondition.Active: floorRenderer.material = activeMat; floorCollider.enabled = true; break;
            default: throw new System.NotSupportedException();
        }
    }
}
