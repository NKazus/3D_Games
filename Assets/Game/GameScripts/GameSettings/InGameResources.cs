using UnityEngine;

public enum BuffType
{
    Boost,
    Heal,
    Slow,
    Damage
}
public class InGameResources : MonoBehaviour
{
    [SerializeField] private GameUIManager ui;
    [SerializeField] private int gameScore;

    private bool boost;
    private bool heal;
    private bool slow;
    private bool damage;

    public int GameScore => gameScore;

    public bool Boost => boost;
    public bool Heal => heal;
    public bool Slow => slow;
    public bool Damage => damage;

    private void OnEnable()
    {
        gameScore = PlayerPrefs.HasKey("_GameScore") ? PlayerPrefs.GetInt("_GameScore") : gameScore;

        boost = PlayerPrefs.HasKey("_Boost") ? (PlayerPrefs.GetInt("_Boost") == 1) : false;
        heal = PlayerPrefs.HasKey("_Heal") ? (PlayerPrefs.GetInt("_Heal") == 1) : false;
        slow = PlayerPrefs.HasKey("_Slow") ? (PlayerPrefs.GetInt("_Slow") == 1) : false;
        damage = PlayerPrefs.HasKey("_Damage") ? (PlayerPrefs.GetInt("_Damage") == 1) : false;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_GameScore", gameScore);

        PlayerPrefs.SetInt("_Boost", boost ? 1 : 0);
        PlayerPrefs.SetInt("_Heal", heal ? 1 : 0);
        PlayerPrefs.SetInt("_Slow", slow ? 1 : 0);
        PlayerPrefs.SetInt("_Damage", damage ? 1 : 0);
    }

    public void UpdateBuff(BuffType id, bool activate)
    {
        switch (id)
        {
            case BuffType.Boost: boost = activate; break;
            case BuffType.Heal: heal = activate; break;
            case BuffType.Slow: slow = activate; break;
            default: damage = activate; break;
        }
        ui.UpdateIcons(id, activate);
    }

    public void RefreshBuffs(bool reset = false)
    {
        if (reset)
        {
            UpdateBuff(BuffType.Boost, false);
            UpdateBuff(BuffType.Slow, false);
            UpdateBuff(BuffType.Heal, false);
            UpdateBuff(BuffType.Damage, false);
            return;
        }
        ui.UpdateIcons(BuffType.Boost, boost);
        ui.UpdateIcons(BuffType.Heal, heal);
        ui.UpdateIcons(BuffType.Slow, slow);
        ui.UpdateIcons(BuffType.Damage, damage);
    }

    public void UpdateGlobalScore(int value)
    {
        gameScore += value;
        if(gameScore < 0)
        {
            gameScore = 0;
        }
        ui.UpdateValues(0, gameScore);
    }

    public void UpdateRounds(int value)
    {
        ui.UpdateValues(1, value);
    }
}
