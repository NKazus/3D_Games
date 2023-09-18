using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BalanceButton : MonoBehaviour
{
    private Text valueText;
    private Button balanceButton;

    private int buttonValue;
    private Action<int> buttonClickCallback;

    private bool isActive;

    private void Awake()
    {
        valueText = transform.GetChild(0).GetComponent<Text>();
        balanceButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        isActive = false;
        valueText.enabled = false;
        balanceButton.image.enabled = false;
    }

    private void OnDisable()
    {
        balanceButton.onClick.RemoveAllListeners();
    }

    public void SetButton(int value, Action<int> buttonCallback)
    {
        buttonValue = value;
        valueText.text = value.ToString();

        balanceButton.image.DOFade(1f, 0.4f);
        balanceButton.onClick.AddListener(ClickButton);

        valueText.enabled = true;
        balanceButton.image.enabled = true;

        buttonClickCallback = buttonCallback;
        isActive = true;
    }

    private void ClickButton()
    {
        ResetButton();

        buttonClickCallback(buttonValue);
    }

    public void ResetButton(bool soft = false)
    {
        if (!isActive)
        {
            return;
        }
        if (!soft)
        {
            balanceButton.image.DOFade(0.5f, 0.4f);
            isActive = false;
        }
        balanceButton.onClick.RemoveListener(ClickButton);        
    }

    public void Activate()
    {
        if (!isActive)
        {
            return;
        }
        balanceButton.onClick.AddListener(ClickButton);
    }
}
