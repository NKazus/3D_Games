using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Setup : MonoBehaviour
{
    [SerializeField] private Button slow;
    [SerializeField] private Button boost;
    [SerializeField] private Button damage;
    [SerializeField] private Button heal;
    [SerializeField] private Button get;

    [SerializeField] private int slowPrice;
    [SerializeField] private int boostPrice;
    [SerializeField] private int damagePrice;
    [SerializeField] private int healPrice;

    [SerializeField] private Text slowText;
    [SerializeField] private Text boostText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text healText;
    [SerializeField] private Text sumText;

    [SerializeField] private Image slowIcon;
    [SerializeField] private Image boostIcon;
    [SerializeField] private Image damageIcon;
    [SerializeField] private Image healIcon;

    private List<AbilityType> enabledBuffs;
    private int sumPrice;

    [Inject] private readonly InGameResources dataHandler;
    [Inject] private readonly InGameEvents events;

   /* private void Awake()
    {
        slowText.text = slowPrice.ToString();
        healText.text = healPrice.ToString();
        boostText.text = boostPrice.ToString();
        damageText.text = damagePrice.ToString();

        enabledBuffs = new List<AbilityType>();
    }

    private void OnEnable()
    {
        dataHandler.UpdatePlayerIncome(0);
        CheckBuffs();

        get.onClick.AddListener(ActivateMany);
        slow.onClick.AddListener(() => PickBuff(AbilityType.Slow));
        boost.onClick.AddListener(() => PickBuff(AbilityType.Boost));
        heal.onClick.AddListener(() => PickBuff(AbilityType.Heal));
        damage.onClick.AddListener(() => PickBuff(AbilityType.Damage));

    }

    private void OnDisable()
    {
        get.onClick.RemoveListener(ActivateMany);
        slow.onClick.RemoveAllListeners();
        boost.onClick.RemoveAllListeners();
        heal.onClick.RemoveAllListeners();
        damage.onClick.RemoveAllListeners();
    }

    private void CheckBuffs()
    {
        sumPrice = 0;
        enabledBuffs.Clear();
        CheckBuff(AbilityType.Boost);
        CheckBuff(AbilityType.Heal);
        CheckBuff(AbilityType.Slow);
        CheckBuff(AbilityType.Damage);

        sumText.text = sumPrice.ToString();
        if (sumPrice == 0)
        {
            get.interactable = false;
        }
        else
        {
            if(sumPrice <= dataHandler.PlayerIncome)
            {
                sumText.color = Color.white;
                get.interactable = true;
            }
            else
            {
                sumText.color = Color.red;
                get.interactable = false;
            }
        }        
    }

    private void CheckBuff(AbilityType type)
    {
        bool cash;
        switch (type)
        {
            case AbilityType.Boost:
                boostText.color = Color.white;
                cash = dataHandler.PlayerIncome >= boostPrice;
                if (!dataHandler.Boost && cash)
                {
                    boost.interactable = true;
                    boostIcon.color = Color.white;
                    sumPrice += boostPrice;
                    enabledBuffs.Add(type);
                    return;
                }
                else if (!cash)
                {
                    boostText.color = Color.red;
                }
                boost.interactable = false;
                boostIcon.color = Color.gray;
                break;
            case AbilityType.Heal:
                healText.color = Color.white;
                cash = dataHandler.PlayerIncome >= healPrice;
                if (!dataHandler.Heal && cash)
                {
                    heal.interactable = true;
                    healIcon.color = Color.white;
                    sumPrice += healPrice;
                    enabledBuffs.Add(type);
                    return;
                }
                else if (!cash)
                {
                    healText.color = Color.red;
                }
                heal.interactable = false;
                healIcon.color = Color.gray;
                break;
            case AbilityType.Slow:
                slowText.color = Color.white;
                cash = dataHandler.PlayerIncome >= slowPrice;
                if (!dataHandler.Slow && cash)
                {
                    slow.interactable = true;
                    slowIcon.color = Color.white;
                    sumPrice += slowPrice;
                    enabledBuffs.Add(type);
                    return;
                }
                else if (!cash)
                {
                    slowText.color = Color.red;
                }
                slow.interactable = false;
                slowIcon.color = Color.gray;
                break;
            default:
                damageText.color = Color.white;
                cash = dataHandler.PlayerIncome >= damagePrice;
                if (!dataHandler.Damage && cash)
                {
                    damage.interactable = true;
                    damageIcon.color = Color.white;
                    sumPrice += damagePrice;
                    enabledBuffs.Add(type);
                    return;
                }
                else if (!cash)
                {
                    damageText.color = Color.red;
                }
                damage.interactable = false;
                damageIcon.color = Color.gray;
                break;
        }
    }

    private void PickBuff(AbilityType type)
    {
        int price;
        switch (type)
        {
            case AbilityType.Boost: price = boostPrice; break;
            case AbilityType.Heal: price = healPrice; break;
            case AbilityType.Slow: price = slowPrice; break;
            default: price = damagePrice; break;
        }
        dataHandler.UpdatePlayerIncome(-price);
        ActivateOne(type);
        events.PlayBonus();
        CheckBuffs();
    }

    private void ActivateOne(AbilityType type)
    {
        dataHandler.UpdateResources(type, true);
    }

    private void ActivateMany()
    {
        dataHandler.UpdatePlayerIncome(-sumPrice);
        for (int i = 0; i < enabledBuffs.Count; i++)
        {
            ActivateOne(enabledBuffs[i]);
        }
        events.PlayBonus();
        CheckBuffs();
    }*/
}
