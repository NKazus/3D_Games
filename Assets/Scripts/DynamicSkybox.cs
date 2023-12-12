using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

namespace FitTheSize
{
    public class DynamicSkybox : MonoBehaviour
    {
        [SerializeField] private bool doExposure;
        [SerializeField] private Material skyboxMat;
        [SerializeField] private float minExposureParam;
        [SerializeField] private float maxExposureParam;
        [SerializeField] private float exposureSpeed;

        private float initialExposure;

        private float currentExposure;

        [Inject] private readonly GameUpdateHandler updateHandler;

        private void Awake()
        {
            initialExposure = skyboxMat.GetFloat("_Exposure");
        }

        private void OnEnable()
        {
            currentExposure = initialExposure;

            if (doExposure)
            {
                updateHandler.GlobalFixedUpdateEvent += UpdateExposure;
            }
        }

        private void OnDisable()
        {
            updateHandler.GlobalFixedUpdateEvent -= UpdateExposure;

            //skyboxMat.SetFloat("_Exposure", initialExposure);
            //skyboxCamera.transform.rotation = initialCameraAngle;
        }

        private void UpdateExposure()
        {
            currentExposure += exposureSpeed * Time.fixedDeltaTime;
            skyboxMat.SetFloat("_Exposure", currentExposure);

            if (currentExposure > maxExposureParam || currentExposure < minExposureParam)
            {
                exposureSpeed = -exposureSpeed;
            }
        }
    }
}
