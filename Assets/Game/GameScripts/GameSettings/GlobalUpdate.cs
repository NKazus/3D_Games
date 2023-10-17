using System;
using UnityEngine;

public class GlobalUpdate : MonoBehaviour
{
    public event Action GlobalUpdateEvent;

    private void Update()
    {
        GlobalUpdateEvent?.Invoke();
    }
}
