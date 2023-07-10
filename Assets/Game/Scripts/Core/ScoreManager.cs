using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;

    private void OnDisable()
    {
        DOTween.KillAll();
    }

    public void UpdateValues(int value)
    {
        UpdateText(scoreUI, value.ToString());
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f);
    }
}
