using UnityEngine;
using UnityEngine.UI;

namespace CMGame.Gameplay
{
    public class ActionSubmenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuObject;
        [SerializeField] private Button actionButton;

        [SerializeField] private Sprite attackSprite;
        [SerializeField] private Sprite defenceSprite;
        [SerializeField] private Sprite buffSprite;

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
                actionButton.image.sprite = target.GetUnitType() switch
                {
                    UnitType.Attack => attackSprite,
                    UnitType.Defence => defenceSprite,
                    UnitType.Buff => buffSprite,
                    _ => throw new System.NotSupportedException()
                };
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
