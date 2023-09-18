using UnityEngine;

public enum ResourceType
{
    Score,
    Inspriration
}

public class Resources : MonoBehaviour
{
    [SerializeField] private int playerScore = 10;
    [SerializeField] private int inspiration = 1;
    [SerializeField] private ScoreManager score;

    public int Inspiration => inspiration;
    public int PlayerScore => playerScore;    

    private void OnEnable()
    {
        playerScore = PlayerPrefs.HasKey("_PlayerScore") ? PlayerPrefs.GetInt("_PlayerScore") : playerScore;
        inspiration = PlayerPrefs.HasKey("_Inspiration") ? PlayerPrefs.GetInt("_Inspiration") : inspiration;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_PlayerScore", playerScore);
        PlayerPrefs.SetInt("_Inspiration", inspiration);
    }

    public void UpdateInspiration(int value)
    {
        inspiration += value;
        if (inspiration < 0)
        {
            inspiration = 0;
        }
        score.UpdateValues(ResourceType.Inspriration, inspiration);
    }

    public void UpdatePlayerScore(int value)
    {
        playerScore += value;
        if(playerScore < 0)
        {
            playerScore = 0;
        }
        score.UpdateValues(ResourceType.Score, playerScore);
    }
}
