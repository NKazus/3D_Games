using UnityEngine;
using Zenject;

public class MovingObject : MonoBehaviour
{
    [SerializeField] protected BezierPath[] paths;
    [SerializeField] protected float movementSpeed;

    protected Transform objectTransform;

    protected int currentPath;

    [Inject] protected readonly RandomValueProvider rand;

    protected virtual void Awake()
    {
        objectTransform = transform;
    }

    public virtual void ResetObject(System.Action callback)
    {
        currentPath = rand.GetInt(0, paths.Length);
        paths[currentPath].SetTarget(objectTransform);
        paths[currentPath].SetSpeed(movementSpeed);
        paths[currentPath].SetCallback(callback);
        paths[currentPath].ResetPath();
    }

    public void Move()
    {
        paths[currentPath].FollowPath();
    }

    public void Stop()
    {
        paths[currentPath].UnfollowPath();
    }

    public void UpdateSpeed(float modifyer)
    {
        paths[currentPath].SetSpeed(movementSpeed * modifyer);
    }
}
