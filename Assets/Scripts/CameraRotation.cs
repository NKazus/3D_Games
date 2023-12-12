using DG.Tweening;
using UnityEngine;

namespace FitTheSize
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private bool doCamera;
        [SerializeField] private Transform skyboxCamera;
        [SerializeField] private float minCameraAngle;
        [SerializeField] private float maxCameraAngle;
        [SerializeField] private float cameraTime;

        private Quaternion initialCameraAngle;

        private Sequence cameraSequence;

        private void Awake()
        {
            initialCameraAngle = skyboxCamera.transform.rotation;
        }

        private void OnEnable()
        {
            if (doCamera)
            {
                UpdateCamera();
            }
        }

        private void OnDisable()
        {
            cameraSequence.Rewind();
            skyboxCamera.rotation = initialCameraAngle;
        }

        private void UpdateCamera()
        {
            cameraSequence = DOTween.Sequence()
                .Append(skyboxCamera.DORotate(new Vector3(minCameraAngle, 0, 0), cameraTime).SetEase(Ease.Linear))
                .Append(skyboxCamera.DORotate(new Vector3(maxCameraAngle, 0, 0), cameraTime).SetEase(Ease.Linear))
                .SetLoops(-1);
        }
    }
}
