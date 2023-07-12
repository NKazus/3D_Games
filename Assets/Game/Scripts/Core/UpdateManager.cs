using System;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static event Action GlobalUpdateEvent;

    void Update()
    {
        GlobalUpdateEvent?.Invoke();
    }
}
