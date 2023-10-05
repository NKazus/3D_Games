using System;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public event Action UpdateEvent;

    private void Update()
    {
        UpdateEvent?.Invoke();
    }
}
