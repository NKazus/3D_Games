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

    private List<BuffType> enabledBuffs;
    private int sumPrice;

    [Inject] private readonly DataHandler dataHandler;
    [Inject] private readonly GlobalEventManager events;

    private void Awake()
    {
        slowText.text = slowPrice.ToString();
        healText.text = healPrice.ToString();
        boostText.text = boostPrice.ToString();
        damageText.text = damagePrice.ToString();

        enabledBuffs = new List<BuffType>();
    }

    private void OnEnable()
    {
        dataHandler.UpdateGlobalScore(0);
        CheckBuffs();

        get.onClick.AddListener(ActivateMany);
        slow.onClick.AddListener(() => PickBuff(BuffType.Slow));
        boost.onClick.AddListener(() => PickBuff(BuffType.Boost));
        heal.onClick.AddListener(() => PickBuff(BuffType.Heal));
        damage.onClick.AddListener(() => PickBuff(BuffType.Damage));

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
        CheckBuff(BuffType.Boost);
        CheckBuff(BuffType.Heal);
        CheckBuff(BuffType.Slow);
        CheckBuff(BuffType.Damage);

        sumText.text = sumPrice.ToString();
        if (sumPrice == 0)
        {
            get.interactable = false;
        }
        else
        {
            if(sumPrice <= dataHandler.GameScore)
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

    private void CheckBuff(BuffType type)
    {
        bool cash;
        switch (type)
        {
            case BuffType.Boost:
                boostText.color = Color.white;
                cash = dataHandler.GameScore >= boostPrice;
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
            case BuffType.Heal:
                healText.color = Color.white;
                cash = dataHandler.GameScore >= healPrice;
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
            case BuffType.Slow:
                slowText.color = Color.white;
                cash = dataHandler.GameScore >= slowPrice;
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
                cash = dataHandler.GameScore >= damagePrice;
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

    private void PickBuff(BuffType type)
    {
        int price;
        switch (type)
        {
            case BuffType.Boost: price = boostPrice; break;
            case BuffType.Heal: price = healPrice; break;
            case BuffType.Slow: price = slowPrice; break;
            default: price = damagePrice; break;
        }
        dataHandler.UpdateGlobalScore(-price);
        ActivateOne(type);
        events.PlayBonus();
        CheckBuffs();
    }

    private void ActivateOne(BuffType type)
    {
        dataHandler.UpdateBuff(type, true);
    }

    private void ActivateMany()
    {
        dataHandler.UpdateGlobalScore(-sumPrice);
        for (int i = 0; i < enabledBuffs.Count; i++)
        {
            ActivateOne(enabledBuffs[i]);
        }
        events.PlayBonus();
        CheckBuffs();
    }
}
