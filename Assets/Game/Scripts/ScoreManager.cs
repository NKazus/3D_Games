using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text movesUI;
    [SerializeField] private Text jumpsUI;
    [SerializeField] private Text unlocksUI;

    public void UpdateValues(int id, int value)
    {
        Text ui;
        switch (id)
        {
            case 2: ui = movesUI; break;
            case 3: ui = jumpsUI; break;
            case 4: ui = unlocksUI; break;
            default: ui = globalScoreUI; break;
        }
        UpdateText(ui, value.ToString());
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }
}
