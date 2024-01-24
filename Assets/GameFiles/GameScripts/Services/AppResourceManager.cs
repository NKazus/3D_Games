using System;
using UnityEngine;

public class AppResourceManager : MonoBehaviour
{
    [SerializeField] private int oreCount = 5;
    [SerializeField] private int shovels = 10;
    [SerializeField] private int insight = 10;
    [SerializeField] private int initialScoops = 15; //only constants
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
            playerRes = saveManager.LoadData<PlayerResources>($"/{saveFileName}.json", enableEncryption);
            Debug.Log($"Ore: {playerRes.oreCount} Shovels: {playerRes.shovels} Insight: {playerRes.insight}");
        }
        catch(Exception e)
        {
            Debug.LogError($"Loading exception: {e.Message} {e.StackTrace}");
            Debug.Log("Initialize start values!");
            playerRes.oreCount = oreCount;
            playerRes.shovels = shovels;
            playerRes.insight = insight;
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
        if (isShovel)
        {
            playerRes.shovels--;
            scoreManager.UpdateValues(2, playerRes.shovels);
        }
        else
        {
            insight--;
            scoreManager.UpdateValues(3, playerRes.insight);
        }
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
        scoreManager.UpdateValues(0, oreCount);
        scoreManager.UpdateValues(1, scoops);
        scoreManager.UpdateValues(2, playerRes.shovels);
        scoreManager.UpdateValues(4, shovels);
        scoreManager.UpdateValues(3, playerRes.insight);
        scoreManager.UpdateValues(5, insight);
    }
}
