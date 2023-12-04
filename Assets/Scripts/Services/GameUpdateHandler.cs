using System;
using UnityEngine;

namespace FitTheSize.GameServices
{
    public class GameUpdateHandler : MonoBehaviour
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
}
