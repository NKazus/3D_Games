using UnityEngine;

using FitTheSize.GameServices;

namespace FitTheSize.Setup
{
    public class UpdateComponent : SetupComponent
    {
        [Header("Update:")]
        [SerializeField] private float upgradeValue;

        protected override void DoResource()
        {
            int currentScore = (int)gameData.GetResourceValue(GameResources.HighScore);
            float price = priceValue / 100f;
            currentScore -= (int)(currentScore * price);

            gameData.UpdateHighScore(currentScore, true);
            gameData.UpdateRes(resType, upgradeValue);
            CheckResource();
        }
    }
}
