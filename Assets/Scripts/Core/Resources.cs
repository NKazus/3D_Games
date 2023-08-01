using UnityEngine;

public class Resources : MonoBehaviour
{
    [SerializeField] private int playerScore;
    [SerializeField] private int balanceCharges10;
    [SerializeField] private int balanceCharges30;
    [SerializeField] private ScoreManager score;

    public int BalanceCharges10 => balanceCharges10;
    public int BalanceCharges30 => balanceCharges30;
    public int PlayerScore => playerScore;    

    private void OnEnable()
    {
        playerScore = 50;// PlayerPrefs.HasKey("_PlayerScore") ? PlayerPrefs.GetInt("_PlayerScore") : playerScore;
        balanceCharges10 = 1;// PlayerPrefs.HasKey("_BalanceCharges10") ? PlayerPrefs.GetInt("_BalanceCharges10") : balanceCharges10;
        balanceCharges30 = 1;// PlayerPrefs.HasKey("_BalanceCharges30") ? PlayerPrefs.GetInt("_BalanceCharges30") : balanceCharges30;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_PlayerScore", playerScore);
        PlayerPrefs.SetInt("_BalanceCharges10", balanceCharges10);
        PlayerPrefs.SetInt("_BalanceCharges30", balanceCharges30);
    }

    public void UpdateBalanceCharges(bool is10, int charges)
    {
        if (is10)
        {
            balanceCharges10 += charges;
            score.UpdateValues(1, balanceCharges10);
            score.UpdateValues(3, balanceCharges10);
        }
        else
        {
            balanceCharges30 += charges;
            score.UpdateValues(2, balanceCharges30);
            score.UpdateValues(4, balanceCharges30);
        }        
    }

    public void UpdatePlayerScore(int value)
    {
        playerScore += value;
        if(playerScore < 0)
        {
            playerScore = 0;
        }
        score.UpdateValues(0, playerScore);
    }
}
