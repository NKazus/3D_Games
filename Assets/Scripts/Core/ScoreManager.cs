using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text charges10UI;
    [SerializeField] private Text charges30UI;

    public void UpdateValues(int id, int value)
    {
        Text ui;
        string result = value.ToString();

        switch (id)
        {
            case 1: ui = charges10UI; break;
            case 2: ui = charges30UI; break;
            default: ui = scoreUI; break;
        }
        UpdateText(ui, result);
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f);
    }
}
