using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private int globalScore;
    [SerializeField] private float bonusTime;
    [SerializeField] private float bonusTimeDelta;

    public float BonusTime => bonusTime;
    public int GlobalScore => globalScore;

    private void OnEnable()
    {
        globalScore = PlayerPrefs.HasKey("_GlobalScore") ? PlayerPrefs.GetInt("_GlobalScore") : globalScore;
        bonusTime = PlayerPrefs.HasKey("_BonusTime") ? PlayerPrefs.GetFloat("_BonusTime") : bonusTime;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GlobalScore", globalScore);
        PlayerPrefs.SetFloat("_BonusTime", bonusTime);
    }

    public void SetBonusTime(bool reset = false)
    {
        bonusTime = reset ? 0 : bonusTime + bonusTimeDelta;
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
