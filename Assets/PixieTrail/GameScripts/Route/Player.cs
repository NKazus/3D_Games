using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [Inject] private readonly InGameEvents events;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pixie"))
        {
            events.CheckCollision();
        }
    }
}
