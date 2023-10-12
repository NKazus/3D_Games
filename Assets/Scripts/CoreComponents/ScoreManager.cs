using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text progressUI;
    [SerializeField] private Text triesUI;

    public void UpdateScore(int value)
    {
        progressUI.DOText(value.ToString(), 0.5f);
    }

    public void UpdateTries(int value)
    {
        triesUI.DOText(value.ToString(), 0.5f);
    }
}
