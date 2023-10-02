using UnityEngine;

public class BlobEmission : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] effects;

    public void StartEmitting()
    {
        for(int i = 0; i < effects.Length; i++)
        {
            effects[i].Play();
        }
    }

    public void StopEmitting()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Stop();
        }
    }
}
