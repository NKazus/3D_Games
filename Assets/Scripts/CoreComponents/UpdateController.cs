using System;
using UnityEngine;

public class UpdateController : MonoBehaviour
{
    public event Action UpdateEvent;

    private void Update()
    {
        UpdateEvent?.Invoke();
    }
}
