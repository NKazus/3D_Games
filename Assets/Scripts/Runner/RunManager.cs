using UnityEngine;
using UnityEngine.UI;
using Zenject;
using FitTheSize.GameServices;
using FitTheSize.Route;

namespace FitTheSize.Main
{
    public class RunManager : MonoBehaviour
    {
        [SerializeField] private PlatformHandler route;
        [SerializeField] private Player player;
        [SerializeField] private PlayerSwipe swipeComponent;
        [SerializeField] private PlayerAnimation animationComponent;
        [SerializeField] private RunnerScore score;
        [SerializeField] private Button startButton;
        [SerializeField] private ForceScale forceScaleComponent;

        [Header("Scaling parameters:")]
        [SerializeField] private float routeSpeedDelta;

        private float scaleSpeed;
        private float boost;

        [Inject] private readonly GameEventHandler eventHandler;
        [Inject] private readonly GameUpdateHandler updateHandler;
        [Inject] private readonly GameData gameData;

        private void Awake()
        {
            startButton.onClick.AddListener(Initialize);
            player.SetupPlayer(HandleCollision, updateHandler);
            animationComponent.SetupPlayerAnimation(updateHandler);
            swipeComponent.SetupSwipe(HandleSwipe, updateHandler);
            score.SetupScore(HandleZeroScale, updateHandler);
            route.SetDespawnCallback(UpdateScore);
            forceScaleComponent.SetupForceScale(HandleForceScale);
        }

        private void OnEnable()
        {
            route.ResetInitialSpawnFlag();
            route.SetRouteSpeed(gameData.GetResourceValue(GameResources.RouteSpeed));
            route.ResetRoute();
            player.ResetPlayer();
            startButton.gameObject.SetActive(true);
            forceScaleComponent.gameObject.SetActive(false);

            gameData.RefreshHighScore();
        }

        private void OnDisable()
        {
            eventHandler.GameStateEvent -= Activate;
        }

        private void Initialize()
        {
            startButton.gameObject.SetActive(false);
            Activate(true);
            eventHandler.GameStateEvent += Activate;
        }

        private void Activate(bool activate)
        {
            if (activate)
            {
                route.SetRouteSpeed(gameData.GetResourceValue(GameResources.RouteSpeed));
                route.ResetRoute();
                player.ResetPlayer();
                player.ActivateCollisions(true);
                animationComponent.SwitchRotation(true);
                score.ResetRunnerScore();
                route.MoveRoute();

                forceScaleComponent.gameObject.SetActive(true);
                forceScaleComponent.ResetForceScale((int)gameData.GetResourceValue(GameResources.ForceScale));

                scaleSpeed = gameData.GetResourceValue(GameResources.ScaleSpeed);
                boost = gameData.GetResourceValue(GameResources.BoostMult);
                //Debug.Log("scale speed:" + scaleSpeed + " boost mult:" + boost + " route speed:" + gameData.GetResourceValue(GameResources.RouteSpeed));

                swipeComponent.ResetSwipe();
                swipeComponent.ActivateSwiping(true);
            }
            else
            {
                swipeComponent.ActivateSwiping(false);
                forceScaleComponent.gameObject.SetActive(false);

                route.StopRoute();
                player.ActivateCollisions(false);
                player.ActivateScaling(false);
                player.StopPlayer();
                animationComponent.SwitchRotation(false);
                score.ActivateScaling(false);
            }
        }

        private void HandleForceScale(bool upScale)
        {
            player.ForceScaling(upScale);
            score.ForceScaling(upScale);
            eventHandler.PlaySound(AudioEffect.ForceScale);
            gameData.UpdateRes(GameResources.ForceScale, -1);
        }

        private void CalculateScore()
        {
            int path = score.GetPathScore();
            int scale = score.GetScaleScore();
            int currentScore = path * scale;
            //Debug.Log("current score:" + currentScore);

            bool isNewHighScore = gameData.UpdateHighScore(currentScore);
            if (isNewHighScore)
            {
                eventHandler.PlaySound(AudioEffect.HighScore);
                gameData.UpdateRes(GameResources.RouteSpeed, routeSpeedDelta);//when high score beaten speed up route
            }
            else
            {
                eventHandler.PlaySound(AudioEffect.Finish);
                gameData.ResetTempResources();
            }

            eventHandler.DoResult(path, scale, currentScore);
        }

        #region PLAYER
        private void HandleSwipe(SwipeDirection direction)
        {
            player.SwipePlayer(direction);
        }

        private void HandleCollision(PlayerEvent playerEvent, bool enterCollision)
        {
            if (playerEvent == PlayerEvent.Wall)
            {
                //Debug.Log("death");
                CalculateScore();
                eventHandler.SwitchGameState(false);
                return;
            }

            if (!enterCollision)
            {
                //Debug.Log("stop_scaling");
                player.ActivateScaling(false);
                score.ActivateScaling(false);
                return;
            }

            switch (playerEvent)
            {
                case PlayerEvent.Reduce:
                    //Debug.Log("reduce");
                    player.ActivateScaling(true, -scaleSpeed);
                    score.ActivateScaling(true, -scaleSpeed);
                    break;
                case PlayerEvent.Increase:
                    //Debug.Log("increase");
                    player.ActivateScaling(true, scaleSpeed);
                    score.ActivateScaling(true, scaleSpeed);
                    break;
                case PlayerEvent.Boost:
                    //Debug.Log("boost");
                    player.ActivateScaling(true, scaleSpeed * boost);
                    score.ActivateScaling(true, scaleSpeed * boost);
                    break;
                default: throw new System.NotSupportedException();
            }
        }
        #endregion

        #region SCORE
        private void HandleZeroScale()
        {
            //Debug.Log("zero scale");
            CalculateScore();
            eventHandler.SwitchGameState(false);
        }

        private void UpdateScore()
        {
            score.UpdatePathScore();
        }
        #endregion
    }
}
