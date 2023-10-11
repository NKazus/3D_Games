using System;
using UnityEngine;

public class UpdateController : MonoBehaviour
{
    public event Action UpdateEvent;
    public event Action FixedUpdateEvent;

    private void Update()
    {
        UpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        FixedUpdateEvent?.Invoke();
    }
}
