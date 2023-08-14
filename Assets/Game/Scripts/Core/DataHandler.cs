using UnityEngine;

public enum Buff
{
    Boost,
    Heal,
    Slow,
    Damage
}
public class DataHandler : MonoBehaviour
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

    public void UpdateBuff(Buff id, bool activate)
    {
        switch (id)
        {
            case Buff.Boost: boost = activate; break;
            case Buff.Heal: heal = activate; break;
            case Buff.Slow: slow = activate; break;
            default: damage = activate; break;
        }
        ui.UpdateIcons(id, activate);
    }

    public void RefreshBuffs(bool reset = false)
    {
        if (reset)
        {
            UpdateBuff(Buff.Boost, false);
            UpdateBuff(Buff.Heal, false);
            UpdateBuff(Buff.Slow, false);
            UpdateBuff(Buff.Damage, false);
            return;
        }
        ui.UpdateIcons(Buff.Boost, boost);
        ui.UpdateIcons(Buff.Heal, boost);
        ui.UpdateIcons(Buff.Slow, boost);
        ui.UpdateIcons(Buff.Damage, boost);
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
