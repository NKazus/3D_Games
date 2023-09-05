using System;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private ScoreManager score;

    [SerializeField] private int money = 50;
    [SerializeField] private int charges = 10;
    [SerializeField] private bool timeScale = false;

    public int Money => money;
    public int Charges => charges;
    public bool TimeScale => timeScale;

    public DateTime IncomeDate => incomeDate;
    private DateTime incomeDate;

    private void OnEnable()
    {
        money = 100;// (PlayerPrefs.HasKey("_Money")) ? PlayerPrefs.GetInt("_Money") : money;
        charges = (PlayerPrefs.HasKey("_Charges")) ? PlayerPrefs.GetInt("_Charges") : charges;

        timeScale = false;// (PlayerPrefs.HasKey("_TimeScale")) ? PlayerPrefs.GetInt("_TimeScale") == 1 : timeScale;

        incomeDate = PlayerPrefs.HasKey("_DailyIncome") ? new DateTime(
           Convert.ToInt64(PlayerPrefs.GetString("_DailyIncome")))
           .ToLocalTime() : DateTime.Now.AddDays(-1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_Money", money);
        PlayerPrefs.SetInt("_Charges", charges);

        PlayerPrefs.SetInt("_TimeScale", timeScale ? 1 : 0);
 
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

    public void UpdateCharges(int value)
    {
        charges += value;
        if(charges < 0)
        {
            charges = 0;
        }
        score.UpdateGlobal(true, charges);
    }

    public void UpdateTime(bool active)
    {
        timeScale = active;
    }

    public void RefreshDaily(DateTime newDate)
    {
        incomeDate = newDate;
    }
}
