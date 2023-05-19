using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Image progressBar;

    [SerializeField] private DataHandler data;

    private int scoreCount;
    private float progress;

    private List<string> visitedDots = new List<string>();

    #region MONO
    private void OnEnable()
    {
        scoreCount++;
        scoreUI.text = scoreCount.ToString();
        GlobalEventManager.GameStateEvent += ChangeScoreState;
    }
    private void OnDisable()
    {
        GlobalEventManager.GameStateEvent -= ChangeScoreState;
    }
    #endregion

    private void UpdateScore(string id)
    {
        scoreCount++;
        scoreUI.text = scoreCount.ToString();

        CheckProgress(id);        
    }

    private void ChangeScoreState(bool isActive)
    {
        if (isActive)
        {
            scoreCount = 0;
            scoreUI.text = scoreCount.ToString();
            visitedDots.Clear();
            progress = 0f;
            progressBar.fillAmount = progress;
            GlobalEventManager.ScoreEvent += UpdateScore;
        }
        else
        {
            GlobalEventManager.ScoreEvent -= UpdateScore;
        }
    }

    private void CheckProgress(string id)
    {
        if (visitedDots.Contains(id))
        {
            return;
        }
        visitedDots.Add(id);
        int count = visitedDots.Count;
        int dots = data.Dots;
        progress = (float)count / dots;
        progressBar.fillAmount = progress;

        if (count >= dots)
        {
            data.ResetDots();
            GlobalEventManager.DoWin();
            GlobalEventManager.SwitchGameState(false);
        }
    }
}
