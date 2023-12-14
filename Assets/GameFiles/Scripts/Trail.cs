using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private ParticleSystem explosion;

    private void OnDisable()
    {
        trail.Stop();
        explosion.Stop();
    }

    public void DoTrail(bool active)
    {
        if (active)
        {
            trail.Play();
        }
        else
        {
            trail.Stop();
        }
    }

    public void DoExplosion()
    {
        explosion.Stop();
        explosion.Play();
    }

}
