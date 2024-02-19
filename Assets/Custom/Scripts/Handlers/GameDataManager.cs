using UnityEngine;
using CMGame.Saves;

public enum DataType
{
    Switches,
    Adds,
    Points
}

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameScoreManager scoreManager;

    [SerializeField] private int score;
    [SerializeField] private int switches;
    [SerializeField] private int adds;

    [SerializeField] private string playerFileName;
    [SerializeField] private bool enableEncryption;

    private JsonSaveManager saveManager = new JsonSaveManager();
    private PlayerData playerData = new PlayerData();

    private void OnEnable()
    {
        //globalScore = PlayerPrefs.HasKey("_GlobalScore") ? PlayerPrefs.GetInt("_GlobalScore") : globalScore;
        DeserializeData();
    }

    private void OnDisable()
    {
        //PlayerPrefs.SetInt("_GlobalScore", globalScore);
        SerializeData();
    }

    private void SerializeData()
    {
        saveManager.SaveData($"/{playerFileName}.json", playerData, enableEncryption);
    }

    private void DeserializeData()
    {
        try
        {
            playerData = saveManager.LoadData<PlayerData>($"/{playerFileName}.json", enableEncryption);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Loading exception: {e.Message} {e.StackTrace}");
            playerData.points = score;
            playerData.adds = adds;
            playerData.switches = switches;
            playerData.rewardDate = System.DateTime.Now.AddDays(-2);
        }
    }

    public void UpdateData(DataType type, int value)
    {
        int targetValue;
        switch (type)
        {
            case DataType.Points: playerData.points += value; targetValue = playerData.points; break;
            case DataType.Switches: playerData.switches += value; targetValue = playerData.switches; break;
            case DataType.Adds: playerData.adds += value; targetValue = playerData.adds; break;
            default: throw new System.NotSupportedException();
        }
        scoreManager.UpdateValues(type, targetValue);
    }

    public int GetData(DataType type)
    {
        return type switch
        {
            DataType.Points => playerData.points,
            DataType.Switches => playerData.switches,
            DataType.Adds => playerData.adds,
            _ => throw new System.NotSupportedException()
        };
    }

    public void RefreshData()
    {
        UpdateData(DataType.Points, 0);
        UpdateData(DataType.Switches, 0);
        UpdateData(DataType.Adds, 0);
    }

    public void UpdateReward(System.DateTime dateTime)
    {
        playerData.rewardDate = dateTime;
    }

    public System.DateTime GetRewardDate()
    {
        return playerData.rewardDate;
    }
}
