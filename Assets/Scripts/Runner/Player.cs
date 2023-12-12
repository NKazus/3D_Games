using UnityEngine;
using FitTheSize.GameServices;
using DG.Tweening;

namespace FitTheSize.Main
{
    public class Player : MonoBehaviour
    {      
        [Header("Collision tags:")]
        [SerializeField] private string wallTag;
        [SerializeField] private string reduceTag;
        [SerializeField] private string increaseTag;
        [SerializeField] private string boostTag;

        [Header("Swipes:")]
        [Tooltip("Left ---> Right")]
        [SerializeField] private Transform[] swipePositions;

        private Transform playerTransform;
        private Vector3 initialScale;
        private float scaleSpeed;

        private int currentSwipePosition;

        private BoxCollider playerCollider;
        private GameUpdateHandler updateHandler;
        private System.Action<PlayerEvent, bool> CollisionCallback;

        private const string DOTWEEN_KILLABLE = "PLAYER_SCALE_UP_OR_SWIPE";
        private const string DOTWEEN_UNKILLABLE = "PLAYER_SCALE_DOWN";

        private void Awake()
        {
            playerTransform = transform;
            initialScale = playerTransform.localScale;
            playerCollider = GetComponent<BoxCollider>();
        }

        private void OnDisable()
        {
            ActivateScaling(false);
            ActivateCollisions(false);
        }

        private void Scale()
        {
            Vector3 newScale = new Vector3(playerTransform.localScale.x + scaleSpeed * Time.fixedDeltaTime,
                playerTransform.localScale.y + scaleSpeed * Time.fixedDeltaTime,
                playerTransform.localScale.z + scaleSpeed * Time.fixedDeltaTime);
            playerTransform.localScale = newScale;
        }

        #region COLLISIONS
        private void OnTriggerEnter(Collider other)
        {
            if (CollisionCallback == null)
            {
                return;
            }

            if (other.gameObject.CompareTag(wallTag))
            {
                CollisionCallback(PlayerEvent.Wall, true);
            }
            if (other.gameObject.CompareTag(reduceTag))
            {
                CollisionCallback(PlayerEvent.Reduce, true);
            }
            if (other.gameObject.CompareTag(increaseTag))
            {
                CollisionCallback(PlayerEvent.Increase, true);
            }
            if (other.gameObject.CompareTag(boostTag))
            {
                CollisionCallback(PlayerEvent.Boost, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (CollisionCallback == null)
            {
                return;
            }

            if (other.gameObject.CompareTag(reduceTag))
            {
                CollisionCallback(PlayerEvent.Reduce, false);
            }
            if (other.gameObject.CompareTag(increaseTag))
            {
                CollisionCallback(PlayerEvent.Increase, false);
            }
            if (other.gameObject.CompareTag(boostTag))
            {
                CollisionCallback(PlayerEvent.Boost, false);
            }
        }
        #endregion

        public void SetupPlayer(System.Action<PlayerEvent, bool> callback, GameUpdateHandler update)
        {
            Debug.Log("setup player");
            CollisionCallback = callback;
            updateHandler = update;
        }

        public void ActivateScaling(bool activate, float scaleValue = 0f)
        {
            if (activate)
            {
                scaleSpeed = scaleValue;
                updateHandler.GlobalFixedUpdateEvent += Scale;
            }
            else
            {
                updateHandler.GlobalFixedUpdateEvent -= Scale;
            }
        }

        public void ForceScaling(bool scaleUp)
        {
            float scaleSign = scaleUp ? 1f : -1f;
            Vector3 newScale = new Vector3(playerTransform.localScale.x + scaleSign * initialScale.x * 0.25f,
                playerTransform.localScale.y + scaleSign * initialScale.y * 0.25f,
                playerTransform.localScale.z + scaleSign * initialScale.z * 0.25f);

            if (newScale.x < 0)
            {
                newScale = Vector3.zero;
            }
            playerTransform.DOScale(newScale, 0.5f).SetId(scaleUp ? DOTWEEN_KILLABLE : DOTWEEN_UNKILLABLE);
        }

        public void SwipePlayer(SwipeDirection direction)
        {
            if ((direction == SwipeDirection.Left && currentSwipePosition <= 0) ||
                (direction == SwipeDirection.Right && currentSwipePosition >= swipePositions.Length - 1))
            {
                return;
            }

            switch (direction)
            {
                case SwipeDirection.Left: currentSwipePosition--; break;
                case SwipeDirection.Right: currentSwipePosition++; break;
                default: throw new System.NotSupportedException();
            }

            playerTransform.DOMove(swipePositions[currentSwipePosition].position, 0.5f).SetId(DOTWEEN_KILLABLE);
        }

        public void ResetPlayer()
        {
            playerTransform.localScale = initialScale;
            currentSwipePosition = swipePositions.Length / 2;
            playerTransform.position = swipePositions[currentSwipePosition].position;
            //visual movement
        }

        public void StopPlayer()
        {
            DOTween.Kill(DOTWEEN_KILLABLE);
        }

        public void ActivateCollisions(bool activate)
        {
            playerCollider.enabled = activate;
        }
    }
}
