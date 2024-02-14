using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text switchesUI;
    [SerializeField] private Text addsUI;

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f).Play();
    }

    public void UpdateValues(DataType type, int value)
    {
        Text ui;
        switch (type)
        {
            case DataType.Points: ui = scoreUI; break;
            case DataType.Switches: ui = switchesUI; break;
            case DataType.Adds: ui = addsUI; break;
            default: throw new System.NotSupportedException();
        }

        UpdateText(ui, value.ToString());
    }
}
