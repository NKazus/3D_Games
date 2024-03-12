using UnityEngine;
using Bouncer.Saves;

public enum DataType
{
    MainScore,
    BonusScore
}

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameScoreManager scoreManager;

    [SerializeField] private int mScore;
    [SerializeField] private int bScore;

    [SerializeField] private string playerFileName;
    [SerializeField] private bool enableEncryption;

    private JsonSaveManager saveManager = new JsonSaveManager();
    private PlayerData playerData = new PlayerData();

    private void OnEnable()
    {
        DeserializeData();
    }

    private void OnDisable()
    {
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
            playerData.mainScore = mScore;
            playerData.bonusScore = bScore;
        }
    }

    public void UpdateData(DataType type, int value)
    {
        switch (type)
        {
            case DataType.MainScore: playerData.mainScore = value; break;
            case DataType.BonusScore: playerData.bonusScore = value; break;
            default: throw new System.NotSupportedException();
        }
        scoreManager.UpdateValues(type, value);
    }

    public int GetData(DataType type)
    {
        return type switch
        {
            DataType.MainScore => playerData.mainScore,
            DataType.BonusScore => playerData.bonusScore,
            _ => throw new System.NotSupportedException()
        };
    }

    public void RefreshData(DataType type)
    {
        UpdateData(type, GetData(type));
    }
}
