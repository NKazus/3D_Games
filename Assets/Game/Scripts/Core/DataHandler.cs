using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private int playerGameScore;
    [SerializeField] private ScoreManager scoreManager;


    public int PlayerGameScore => playerGameScore;

    private void OnEnable()
    {
        playerGameScore = PlayerPrefs.HasKey("_GameScore") ? PlayerPrefs.GetInt("_GameScore") : playerGameScore;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameScore", playerGameScore);
    }


    public void UpdateScore(int value)
    {
        playerGameScore += value;
        if(playerGameScore < 0)
        {
            playerGameScore = 0;
        }
        scoreManager.UpdateValues(playerGameScore);
    }
}
