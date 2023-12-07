using UnityEngine;
using UnityEngine.UI;

namespace FitTheSize.Main
{
    public class ForceScale : MonoBehaviour
    {
        [SerializeField] private Button scaleUpButton;
        [SerializeField] private Button scaleDownButton;
        [SerializeField] private Text scaleUsesUI;

        private bool isInteractable;

        private int scaleUseCounter;
        private System.Action<bool> ForceScaleCallback;

        private void Awake()
        {
            scaleUpButton.onClick.AddListener(() => ForcePlayerScale(true));
            scaleDownButton.onClick.AddListener(() => ForcePlayerScale(false));
            isInteractable = true;
        }

        private void ForcePlayerScale(bool upScale)
        {
            if (ForceScaleCallback == null)
            {
                return;
            }

            ForceScaleCallback(upScale);

            scaleUseCounter--;
            scaleUsesUI.text = scaleUseCounter.ToString();
            if (scaleUseCounter <= 0)
            {
                scaleDownButton.interactable = false;
                scaleUpButton.interactable = false;
                isInteractable = false;
            }
        }

        public void SetupForceScale(System.Action<bool> callback)
        {
            ForceScaleCallback = callback;
        }

        public void ResetForceScale(int uses)
        {
            scaleUseCounter = uses;
            scaleUsesUI.text = scaleUseCounter.ToString();

            if (!isInteractable)
            {
                scaleDownButton.interactable = true;
                scaleUpButton.interactable = true;
                isInteractable = false;
            }
        }
    }
}
