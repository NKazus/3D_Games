using UnityEngine;

public class MovingPlayer : MovingObject
{
    [SerializeField] private AssetSwitch routesAssets;

    public override void ResetObject(System.Action callback)
    {
        base.ResetObject(callback);
        routesAssets.Switch(currentPath);
    }
}
