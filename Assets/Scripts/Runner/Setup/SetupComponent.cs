using UnityEngine;
using UnityEngine.UI;
using FitTheSize.GameServices;

namespace FitTheSize.Setup
{
    public class SetupComponent : MonoBehaviour
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

        [SerializeField] private Color disabledColor;

        protected bool statusValue;
        protected int minCashValue;
        protected GameData gameData;
        protected GameEventHandler eventHandler;

        public void InitComponent(GameData data, GameEventHandler events, int cashLimit)
        {
            gameData = data;
            eventHandler = events;
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
                description.color = statusValue ? Color.white : disabledColor;
            }
        }

        protected virtual void DoResource()
        {
            eventHandler.PlaySound(AudioEffect.Setup);
        }
    }
}
