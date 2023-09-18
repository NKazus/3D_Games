using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text inspirationUI;

    public void UpdateValues(ResourceType type, int value)
    {
        Text ui;
        string result = value.ToString();

        switch (type)
        {
            case ResourceType.Score: ui = scoreUI; break;
            case ResourceType.Inspriration: ui = inspirationUI; break;
            default: throw new NotSupportedException();
        }
        UpdateText(ui, result);
    }

    private void UpdateText(Text uiText, string value)
    {
        uiText.DOText(value, 0.5f);
    }
}
