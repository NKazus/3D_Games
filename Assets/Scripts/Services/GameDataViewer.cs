using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FitTheSize.GameServices
{
    public class GameDataViewer : MonoBehaviour
    {
        [SerializeField] private Text highScoreUI;

        private void UpdateText(Text uiText, string value)
        {
            uiText.DOText(value, 0.5f).Play();
        }

        public void UpdateHighScore(int value)
        {
            UpdateText(highScoreUI, value.ToString());
        }

        public void UpdateResources(GameResources id, int value, bool isGameplay)
        {

        }

    }
}
