using FitTheSize.GameServices;

namespace FitTheSize.Setup
{
    public class ResetComponent : SetupComponent
    {
        protected override void DoResource()//route speed
        {
            int currentScore = (int)gameData.GetResourceValue(GameResources.HighScore);
            float price;

            price = priceValue / 100f;
            currentScore -= (int)(currentScore * price);
            gameData.UpdateHighScore(currentScore, true);
            gameData.ResetRouteSpeed();
            CheckResource();
        }
    }
}
