using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private int globalScore;
    [SerializeField] private int magicDices;

    public int ScoreValue => currentScoreValue;
    private int currentScoreValue;

    public int MagicDices => magicDices;
    public int GlobalScore => globalScore;

    private void OnEnable()
    {
        globalScore = PlayerPrefs.HasKey("_GlobalScore") ? PlayerPrefs.GetInt("_GlobalScore") : globalScore;
        //magicDices = PlayerPrefs.HasKey("_MagicDices") ? PlayerPrefs.GetInt("_MagicDices") : magicDices;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GlobalScore", globalScore);
        PlayerPrefs.SetInt("_MagicDices", magicDices);
    }

    public void ReduceMagicDices()
    {
        magicDices--;
    }

    public void UpdateGlobalScore(int value)
    {
        globalScore += value;
    }
}
