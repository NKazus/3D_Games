using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private GameUIManager ui;
    [SerializeField] private int gameScore;
    [SerializeField] private int highlights;

    public int GameScore => gameScore;
    public int Highlights => highlights;

    private void OnEnable()
    {
        gameScore = PlayerPrefs.HasKey("_GameScore") ? PlayerPrefs.GetInt("_GameScore") : gameScore;

        highlights = 5;// PlayerPrefs.HasKey("_Highlights") ? PlayerPrefs.GetInt("_Highlights") : highlights;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameScore", gameScore);

        PlayerPrefs.SetInt("_Highlights", highlights);
    }

    public void UpdateGlobalScore(int value)
    {
        gameScore += value;
        if(gameScore < 0)
        {
            gameScore = 0;
        }
        ui.UpdateValues(0, gameScore);
    }

    public void UpdateHighlights(int value)
    {
        highlights += value;
        if (highlights < 0)
        {
            highlights = 0;
        }
        ui.UpdateValues(1, highlights);
    }
}
