using UnityEngine;
using FitTheSize.Main;
using FitTheSize.GameServices;
using Zenject;

namespace FitTheSize {
    public class MenuTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerAnimation player;

        [Inject] private readonly GameUpdateHandler updateHandler;

        private void Awake()
        {
            player.SetupPlayerAnimation(updateHandler);
        }

        private void OnEnable()
        {
            player.SwitchRotation(true);
        }

        private void OnDisable()
        {
            player.SwitchRotation(false);
        }
    }
}
