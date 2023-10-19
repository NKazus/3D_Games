using UnityEngine;

public class MovingPixie : MovingObject
{
    private ParticleSystem trail;

    protected override void Awake()
    {
        base.Awake();
        trail = objectTransform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public override void ResetObject(System.Action callback)
    {
        trail.Stop();
        base.ResetObject(callback);
        trail.Play();
    }

}
