using UnityEngine;
using UnityEngine.UI;
using FitTheSize.GameServices;

namespace FitTheSize.Main
{
    public class RunnerScore : MonoBehaviour
    {
        [SerializeField] private Text pathScoreValueUI;
        [SerializeField] private Text scaleScoreValueUI;
        [SerializeField] private float scaleScoreMultiplyer;

        private int pathScoreValue;
        private float scaleScoreValue;

        private float scaleScoreSpeed;

        private GameUpdateHandler updateHandler;
        private System.Action ZeroScoreCallback;

        private void OnDisable()
        {
            ActivateScaling(false);
        }

        private void UpdateScore()
        {
            scaleScoreValue += scaleScoreSpeed * Time.deltaTime;

            if (scaleScoreValue <= 0)
            {
                scaleScoreValue = 0;
                ZeroScoreCallback();
            }
            scaleScoreValueUI.text = ((int)scaleScoreValue).ToString();
        }

        public void SetupScore(System.Action callback, GameUpdateHandler update)
        {
            ZeroScoreCallback = callback;
            updateHandler = update;
        }

        public void UpdatePathScore()
        {
            pathScoreValue++;
            pathScoreValueUI.text = pathScoreValue.ToString();
        }

        public void ResetRunnerScore()
        {
            scaleScoreValue = scaleScoreMultiplyer;
            scaleScoreValueUI.text = ((int)scaleScoreValue).ToString();
            pathScoreValue = 0;
            pathScoreValueUI.text = pathScoreValue.ToString();
        }

        public void ActivateScaling(bool activate, float scaleValue = 0f)
        {
            if (activate)
            {
                scaleScoreSpeed = scaleValue * scaleScoreMultiplyer;
                updateHandler.GlobalUpdateEvent += UpdateScore;
            }
            else
            {
                updateHandler.GlobalUpdateEvent -= UpdateScore;
            }
        }

        public void ForceScaling(bool scaleUp)
        {
            float scaleSign = scaleUp ? 1f : -1f;
            scaleScoreValue += scaleSign * scaleScoreMultiplyer * 0.25f;
            if (scaleScoreValue <= 0)
            {
                scaleScoreValue = 0;
                ZeroScoreCallback();
            }
            scaleScoreValueUI.text = ((int)scaleScoreValue).ToString();
        }

        public int GetPathScore()
        {
            return pathScoreValue;
        }

        public int GetScaleScore()
        {
            return (int)scaleScoreValue;
        }
    }
}
