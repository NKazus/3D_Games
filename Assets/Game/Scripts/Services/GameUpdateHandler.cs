using UnityEngine;

public class GameUpdateHandler : MonoBehaviour
{
    public event System.Action UpdateEvent;

    private void Update()
    {
        UpdateEvent?.Invoke();
    }
}
