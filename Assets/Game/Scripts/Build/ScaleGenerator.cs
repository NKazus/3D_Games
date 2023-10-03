using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScaleGenerator : MonoBehaviour
{
    [SerializeField] private Vector3 defaultScale;
    [SerializeField] private float minDelta;
    [SerializeField] private float maxDelta;

    private Vector3 targetScale;

    [Inject] private readonly RandomGenerator random;

    public void SetScale(Vector3 scaleValue)
    {
        targetScale = scaleValue;
    }

    public int GenerateId(int size)
    {
        return random.GenerateInt(0, size);
    }

    public Vector3 GenerateDefault()
    {
        return Generate(defaultScale);
    }

    public Vector3 GenerateTarget()
    {
        return Generate(targetScale);
    }

    public Vector3 GetTarget()
    {
        return targetScale;
    }

    private Vector3 Generate(Vector3 target)
    {
        return new Vector3(target.x + random.GenerateFloat(minDelta, maxDelta),
            target.y + random.GenerateFloat(minDelta, maxDelta), target.z + random.GenerateFloat(minDelta, maxDelta));
    }
}
