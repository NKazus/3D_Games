using System;
using UnityEngine;

public class GameUpdateHandler : MonoBehaviour
{
    public event Action UpdateEvent;

    private void Update()
    {
        UpdateEvent?.Invoke();
    }
}
