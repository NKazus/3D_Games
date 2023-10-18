using UnityEngine;

public enum AbilityType
{
    Shield,
    Pollen
}
public class InGameResources : MonoBehaviour
{
    [SerializeField] private InGameUI ui;
    [SerializeField] private int playerIncome;
    [SerializeField] private int shieldNumber;
    [SerializeField] private int pollenNumber;

    private bool shieldUpgrade;
    private bool pollenUpgrade;

    public int PlayerIncome => playerIncome;

    public int Shields => shieldNumber;
    public int Pollen => pollenNumber;

    public bool ShieldUpgrade => shieldUpgrade;
    public bool PollenUpdrade => pollenUpgrade;

    private void OnEnable()
    {
        playerIncome = PlayerPrefs.HasKey("_PT_PlayerIncome") ? PlayerPrefs.GetInt("_PT_PlayerIncome") : playerIncome;

        shieldNumber = PlayerPrefs.HasKey("_PT_ShieldNumber") ? PlayerPrefs.GetInt("_PT_ShieldNumber") : shieldNumber;
        pollenNumber = PlayerPrefs.HasKey("_PT_PollenNumber") ? PlayerPrefs.GetInt("_PT_PollenNumber") : pollenNumber;

        shieldUpgrade = PlayerPrefs.HasKey("_PT_ShieldUpgrade") ? (PlayerPrefs.GetInt("_PT_ShieldUpgrade") == 1) : false;
        pollenUpgrade = PlayerPrefs.HasKey("_PT_PollenUpgrade") ? (PlayerPrefs.GetInt("_PT_PollenUpgrade") == 1) : false;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_PT_PlayerIncome", playerIncome);

        PlayerPrefs.SetInt("_PT_ShieldNumber", shieldNumber);
        PlayerPrefs.SetInt("_PT_PollenNumber", pollenNumber);

        PlayerPrefs.SetInt("_PT_ShieldUpgrade", shieldUpgrade ? 1 : 0);
        PlayerPrefs.SetInt("_PT_PollenUpgrade", pollenUpgrade ? 1 : 0);
    }

    public void UpdateResources()
    {
        UpdateResources(AbilityType.Shield, 0);
        UpdateResources(AbilityType.Pollen, 0);

        UpgradeTools(AbilityType.Shield, shieldUpgrade);
        UpgradeTools(AbilityType.Pollen, pollenUpgrade);
    }

    public void UpdateResources(AbilityType id, int value)
    {
        int result;
        switch (id)
        {
            case AbilityType.Shield: shieldNumber += value; result = shieldNumber; break;
            case AbilityType.Pollen: pollenNumber += value; result = pollenNumber; break;
            default: throw new System.NotSupportedException();
        }
        ui.UpdateValues(id, result);
    }

    public void UpgradeTools(AbilityType id, bool value)
    {
        switch (id)
        {
            case AbilityType.Shield: shieldUpgrade = value; break;
            case AbilityType.Pollen: pollenUpgrade = value; break;
            default: throw new System.NotSupportedException();
        }
        ui.UpdateIcons(id, value);
    }

    public void UpdatePlayerIncome(int value)
    {
        playerIncome += value;
        if(playerIncome < 0)
        {
            playerIncome = 0;
        }
        ui.UpdateValues(0, playerIncome);
    }
}
