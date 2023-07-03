using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text playerScoreUI;
    [SerializeField] private Text botScoreUI;
    [SerializeField] private Text throwsUI;
    [SerializeField] private Text roundsUI;

    [SerializeField] private Text[] playerCombos;
    [SerializeField] private Text[] botCombos;

    public void UpdateValues(int id, int value)
    {
        Text ui;
        string result = value.ToString();

        switch (id)
        {
            case 1: ui = playerScoreUI; break;
            case 2: ui = botScoreUI; break;
            case 3: ui = throwsUI; result += "/5"; break;
            case 4: ui = roundsUI; result += "/5"; break;
            default: ui = globalScoreUI; break;
        }
        UpdateText(ui, result);
    }

    public void UpdateComboValues(int id, int score, bool player = false)
    {
        Text ui;
        ui = player ? playerCombos[id - 1] : botCombos[id - 1];
        UpdateText(ui, score.ToString());
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f);
    }
}
