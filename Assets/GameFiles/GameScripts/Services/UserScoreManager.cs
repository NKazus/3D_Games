using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UserScoreManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text scoopsUI;
    [SerializeField] private Text shovelsUI;
    [SerializeField] private Text insightUI;

    [SerializeField] private Text tradedShovelsUI;
    [SerializeField] private Text tradedInsightUI;

    public void UpdateValues(int id, int value)
    {
        Text ui;
        switch (id)
        {
            case 1: ui = scoopsUI; break;
            case 2: ui = shovelsUI; break;
            case 3: ui = insightUI; break;
            case 4: ui = tradedShovelsUI; break;
            case 5: ui = tradedInsightUI; break;
            default: ui = globalScoreUI; break;
        }

        UpdateText(ui, value.ToString());
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.3f);
    }
}
