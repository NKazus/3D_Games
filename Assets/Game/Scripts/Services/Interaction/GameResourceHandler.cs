using UnityEngine;

public class GameResourceHandler : MonoBehaviour
{
    [SerializeField] private GameResourceHolder resourceHolder;
    [SerializeField] private int treasureScore;
    [SerializeField] private int sticks;

    public int Sticks => sticks;
    public int TreasureScore => treasureScore;

    private void OnEnable()
    {
        treasureScore = PlayerPrefs.HasKey("_TreasureScore") ? PlayerPrefs.GetInt("_TreasureScore") : treasureScore;
        sticks = PlayerPrefs.HasKey("_Sticks") ? PlayerPrefs.GetInt("_Sticks") : sticks;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_TreasureScore", treasureScore);
        PlayerPrefs.SetInt("_Sticks", sticks);
    }

    public void UpdateSticks(int value)
    {
        if (sticks < 1)
        {
            sticks = 1;
            resourceHolder.UpdateSticks(sticks);
            return;
        }
        sticks += value;
        resourceHolder.UpdateSticks(sticks);
    }

    public void UpdateTreasure(int value)
    {
        treasureScore += value;
        if(treasureScore < 0)
        {
            treasureScore = 0;
        }
        resourceHolder.UpdateScore(treasureScore);
    }

    public void UpdateTries(int value)
    {
        resourceHolder.UpdateTries(value);
    }
}
