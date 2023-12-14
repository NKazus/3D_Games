using UnityEngine;
using FitTheSize.Main;
using FitTheSize.GameServices;
using Zenject;

namespace FitTheSize {
    public class MenuTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerAnimation[] player;

        [Inject] private readonly GameUpdateHandler updateHandler;

        private void Awake()
        {
            for(int i = 0; i < player.Length; i++)
            {
                player[i].SetupPlayerAnimation(updateHandler);
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < player.Length; i++)
            {
                player[i].SwitchRotation(true);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < player.Length; i++)
            {
                player[i].SwitchRotation(false);
            }
        }
    }
}
