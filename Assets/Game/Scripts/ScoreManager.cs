using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text currentScoreUI;
    [SerializeField] private Text dicesUI;
    [SerializeField] private Text magicDicesUI;


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void UpdateValues(int id, int value)
    {
        Text ui;
        string result = value.ToString();

        switch (id)
        {
            case 1: ui = currentScoreUI; break;
            case 2: ui = dicesUI; result += "/3"; break;
            case 3: ui = magicDicesUI; break;
            default: ui = globalScoreUI; break;
        }
        UpdateText(ui, result);
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f);
    }
}
