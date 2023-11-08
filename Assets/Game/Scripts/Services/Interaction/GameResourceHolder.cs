using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MEGame.Interactions
{
    public class GameResourceHolder : MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Text linksText;

        [SerializeField] private Image bondImage;
        [SerializeField] private Image speedImage;
        [SerializeField] private Image linksImage;

        private void DoText(Text uiText, string value)
        {
            uiText.DOText(value, 0.5f).Play();
        }

        public void UpdateText(ResourceType id, int value)
        {
            Text ui;
            switch (id)
            {
                case ResourceType.Link: ui = linksText; break;
                case ResourceType.Points: ui = pointsText; break;
                default: throw new System.NotSupportedException();
            }

            DoText(ui, value.ToString());
        }

        public void UpdateImages(ResourceType id, bool active)
        {
            Image target;
            switch (id)
            {
                case ResourceType.Bond: target = bondImage; break;
                case ResourceType.Speed: target = speedImage; break;
                case ResourceType.Link: target = linksImage; break;
                default: throw new System.NotSupportedException();
            }
            target.DOFade(active ? 1f : 0.5f, 0.4f);
        }
    }
}
