using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Balance : MonoBehaviour
{
    [SerializeField] private Button balanceButton;
    [SerializeField] private Button button10;
    [SerializeField] private Button button30;

    [SerializeField] private Text balanceText;

    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    private int b10c;
    private int b30c;

    private bool activeBalancing;
    private bool isBalancing;

    private float balancePercent;

    [Inject] private readonly Resources resources;

    private void Awake()
    {
        button10.image.color = inactiveColor;
        button30.image.color = inactiveColor;
        balanceButton.image.color = inactiveColor;
    }

    private void OnDisable()
    {
        LockBalance();
    }

    private void ChoosePercent(int percent)
    {
        balancePercent = percent / 100f;
        if (b30c > 0)
        {
            button30.image.DOColor(percent > 20 ? activeColor : Color.white, 0.4f);
        }
        if (b10c > 0)
        {
            button10.image.DOColor(percent < 20 ? activeColor : Color.white, 0.4f);
        }
        
        if (!activeBalancing)
        {
            balanceButton.onClick.AddListener(SwitchBalance);
            balanceButton.image.DOColor(Color.white, 0.4f);
            activeBalancing = true;
        }        
    }

    private void SwitchBalance()
    {
        isBalancing = !isBalancing;
        balanceButton.image.DOColor(isBalancing ? activeColor : Color.white, 0.4f);
    }

    private void CheckCharges()
    {

        if (b10c > 0)
        {
            button10.onClick.AddListener(() => ChoosePercent(10));
            button10.image.DOColor(Color.white, 0.4f);
        }
        if (b30c > 0)
        {
            button30.onClick.AddListener(() => ChoosePercent(30));
            button30.image.DOColor(Color.white, 0.4f);
        }
    }

    public void ResetBalance()
    {
        activeBalancing = false;
        balanceText.enabled = false;
        isBalancing = false;
        balancePercent = 0;

        CheckCharges();
    }

    public void LockBalance()
    {
        button10.image.DOColor(inactiveColor, 0.4f);
        button30.image.DOColor(inactiveColor, 0.4f);
        balanceButton.image.DOColor(inactiveColor, 0.4f);

        balanceButton.onClick.RemoveListener(SwitchBalance);
        activeBalancing = false;
        balanceText.enabled = false;

        button10.onClick.RemoveAllListeners();
        button30.onClick.RemoveAllListeners();
    }

    public int DoBalance(int playerSum, int cardsSum, float min, float max)
    {
        if (!isBalancing)
        {
            return playerSum;
        }

        ShowProcess(true);

        float currentPercentage = (float)playerSum / cardsSum;

        float balanceSign = 0f;
        if(currentPercentage < min)
        {
            balanceSign = 1f;
        }
        if (currentPercentage > max)
        {
            balanceSign = -1f;
        }

        //resources.UpdateBalanceCharges(balancePercent < 0.2f, -1);
        return (int)(playerSum * (1f + balanceSign * balancePercent));
    }

    public void ShowProcess(bool isVisible)
    {
        balanceText.text = "Balancing...";
        balanceText.enabled = isVisible;        
    }
}
