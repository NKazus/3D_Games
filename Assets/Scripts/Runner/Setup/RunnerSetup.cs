using UnityEngine;
using FitTheSize.GameServices;
using Zenject;

namespace FitTheSize.Setup
{
    public class RunnerSetup : MonoBehaviour
    {
        [SerializeField] private SetupComponent routeSpeed;
        [SerializeField] private SetupComponent scaleSpeed;
        [SerializeField] private SetupComponent boost;
        [SerializeField] private SetupComponent forceScale;

        [SerializeField] private int minScore = 20;

        [Inject] private readonly GameData gameData;
        [Inject] private readonly GameEventHandler eventHandler;

        private void Awake()
        {
            routeSpeed.InitComponent(gameData, eventHandler, minScore);
            scaleSpeed.InitComponent(gameData, eventHandler, minScore);
            boost.InitComponent(gameData, eventHandler, minScore);
            forceScale.InitComponent(gameData, eventHandler, minScore);
        }

        private void OnEnable()
        {
            gameData.RefreshHighScore();
            CheckBonuses();            
        }

        private void CheckBonuses()
        {
            routeSpeed.CheckResource();
            scaleSpeed.CheckResource();
            boost.CheckResource();
            forceScale.CheckResource();
        }
    }
}
