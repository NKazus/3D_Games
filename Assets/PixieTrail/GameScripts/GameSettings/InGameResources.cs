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

    [SerializeField] private float defaultSpeedModif;
    [SerializeField] private float activeSpeedModif;
    [SerializeField] private float upgradeSpeedModif;

    [SerializeField] private int defaultShieldCharges;
    [SerializeField] private int activeShieldCharges;
    [SerializeField] private int upgradeShieldCharges;

    private bool shieldUpgrade; //is upgraded
    private bool pollenUpgrade;

    private float currentSpeedModifyer; //current modifyers, used in game
    private int currentShieldCharges;

    public int PlayerIncome => playerIncome;

    public int Shields => shieldNumber;
    public int Pollen => pollenNumber;

    public bool ShieldUpgrade => shieldUpgrade;
    public bool PollenUpgrade => pollenUpgrade;

    public float CurrentSpeedModifyer => currentSpeedModifyer;
    public float CurrentShieldCharges => currentShieldCharges;

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
        UpdateResource(AbilityType.Shield, 0);
        UpdateResource(AbilityType.Pollen, 0);

        UpgradeTool(AbilityType.Shield, shieldUpgrade);
        UpgradeTool(AbilityType.Pollen, pollenUpgrade);
    }

    public void UpdateResource(AbilityType id, int value)
    {
        int result;

        switch (id)
        {
            case AbilityType.Shield:
                shieldNumber += value; result = shieldNumber;
                break;
            case AbilityType.Pollen:
                pollenNumber += value; result = pollenNumber;
                break;
            default: throw new System.NotSupportedException();
        }
        ui.UpdateValues(id, result);
    }

    public void UpgradeTool(AbilityType id, bool value)
    {
        switch (id)
        {
            case AbilityType.Shield:
                shieldUpgrade = value;
                break;
            case AbilityType.Pollen:
                pollenUpgrade = value;                
                break;
            default: throw new System.NotSupportedException();
        }
        ui.UpdateIcons(id, value);
    }

    public void ActivateTool(AbilityType id, bool value)
    {
        switch (id)
        {
            case AbilityType.Shield:
                currentShieldCharges = value ? (shieldUpgrade ? upgradeShieldCharges : activeShieldCharges)
                    : defaultShieldCharges;
                //Debug.Log("curr_shields:"+currentShieldCharges);
                break;
            case AbilityType.Pollen:
                currentSpeedModifyer = value ? (pollenUpgrade ? upgradeSpeedModif : activeSpeedModif)
                    : defaultSpeedModif;
                //Debug.Log("curr_speed:" + currentSpeedModifyer);
                break;
            default: throw new System.NotSupportedException();
        }
    }

    public void UpdatePlayerIncome(int value)
    {
        playerIncome += value;
        if(playerIncome < 0)
        {
            playerIncome = 0;
        }
        ui.UpdateIncome(playerIncome);
    }
}
