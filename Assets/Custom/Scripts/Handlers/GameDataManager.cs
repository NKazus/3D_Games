using UnityEngine;

public enum DataType
{
    Switches,
    Adds,
    Points
}

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameScoreManager scoreManager;
    [SerializeField] private int globalScore;
    [SerializeField] private int doubleJumps;
    [SerializeField] private int unlocks;

    [SerializeField] private int score;
    [SerializeField] private int switches;
    [SerializeField] private int adds;

    public int ScoreValue => currentScoreValue;
    private int currentScoreValue;

    public int DoubleJumps => doubleJumps;
    public int Unlocks => unlocks;
    public int GlobalScore => globalScore;

    private void OnEnable()
    {
        //globalScore = PlayerPrefs.HasKey("_GlobalScore") ? PlayerPrefs.GetInt("_GlobalScore") : globalScore;

        score = 10;
        switches = 3;
        adds = 5;
    }

    private void OnDisable()
    {
        //PlayerPrefs.SetInt("_GlobalScore", globalScore);
    }

    public void UpdateData(DataType type, int value)
    {
        int targetValue;
        switch (type)
        {
            case DataType.Points: score += value; targetValue = score; break;
            case DataType.Switches: switches += value; targetValue = switches; break;
            case DataType.Adds: adds += value; targetValue = adds; break;
            default: throw new System.NotSupportedException();
        }
        scoreManager.UpdateValues(type, targetValue);
    }

    public int GetData(DataType type)
    {
        return type switch
        {
            DataType.Points => score,
            DataType.Switches => switches,
            DataType.Adds => adds,
            _ => throw new System.NotSupportedException()
        };
    }

    public void RefreshData()
    {
        UpdateData(DataType.Points, 0);
        UpdateData(DataType.Switches, 0);
        UpdateData(DataType.Adds, 0);
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
