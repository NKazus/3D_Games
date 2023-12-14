using System;
using UnityEngine;

public class GlobalUpdateManager : MonoBehaviour
{
    public event Action GlobalFixedUpdateEvent;
    public event Action GlobalUpdateEvent;

    private void FixedUpdate()
    {
        GlobalFixedUpdateEvent?.Invoke();
    }

    private void Update()
    {
        GlobalUpdateEvent?.Invoke();
    }
}
