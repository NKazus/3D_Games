using UnityEngine;

public enum ResData
{
    Points,
    Locks,
    Spices,
    Checks
} 

public class GameData : MonoBehaviour
{
    [SerializeField] private int gameScore = 0;
    [SerializeField] private int locks = 5;
    [SerializeField] private int spices = 5;
    [SerializeField] private int checks = 5;

    [SerializeField] private GameScore score;

    public System.DateTime rewardDate;

    public int Spices => spices;
    public int Locks => locks;
    public int Checks => checks;
    public int GameScore => gameScore;

    public System.DateTime RewardDate => rewardDate;

    private void OnEnable()
    {
        gameScore = PlayerPrefs.HasKey("DATA_INT_Score") ? PlayerPrefs.GetInt("DATA_INT_Score") : gameScore;
        locks = 10;// PlayerPrefs.HasKey("DATA_INT_Locks") ? PlayerPrefs.GetInt("DATA_INT_Locks") : locks;
        spices = 4;// PlayerPrefs.HasKey("DATA_INT_Spices") ? PlayerPrefs.GetInt("DATA_INT_Spices") : spices;
        checks = 5;// PlayerPrefs.HasKey("DATA_INT_Checks") ? PlayerPrefs.GetInt("DATA_INT_Checks") : checks;

        rewardDate = PlayerPrefs.HasKey("DATA_INT_Reward") ? new System.DateTime(
               System.Convert.ToInt64(PlayerPrefs.GetString("DATA_INT_Reward")))
               .ToLocalTime() : System.DateTime.Now.AddDays(-2);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("DATA_INT_Score", gameScore);
        PlayerPrefs.SetInt("DATA_INT_Locks", locks);
        PlayerPrefs.SetInt("DATA_INT_Spices", spices);
        PlayerPrefs.SetInt("DATA_INT_Checks", checks);

        PlayerPrefs.SetString("DATA_INT_Reward", "" + rewardDate.ToUniversalTime().Ticks);
    }

    public void UpdateResData(ResData type, int changeValue)
    {
        int updateValue;
        switch (type)
        {
            case ResData.Points: updateValue = gameScore += changeValue; break;
            case ResData.Locks: updateValue = locks += changeValue; break;
            case ResData.Spices: updateValue = spices += changeValue; break;
            case ResData.Checks: updateValue = checks += changeValue; break;
            default: throw new System.NotSupportedException();
        }

        Debug.Log(updateValue);
        score.UpdateResUI(type, updateValue);
    }

    public void RefreshResData()
    {
        UpdateResData(ResData.Points, 0);
        UpdateResData(ResData.Locks, 0);
        UpdateResData(ResData.Spices, 0);
        UpdateResData(ResData.Checks, 0);
    }

    public void UpdateRewardDate(System.DateTime newDate)
    {
        rewardDate = newDate;
    }

}
