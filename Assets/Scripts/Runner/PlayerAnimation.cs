using DG.Tweening;
using UnityEngine;
using FitTheSize.GameServices;

namespace FitTheSize.Main
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private float maxY;
        [SerializeField] private float minY;
        [SerializeField] private float time;

        [SerializeField] private float rotationSpeed;

        private Transform playerTransform;

        private Sequence shakingSequence;

        private GameUpdateHandler updateHandler;

        private void Awake()
        {
            playerTransform = transform;
        }

        private void OnEnable()
        {
            shakingSequence = DOTween.Sequence()
                .Append(playerTransform.DOLocalMoveY(maxY, time).SetEase(Ease.Linear))
                .Append(playerTransform.DOLocalMoveY(minY, time).SetEase(Ease.Linear))
                
                .SetLoops(-1);
        }

        private void OnDisable()
        {
            shakingSequence.Rewind();
        }

        private void RotatePlayer()
        {
            playerTransform.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0);
        }

        public void SwitchRotation(bool active)
        {
            if (active)
            {
                updateHandler.GlobalFixedUpdateEvent += RotatePlayer;
            }
            else
            {
                updateHandler.GlobalFixedUpdateEvent -= RotatePlayer;
            }
        }

        public void SetupPlayerAnimation(GameUpdateHandler update)
        {
            updateHandler = update;
        }
    }
}
