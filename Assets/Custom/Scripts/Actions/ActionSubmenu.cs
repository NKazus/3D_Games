using UnityEngine;
using UnityEngine.UI;

namespace CMGame.Gameplay
{
    public class ActionSubmenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuObject;
        [SerializeField] private Button actionButton;

        private Unit activeTarget;

        public void Init(System.Action callback)
        {
            actionButton.onClick.AddListener(() => callback());
        }

        public void ResetMenu()
        {
            activeTarget = null;
            menuObject.SetActive(false);
        }

        public void SwitchMenu(Unit target)
        {
            if (activeTarget == null)
            {
                activeTarget = target;
                menuObject.SetActive(true);
                return;
            }

            if (activeTarget == target)
            {
                ResetMenu();
            }
        }
    }
}
