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

    [SerializeField] private Text playerExtraUI;
    [SerializeField] private Text botExtraUI;
    [SerializeField] private Text timerUI;

    [SerializeField] private Color activeComboColor;
    [SerializeField] private Color cyanHue;

    public void UpdateValues(int id, int value)
    {
        Text ui;
        string result = value.ToString();

        switch (id)
        {
            case 1: ui = playerScoreUI; break;
            case 2: ui = botScoreUI; break;
            case 3: ui = throwsUI; result += "/5"; throwsUI.color = value < 5 ? activeComboColor : Color.grey; break;
            case 4: ui = roundsUI; result += "/5"; break;
            case 5: ui = playerExtraUI; playerExtraUI.color = value > 0 ? activeComboColor : cyanHue; break;
            case 6: ui = botExtraUI; botExtraUI.color = value > 0 ? activeComboColor : cyanHue; break;
            default: ui = globalScoreUI; break;
        }
        UpdateText(ui, result);
    }

    public void UpdateComboValues(int id, int score, bool player = false)
    {
        Text ui;
        ui = player ? playerCombos[id - 1] : botCombos[id - 1];
        ui.color = score > 0 ? activeComboColor : Color.gray;
        UpdateText(ui, score.ToString());
    }

    public void UpdateTimer(float timeLeft)
    {
        if(timeLeft < 0)
        {
            timeLeft = 0;
        }
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f);
    }
}
