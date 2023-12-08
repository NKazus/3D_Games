using UnityEngine;
using UnityEngine.UI;
using FitTheSize.GameServices;

namespace FitTheSize.Setup
{
    public class CountedComponent : UpdateComponent
    {
        [Header("Counted:")]
        [SerializeField] private Text counterText;

        public override void CheckResource()
        {
            CheckResourceCounter();
            base.CheckResource();
        }

        protected override void DoResource()
        {
            base.DoResource();
            CheckResourceCounter();
        }

        private void CheckResourceCounter()
        {
            int forceScaleUses = (int)gameData.GetResourceValue(GameResources.ForceScale);
            counterText.text = forceScaleUses.ToString();
        }
    }
}
