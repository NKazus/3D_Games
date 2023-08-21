using UnityEngine;

public enum GameResources
{
    Food,
    Fuel,
    Tools,
    Health
}

public class GameData : MonoBehaviour
{
    [SerializeField] private ScoreManager score;

    [SerializeField] private int money = 100;
    [SerializeField] private int reputation = 0;
    [SerializeField] private int food = 20;
    [SerializeField] private int fuel = 20;
    [SerializeField] private int tools = 20;
    [SerializeField] private int health = 20;

    public int Money => money;
    public int Reputation => reputation;
    public int Food => food;
    public int Fuel => fuel;
    public int Tools => tools;
    public int Health => health;

    private void OnEnable()
    {
        money = (PlayerPrefs.HasKey("_Money")) ? PlayerPrefs.GetInt("_Money") : money;
        reputation = (PlayerPrefs.HasKey("_Reputation")) ? PlayerPrefs.GetInt("_Reputation") : reputation;

        food = (PlayerPrefs.HasKey("_Food")) ? PlayerPrefs.GetInt("_Food") : food;
        fuel = (PlayerPrefs.HasKey("_Fuel")) ? PlayerPrefs.GetInt("_Fuel") : fuel;
        tools = (PlayerPrefs.HasKey("_Tools")) ? PlayerPrefs.GetInt("_Tools") : tools;
        health = (PlayerPrefs.HasKey("_Health")) ? PlayerPrefs.GetInt("_Health") : health;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_Money", money);
        PlayerPrefs.SetInt("_Reputation", money);

        PlayerPrefs.SetInt("_Food", money);
        PlayerPrefs.SetInt("_Fuel", money);
        PlayerPrefs.SetInt("_Tools", money);
        PlayerPrefs.SetInt("_Health", money);
    }

    
}
