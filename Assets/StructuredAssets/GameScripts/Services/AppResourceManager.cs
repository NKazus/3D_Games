using System;
using UnityEngine;

public enum PlayerRes
{
    FreeAction,
    Lamp
}

public class AppResourceManager : MonoBehaviour
{
    [SerializeField] private int freeActions = 5;
    [SerializeField] private int lamps = 0;

    [SerializeField] private UserScoreManager scoreManager;
    [SerializeField] private string saveFileName;
    [SerializeField] private bool enableEncryption;

    private PlayerResources playerRes = new PlayerResources(); //all res changes here
    private ISaveManager saveManager = new JsonSaveManager();

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
        saveManager.SaveData($"/{saveFileName}.json", playerRes, enableEncryption);
    }

    private void DeserializeData()
    {
        try
        {
            playerRes = saveManager.LoadData<PlayerResources>($"/{saveFileName}.json", enableEncryption);
            playerRes.lamps = 3;
        }
        catch(Exception e)
        {
            Debug.LogError($"Loading exception: {e.Message} {e.StackTrace}");
            playerRes.freeActions = freeActions;
            playerRes.lamps = lamps;
        }
    }

    public int GetResValue(PlayerRes type)
    {
        return type switch
        {
            PlayerRes.FreeAction => playerRes.freeActions,
            PlayerRes.Lamp => playerRes.lamps,
            _ => throw new NotSupportedException()
        };
    }

    public void UpdateRes(PlayerRes type, int value)
    {
        int currentValue;
        switch (type)
        {
            case PlayerRes.FreeAction: playerRes.freeActions += value; currentValue = playerRes.freeActions; break;
            case PlayerRes.Lamp: playerRes.lamps += value; currentValue = playerRes.lamps; break;
            default: throw new NotSupportedException();
        }
        scoreManager.UpdateValues(type, currentValue);
    }

    public void Refresh()
    {
        scoreManager.UpdateValues(PlayerRes.Lamp, playerRes.lamps);
        scoreManager.UpdateValues(PlayerRes.FreeAction, playerRes.freeActions);
    }
}
