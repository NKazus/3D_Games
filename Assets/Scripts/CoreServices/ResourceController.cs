using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private int gameProgress;
    [SerializeField] private bool scanActive;

    private int currentGameSession;
    private int extraScanCharges;

    public bool ScanActive => scanActive;
    public int GameProgress => gameProgress;
    public int CurrentGameSession => currentGameSession;
    public int ExtraScanCharges => extraScanCharges;

    private void OnEnable()
    {
        gameProgress = PlayerPrefs.HasKey("_GameProgress") ? PlayerPrefs.GetInt("_GameProgress") : gameProgress;
        scanActive = PlayerPrefs.HasKey("_ScanActive") ? (PlayerPrefs.GetInt("_ScanActive") == 1) : scanActive;
        currentGameSession = PlayerPrefs.HasKey("_GameSession") ? PlayerPrefs.GetInt("_GameSession") : currentGameSession;
        extraScanCharges = PlayerPrefs.HasKey("_ScanCharges") ? PlayerPrefs.GetInt("_ScanCharges") : extraScanCharges;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameProgress", gameProgress);
        PlayerPrefs.SetInt("_ScanActive", scanActive ? 1 : 0);
        PlayerPrefs.SetInt("_GameSession", currentGameSession);
        PlayerPrefs.SetInt("_ScanCharges", extraScanCharges);
    }

    public void SetScanStatus(bool value)
    {
        scanActive = value;
    }

    public void UpdateProgress(int value)
    {
        gameProgress += value;
        if(gameProgress < 0)
        {
            gameProgress = 0;
        }
        scoreManager.UpdateScore(gameProgress);
    }

    public void UpdateTries(int value)
    {
        scoreManager.UpdateTries(value);
    }

    public void UpdateGameSession(bool reset = false)
    {
        if (reset)
        {
            currentGameSession = 0;
            return;
        }
        currentGameSession++;
    }

    public void UpdateScanExtra(int value)
    {
        extraScanCharges += value;
        if (extraScanCharges < 0)
        {
            extraScanCharges = 0;
        }
        scoreManager.UpdateExtraScans(extraScanCharges);
    }
}
