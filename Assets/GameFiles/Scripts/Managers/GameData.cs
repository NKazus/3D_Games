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

    public int Spices => spices;
    public int Locks => locks;
    public int Checks => checks;
    public int GameScore => gameScore;

    private void OnEnable()
    {
        gameScore = PlayerPrefs.HasKey("DATA_INT_Score") ? PlayerPrefs.GetInt("DATA_INT_Score") : gameScore;
        locks = PlayerPrefs.HasKey("DATA_INT_Locks") ? PlayerPrefs.GetInt("DATA_INT_Locks") : locks;
        spices = PlayerPrefs.HasKey("DATA_INT_Spices") ? PlayerPrefs.GetInt("DATA_INT_Spices") : spices;
        checks = PlayerPrefs.HasKey("DATA_INT_Checks") ? PlayerPrefs.GetInt("DATA_INT_Checks") : checks;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("DATA_INT_Score", gameScore);
        PlayerPrefs.SetInt("DATA_INT_Locks", locks);
        PlayerPrefs.SetInt("DATA_INT_Spices", spices);
        PlayerPrefs.SetInt("DATA_INT_Checks", checks);
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

        score.UpdateResUI(type, updateValue);
    }
}
