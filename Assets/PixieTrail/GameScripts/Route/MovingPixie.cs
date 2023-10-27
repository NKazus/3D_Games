using UnityEngine;

public class MovingPixie : MovingObject
{
    private ParticleSystem trail;
    private Vector3 initialPosition;

    protected override void Awake()
    {
        base.Awake();
        trail = objectTransform.GetChild(0).GetComponent<ParticleSystem>();
        initialPosition = objectTransform.position;
    }

    private void OnEnable()
    {
        objectTransform.position = initialPosition;
    }

    public override void ResetObject(System.Action callback)
    {
        trail.Stop();
        base.ResetObject(callback);
        trail.Play();
    }

}
