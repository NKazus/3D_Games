using UnityEngine;
using Zenject;

namespace MEGame.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform startObject;
        [SerializeField] private float movingSpeed;

        private Transform playerTransform;
        private float currentSpeed;

        [Inject] private readonly GameUpdateHandler updateHandler;

        private void OnDisable()
        {
            SwitchMovement(false);
        }

        private void LocalUpdate()
        {
            playerTransform.position += new Vector3(0, 0, -currentSpeed * Time.deltaTime);
        }

        public void Init()
        {
            playerTransform = transform;
            currentSpeed = movingSpeed;
        }

        public void SwitchMovement(bool activate)
        {
            //Debug.Log("player:" + activate);
            if (activate)
            {
                updateHandler.UpdateEvent += LocalUpdate;
            }
            else
            {
                updateHandler.UpdateEvent -= LocalUpdate;
            }
        }

        public void SwitchParent(Transform targetParent)
        {
            playerTransform.parent = targetParent;
        }

        public System.Action<bool> GetMovementCallback()
        {
            return SwitchMovement;
        }

        public void ResetPosition()
        {
            playerTransform.position = startObject.position;
        }

        public void SetSpeed(float value)
        {
            currentSpeed = movingSpeed * value;
        }
    }
}
