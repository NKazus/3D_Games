using UnityEngine;

public enum GameResource
{
    Score,
    Walls
}
public class GameData : MonoBehaviour
{
    [SerializeField] private GameUI ui;
    [SerializeField] private int gameScore;
    [SerializeField] private int wallNumber;

    public int GameScore => gameScore;
    public int WallNumber => wallNumber;

    private void OnEnable()
    {
        gameScore = PlayerPrefs.HasKey("_GL_Score") ? PlayerPrefs.GetInt("_GL_Score") : gameScore;
        wallNumber = PlayerPrefs.HasKey("_GL_Walls") ? PlayerPrefs.GetInt("_GL_Walls") : wallNumber;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GL_Score", gameScore);
        PlayerPrefs.SetInt("_GL_Walls", wallNumber);
    }

    public void UpdateResourceValue(GameResource target, int value)
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

    public void UpdateRoundsValue(int value)
    {
        ui.UpdateRoundsValue(value);
    }
}
