using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text globalScoreUI;
    [SerializeField] private Text timerUI;

    public void UpdateScore(int value)
    {

        globalScoreUI.DOText(value.ToString(), 0.5f);
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

}
