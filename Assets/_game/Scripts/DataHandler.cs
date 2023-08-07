using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public int Potions => potions;
    [SerializeField] private int potions;
    [SerializeField] private Reagent[] reagents;
    [SerializeField] private ScoreManager scoreManager;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("_Reagent0"))
        {
            for (int i = 0; i < reagents.Length; i++)
            {
                reagents[i].amount = PlayerPrefs.GetInt("_Reagent" + i);
            }
        }
        potions = PlayerPrefs.HasKey("_Potions") ? PlayerPrefs.GetInt("_Potions") : potions;
        scoreManager.UpdatePotions(potions);
        for (int i = 0; i < reagents.Length; i++)
        {
            scoreManager.UpdateReagents(i, reagents[i].amount);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < reagents.Length; i++)
        {
            PlayerPrefs.SetInt("_Reagent" + i, reagents[i].amount);
        }
        PlayerPrefs.SetInt("_Potions", potions);
    }

    public void TradePotions(int price, int reagentID)
    {
        potions -= price;
        scoreManager.UpdatePotions(potions);

        reagents[reagentID].amount++;
        scoreManager.UpdateReagents(reagentID, reagents[reagentID].amount);
    }

    public void UpdatePotions(int bonusPotions)
    {
        potions += bonusPotions + 5;
        scoreManager.UpdatePotions(potions);
    }

    public bool CheckReagent(int reagentID)
    {
        if(reagents[reagentID].amount > 0)
        {
            reagents[reagentID].amount--;
            scoreManager.UpdateReagents(reagentID, reagents[reagentID].amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public Reagent GetReagent(int reagentID)
    {
        return reagents[reagentID];
    }

    public Reagent GetReagentByValue(int reagentValue)
    {
        Reagent reagent = null;
        for(int i = 0; i < reagents.Length; i++)
        {
            if(reagents[i].value == reagentValue)
            {
                reagent = reagents[i];
                break;
            }
        }
        return reagent;
    }
}
