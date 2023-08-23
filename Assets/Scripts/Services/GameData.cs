using System;
using UnityEngine;

public enum GameResources
{
    Food,
    Fuel,
    Tools,
    Meds
}

public class GameData : MonoBehaviour
{
    [SerializeField] private ScoreManager score;

    [SerializeField] private int money = 100;
    [SerializeField] private int reputation = 0;
    [SerializeField] private int food = 20;
    [SerializeField] private int fuel = 20;
    [SerializeField] private int tools = 20;
    [SerializeField] private int meds = 20;

    public int Money => money;
    public int Reputation => reputation;
    public int Food => food;
    public int Fuel => fuel;
    public int Tools => tools;
    public int Meds => meds;

    public DateTime IncomeDate => incomeDate;
    private DateTime incomeDate;

    private void OnEnable()
    {
        money = (PlayerPrefs.HasKey("_Money")) ? PlayerPrefs.GetInt("_Money") : money;
        reputation = (PlayerPrefs.HasKey("_Reputation")) ? PlayerPrefs.GetInt("_Reputation") : reputation;

        food = 20;// (PlayerPrefs.HasKey("_Food")) ? PlayerPrefs.GetInt("_Food") : food;
        fuel = 20;//(PlayerPrefs.HasKey("_Fuel")) ? PlayerPrefs.GetInt("_Fuel") : fuel;
        tools = 20;// (PlayerPrefs.HasKey("_Tools")) ? PlayerPrefs.GetInt("_Tools") : tools;
        meds = 20;// (PlayerPrefs.HasKey("_Meds")) ? PlayerPrefs.GetInt("_Meds") : meds;

        incomeDate = PlayerPrefs.HasKey("_DailyIncome") ? new DateTime(
           Convert.ToInt64(PlayerPrefs.GetString("_DailyIncome")))
           .ToLocalTime() : DateTime.Now.AddDays(-1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_Money", money);
        PlayerPrefs.SetInt("_Reputation", reputation);

        PlayerPrefs.SetInt("_Food", food);
        PlayerPrefs.SetInt("_Fuel", fuel);
        PlayerPrefs.SetInt("_Tools", tools);
        PlayerPrefs.SetInt("_Meds", meds);

        PlayerPrefs.SetString("_DailyIncome", "" + incomeDate.ToUniversalTime().Ticks);
    }

    public void UpdateMoney(int value)
    {
        money += value;
        if (money < 0)
        {
            money = 0;
        }
        score.UpdateGlobal(false, money);
    }

    public void UpdateRep(int value)
    {
        reputation += value;
        reputation = Mathf.Clamp(reputation, -100, 100);
        score.UpdateGlobal(true, reputation);
    }

    public void UpdateRes(GameResources id, int value, bool gameplay)
    {
        switch (id)
        {
            case GameResources.Food: food += value; score.UpdateResources(id, food, gameplay); break;
            case GameResources.Fuel: fuel += value; score.UpdateResources(id, fuel, gameplay); break;
            case GameResources.Tools: tools += value; score.UpdateResources(id, tools, gameplay); break;
            case GameResources.Meds: meds += value; score.UpdateResources(id, meds, gameplay); break;
            default: throw new NotSupportedException();
        }
    }

    public void UpdateAllRes(bool gameplay)
    {
        UpdateRes(GameResources.Food, 0, gameplay);
        UpdateRes(GameResources.Fuel, 0, gameplay);
        UpdateRes(GameResources.Tools, 0, gameplay);
        UpdateRes(GameResources.Meds, 0, gameplay);
    }

    public void RefreshDaily(DateTime newDate)
    {
        incomeDate = newDate;
    }
}
