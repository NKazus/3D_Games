using UnityEngine;

public enum GameResource
{
    Score,
    Walls
}
public class GameData : MonoBehaviour
{
    [SerializeField] private GameUIManager ui;
    [SerializeField] private int gameScore;
    [SerializeField] private int wallNumber;

    public int GameScore => gameScore;
    public int WallNumber => wallNumber;

    private void OnEnable()
    {
        gameScore = PlayerPrefs.HasKey("_GameScore") ? PlayerPrefs.GetInt("_GameScore") : gameScore;
        wallNumber = PlayerPrefs.HasKey("_WallNumber") ? PlayerPrefs.GetInt("_WallNumber") : wallNumber;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameScore", gameScore);
        PlayerPrefs.SetInt("_WallNumber", wallNumber);
    }

    public void UpdateResource(GameResource target, int value)
    {
        switch (target)
        {
            case GameResource.Score:
                gameScore += value;
                if (gameScore < 0)
                {
                    gameScore = 0;
                }
                ui.UpdateResourceValues(target, gameScore);
                break;
            case GameResource.Walls:
                wallNumber += value;
                ui.UpdateResourceValues(target, wallNumber);
                break;
            default: throw new System.NotSupportedException();
        }
    }

    public void UpdateRounds(int value)
    {
        ui.UpdateRoundsValue(value);
    }
}
