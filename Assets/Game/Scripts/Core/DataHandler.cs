using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private int globalScore;
    [SerializeField] private int doubleJumps;
    [SerializeField] private int unlocks;

    public int ScoreValue => currentScoreValue;
    private int currentScoreValue;

    public int DoubleJumps => doubleJumps;
    public int Unlocks => unlocks;
    public int GlobalScore => globalScore;

    private void OnEnable()
    {
        globalScore = PlayerPrefs.HasKey("_GlobalScore") ? PlayerPrefs.GetInt("_GlobalScore") : globalScore;
        doubleJumps = PlayerPrefs.HasKey("_DoubleJumps") ? PlayerPrefs.GetInt("_DoubleJumps") : doubleJumps;
        unlocks = PlayerPrefs.HasKey("_Unlocks") ? PlayerPrefs.GetInt("_Unlocks") : unlocks;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GlobalScore", globalScore);
        PlayerPrefs.SetInt("_DoubleJumps", doubleJumps);
        PlayerPrefs.SetInt("_Unlocks", unlocks);
    }

    public void AddBonus(bool isJump)
    {
        if (isJump)
        {
            doubleJumps++;
        }
        else
        {
            unlocks++;
        }
    }

    public void RemoveBonus(bool isJump)
    {
        if (isJump)
        {
            doubleJumps--;
        }
        else
        {
            unlocks--;
        }
    }

    public void UpdateGlobalScore(int value)
    {
        globalScore += value;
        if(globalScore < 0)
        {
            globalScore = 0;
        }
    }

    public void UpdateCurrentScore(bool doReset = false)
    {
        currentScoreValue = doReset ? 0 : ++currentScoreValue;
    }
}
