using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text highlightsUI;

    public void UpdateValues(int id, int value)
    {
        Text ui;
        string result = value.ToString();
        switch (id)
        {
            case 1: ui = highlightsUI; break;
            default: ui = globalScoreUI; break;
        }

        UpdateText(ui, result);
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }
}
