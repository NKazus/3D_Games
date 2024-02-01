using System;
using UnityEngine;

public enum PlayerRes
{
    FreeAction,
    PoorMedal,
    OrdinaryMedal,
    LuxMedal
}

public class AppResourceManager : MonoBehaviour
{
    [SerializeField] private int freeActions = 5;
    [SerializeField] private int poorMedals = 0;
    [SerializeField] private int ordinaryMedals = 0;
    [SerializeField] private int luxMedals = 0;

    [SerializeField] private UserScoreManager scoreManager;
    [SerializeField] private string saveFileName;
    [SerializeField] private bool enableEncryption;

    private Tool activeTool;
    public Tool ActiveTool => activeTool;

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
        }
        catch(Exception e)
        {
            Debug.LogError($"Loading exception: {e.Message} {e.StackTrace}");
            //Debug.Log("Initialize start values!");
            playerRes.freeActions = freeActions;
            playerRes.poorMedals = poorMedals;
            playerRes.ordinaryMedals = ordinaryMedals;
            playerRes.luxMedals = luxMedals;
        }
    }

    public int GetResValue(PlayerRes type)
    {
        return type switch
        {
            PlayerRes.FreeAction => playerRes.freeActions,
            PlayerRes.PoorMedal => playerRes.poorMedals,
            PlayerRes.OrdinaryMedal => playerRes.ordinaryMedals,
            PlayerRes.LuxMedal => playerRes.luxMedals,
            _ => throw new NotSupportedException()
        };
    }

    public void UpdateRes(PlayerRes type, int value)
    {
        int currentValue;
        switch (type)
        {
            case PlayerRes.FreeAction: playerRes.freeActions += value; return;
            case PlayerRes.PoorMedal: playerRes.poorMedals += value; currentValue = playerRes.poorMedals; break;
            case PlayerRes.OrdinaryMedal: playerRes.ordinaryMedals += value; currentValue = playerRes.ordinaryMedals; break;
            case PlayerRes.LuxMedal: playerRes.luxMedals += value; currentValue = playerRes.luxMedals; break;
            default: throw new NotSupportedException();
        }
        scoreManager.UpdateValues(type, currentValue);
    }

    public void Refresh()
    {
        scoreManager.UpdateValues(PlayerRes.PoorMedal, playerRes.poorMedals);
        scoreManager.UpdateValues(PlayerRes.OrdinaryMedal, playerRes.ordinaryMedals);
        scoreManager.UpdateValues(PlayerRes.LuxMedal, playerRes.luxMedals);
    }
}
