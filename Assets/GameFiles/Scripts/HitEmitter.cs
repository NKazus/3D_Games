using UnityEngine;

public class HitEmitter : MonoBehaviour
{
    private const int EMITTERS_NUMBER = 2;
    private ParticleSystem[] emitters;

    bool isEmitting;

    private void Awake()
    {
        isEmitting = false;
        Transform emitTransform = transform;
        emitters = new ParticleSystem[EMITTERS_NUMBER];
        for(int i = 0; i < EMITTERS_NUMBER; i++)
        {
            emitters[i] = emitTransform.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    private void OnDisable()
    {
        DeactivateEmission();
    }

    public void ActivateEmission()
    {
        for (int i = 0; i < EMITTERS_NUMBER; i++)
        {
            emitters[i].Play();
        }
        isEmitting = true;
    }

    public void DeactivateEmission()
    {
        if (!isEmitting)
        {
            return;
        }

        for (int i = 0; i < EMITTERS_NUMBER; i++)
        {
            emitters[i].Stop();
        }
        isEmitting = false;
    }
}
