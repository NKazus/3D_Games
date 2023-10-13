using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private int gameProgress;
    [SerializeField] private bool scanActive;

    public bool ScanActive => scanActive;
    public int GameProgress => gameProgress;

    private void OnEnable()
    {
        gameProgress = PlayerPrefs.HasKey("_GameProgress") ? PlayerPrefs.GetInt("_GameProgress") : gameProgress;
        scanActive = PlayerPrefs.HasKey("_ScanActive") ? (PlayerPrefs.GetInt("_ScanActive") == 1) : scanActive;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameProgress", gameProgress);
        PlayerPrefs.SetInt("_ScanActive", scanActive ? 1 : 0);
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
}
