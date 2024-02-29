using UnityEngine;

public class GameUpdateHandler : MonoBehaviour
{
    public event System.Action GameUpdateEvent;
    public event System.Action GameFixedUpdateEvent;

    private void Update()
    {
        GameUpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        GameFixedUpdateEvent?.Invoke();
    }
}
