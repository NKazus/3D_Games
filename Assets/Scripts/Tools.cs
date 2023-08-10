using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    [SerializeField] private Garden target;

    public GardenState DoWater(GardenState state)
    {
        GardenState newState;
        switch (state)
        {
            case GardenState.Normal:
            case GardenState.Rain: newState = GardenState.Rain; target.UpdateRootPlane(newState); break;
            case GardenState.Heat: newState = GardenState.Normal; target.UpdateRootPlane(newState); break;
            default: throw new NotSupportedException();
        }
        return newState;
    }

    public GardenState DoLoosening(GardenState state)
    {
        GardenState newState;
        switch (state)
        {
            case GardenState.Normal:
            case GardenState.Heat: newState = GardenState.Heat; target.UpdateRootPlane(newState); break;
            case GardenState.Rain: newState = GardenState.Normal; target.UpdateRootPlane(newState); break;            
            default: throw new NotSupportedException();
        }
        return newState;
    }

    public void DoProp()
    {
        target.UpdatePropState(true);
    }
}
