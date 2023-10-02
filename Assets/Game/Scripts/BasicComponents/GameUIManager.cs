using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text wallsUI;
    [SerializeField] private Text roundsUI;

    public void UpdateResourceValues(GameResource id, int value)
    {
        Text ui;
        switch (id)
        {
            case GameResource.Score: ui = scoreUI; break;
            case GameResource.Walls: ui = wallsUI; break;
            default: throw new System.NotSupportedException();
        }

        UpdateText(ui, value.ToString());
    }

    public void UpdateRoundsValue(int value)
    {
        string result = value.ToString();
        result += "/10";
        UpdateText(roundsUI, result);
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }
}
