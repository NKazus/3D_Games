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

        private void Awake()
        {
            routeSpeed.InitComponent(gameData, minScore);
            scaleSpeed.InitComponent(gameData, minScore);
            boost.InitComponent(gameData, minScore);
            forceScale.InitComponent(gameData, minScore);
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
