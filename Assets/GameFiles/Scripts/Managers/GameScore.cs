using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    [SerializeField] private Text scoreUI;

    [SerializeField] private Text locksUI;
    [SerializeField] private Text spicesUI;
    [SerializeField] private Text checksUI;

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f);
    }

    public void UpdateResUI(ResData type, int value)
    {
        Text ui;
        string result = value.ToString();

        switch (type)
        {
            case ResData.Points: ui = scoreUI; break;
            case ResData.Locks: ui = locksUI; break;
            case ResData.Spices: ui = spicesUI; break;
            case ResData.Checks: ui = checksUI; break;
            default: throw new System.NotSupportedException();
        }
        UpdateText(ui, result);
    }

    public void UpdateSessionUI()
    {

    }
}
