using UnityEngine;
using UnityEngine.UI;
using FitTheSize.GameServices;

namespace FitTheSize.Setup
{
    public abstract class SetupComponent : MonoBehaviour
    {
        [Header("General:")]
        [SerializeField] protected GameResources resType;
        [SerializeField] protected Text priceText;
        [SerializeField] protected int priceValue;
        [SerializeField] protected Button getButton;

        [SerializeField] private bool doText;
        [SerializeField] private Text description;
        [SerializeField] private string enabledText;
        [SerializeField] private string disabledText;

        protected bool statusValue;
        protected int minCashValue;
        protected GameData gameData;

        public void InitComponent(GameData data, int cashLimit)
        {
            gameData = data;
            minCashValue = cashLimit;

            priceText.text = priceValue.ToString() + "%";

            getButton.onClick.AddListener(DoResource);
        }

        public virtual void CheckResource()
        {
            bool statusValue = gameData.GetResourceStatus(resType);
            bool cash = (int)gameData.GetResourceValue(GameResources.HighScore) >= minCashValue;

            priceText.color = cash ? Color.white : Color.red;
            getButton.interactable = (cash && statusValue);

            if (doText)
            {
                description.text = statusValue ? enabledText : disabledText;
                description.color = statusValue ? Color.white : Color.gray;
            }
        }

        protected abstract void DoResource();
    }
}
