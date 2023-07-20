using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    [SerializeField] private int oreCount = 5;
    [SerializeField] private int shovels = 10;
    [SerializeField] private int insight = 10;
    [SerializeField] private int initialScoops = 15;
    [SerializeField] private UserScoreManager scoreManager;

    private int scoops;
    private Tool activeTool;
    public Tool ActiveTool => activeTool;

    public int Shovels => shovels;
    public int Scoops => scoops;
    public int Insight => insight;
    public int OreCount => oreCount;

    private void OnEnable()
    {
        oreCount = PlayerPrefs.HasKey("_OreCount") ? PlayerPrefs.GetInt("_OreCount") : oreCount;
        shovels = PlayerPrefs.HasKey("_Shovels") ? PlayerPrefs.GetInt("_Shovels") : shovels;
        insight = PlayerPrefs.HasKey("_Insight") ? PlayerPrefs.GetInt("_Insight") : insight;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_OreCount", oreCount);
        PlayerPrefs.SetInt("_Shovels", shovels);
        PlayerPrefs.SetInt("_Insight", insight);
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
            shovels--;
            scoreManager.UpdateValues(2, shovels);
        }
        else
        {
            insight--;
            scoreManager.UpdateValues(3, insight);
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
        scoreManager.UpdateValues(2, shovels);
        scoreManager.UpdateValues(4, shovels);
        scoreManager.UpdateValues(3, insight);
        scoreManager.UpdateValues(5, insight);
    }
}
