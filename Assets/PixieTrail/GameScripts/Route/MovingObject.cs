using UnityEngine;
using Zenject;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private BezierPath[] paths;
    [SerializeField] private float movementSpeed;

    private Transform objectTransform;

    private int currentPath;

    [Inject] private readonly RandomValueProvider rand;

    private void Awake()
    {
        objectTransform = transform;
    }

    public void ResetObject(System.Action callback)
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

    public void UpdateSpeed(float modifyer)
    {
        paths[currentPath].SetSpeed(movementSpeed * modifyer);
    }

    public void Stop()
    {
        paths[currentPath].UnfollowPath();
    }
}
