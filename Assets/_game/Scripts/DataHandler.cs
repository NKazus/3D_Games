using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [SerializeField] private int seeds;
    [SerializeField] private int money;
    [SerializeField] private int props;

    public int Seeds => seeds;
    public int Money => money;
    public int Props => props;

    [SerializeField] private ScoreManager scoreManager;

    private void OnEnable()
    {
        seeds = PlayerPrefs.HasKey("_Seeds") ? PlayerPrefs.GetInt("_Seeds") : seeds;
        money = 10;// PlayerPrefs.HasKey("_Money") ? PlayerPrefs.GetInt("_Money") : money;
        props = 10;// PlayerPrefs.HasKey("_Props") ? PlayerPrefs.GetInt("_Props") : props;
        scoreManager.UpdateSeeds(seeds);
        scoreManager.UpdateProps(props);
        scoreManager.UpdateMoney(money);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_Props", props);
        PlayerPrefs.SetInt("_Money", money);
        PlayerPrefs.SetInt("_Seeds", seeds);
    }

    public void UpdateSeeds(int amount)
    {
        seeds += amount;
        scoreManager.UpdateSeeds(seeds);
    }

    public void UpdateMoney(int amount)
    {
        money += amount;
        scoreManager.UpdateMoney(money);
    }

    public void UpdateProps(int amount)
    {
        props += amount;
        scoreManager.UpdateProps(props);
    }
}
