using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private int globalScore;
    [SerializeField] private int bonusMultiplyer;

    public int BonusMultiplyer => bonusMultiplyer;
    public int GlobalScore => globalScore;

    private void OnEnable()
    {
        globalScore = PlayerPrefs.HasKey("_GlobalScore") ? PlayerPrefs.GetInt("_GlobalScore") : globalScore;
        bonusMultiplyer = PlayerPrefs.HasKey("_BonusMultiplyer") ? PlayerPrefs.GetInt("_BonusMultiplyer") : bonusMultiplyer;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GlobalScore", globalScore);
        PlayerPrefs.SetInt("_BonusMultiplyer", bonusMultiplyer);
    }

    public void ActivateBonus(bool activate)
    {
        bonusMultiplyer = activate ? 2 : 1;
    }

    public void UpdateGlobalScore(int value)
    {
        globalScore += value;
        if(globalScore < 0)
        {
            globalScore = 0;
        }
    }
}
