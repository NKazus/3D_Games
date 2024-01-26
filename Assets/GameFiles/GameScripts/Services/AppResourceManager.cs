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
    [SerializeField] private int oreCount = 5;
    [SerializeField] private int shovels = 10;
    [SerializeField] private int insight = 10;
    [SerializeField] private int initialScoops = 15; //only constants

    [SerializeField] private int freeActions = 5;
    [SerializeField] private int poorMedals = 0;
    [SerializeField] private int ordinaryMedals = 0;
    [SerializeField] private int luxMedals = 0;

    [SerializeField] private UserScoreManager scoreManager;
    [SerializeField] private string saveFileName;
    [SerializeField] private bool enableEncryption;

    private int scoops;
    private Tool activeTool;
    public Tool ActiveTool => activeTool;

    public int Shovels => shovels;
    public int Scoops => scoops;
    public int Insight => insight;
    public int OreCount => oreCount;

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
            playerRes.freeActions = freeActions;
            playerRes.poorMedals = poorMedals;
            playerRes.ordinaryMedals = ordinaryMedals;
            playerRes.luxMedals = luxMedals;
            //playerRes = saveManager.LoadData<PlayerResources>($"/{saveFileName}.json", enableEncryption);
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
        switch (type)
        {
            case PlayerRes.FreeAction: playerRes.freeActions += value; break;
            case PlayerRes.PoorMedal: playerRes.poorMedals += value; break;
            case PlayerRes.OrdinaryMedal: playerRes.ordinaryMedals += value; break;
            case PlayerRes.LuxMedal: playerRes.luxMedals += value; break;
            default: throw new NotSupportedException();
        }
    }

    public void AddBonus(bool isShovel)
    {
        if (isShovel)
        {
            shovels++;
            scoreManager.UpdateValues(2, shovels);
            scoreManager.UpdateValues(4, shovels);
        }
        else
        {
            insight++;
            scoreManager.UpdateValues(3, insight);
            scoreManager.UpdateValues(5, insight);
        }
    }

    public void RemoveBonus(bool isShovel)
    {
        
    }

    public void SetActiveTool(Tool value)
    {
        activeTool = value;
    }

    public void SetScoops(bool reset = false)
    {
        scoops = reset ? initialScoops : --scoops;
        scoreManager.UpdateValues(1, scoops);
    }

    public void UpdateGlobalScore(int value)
    {
        oreCount += value;
        if(oreCount < 0)
        {
            oreCount = 0;
        }
        scoreManager.UpdateValues(0, oreCount);
    }

    public void Refresh()
    {
        
    }
}
