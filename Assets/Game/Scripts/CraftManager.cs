using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CraftManager : MonoBehaviour
{
    [SerializeField] private StickComponent left;
    [SerializeField] private StickComponent right;

    [SerializeField] private Button startButton;
    [SerializeField] private Button confButton;
    [SerializeField] private Button careButton;

    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private int tries;
    [SerializeField] private float confValue;
    [SerializeField] private float careValue;

    private int currentTry;
    private float leftScale;
    private float rightScale;

    private const float SUM_VALUE = 2f;

    [Inject] private readonly GameResourceHandler resources;
    [Inject] private readonly GameGlobalEvents eventManager;
    [Inject] private readonly RandomProvider randomGenerator;

    private void OnEnable()
    {
        resources.UpdateTreasure(0);
        resources.UpdateSticks(0);

        winPanel.SetActive(false);
        hintText.SetActive(false);
        currentTry = 0;
        resources.UpdateTries(currentTry);

        startButton.onClick.AddListener(Activate);
        startButton.gameObject.SetActive(true);

        careButton.onClick.AddListener(() => CalculateScale(careValue));
        confButton.onClick.AddListener(() => CalculateScale(confValue));
    }


    private void OnDisable()
    {
        StopAllCoroutines();
        startButton.onClick.RemoveListener(Activate);
        careButton.onClick.RemoveAllListeners();
        confButton.onClick.RemoveAllListeners();
        careButton.gameObject.SetActive(false);
        confButton.gameObject.SetActive(false);

        hintText.SetActive(false);
    }

    private void Activate()
    {
        currentTry = 0;
        resources.UpdateTries(currentTry);

        startButton.gameObject.SetActive(false);
        winPanel.SetActive(false);
        hintText.SetActive(true);
        InitializeSticks();

        careButton.gameObject.SetActive(true);
        confButton.gameObject.SetActive(true);
    }

    private void InitializeSticks()
    {
        left.ResetComponent();
        right.ResetComponent();

        float initCoeff = randomGenerator.GenerateInt(3, 8) / 10f;
        initCoeff *= randomGenerator.GenerateInt(0, 10) > 5 ? 1 : -1;

        leftScale = SUM_VALUE / 2f + initCoeff;
        rightScale = SUM_VALUE - leftScale;

        left.SetScale(leftScale);
        right.SetScale(rightScale);
    }

    private void CalculateScale(float value)
    {
        currentTry++;
        resources.UpdateTries(currentTry);

        float dirValue = leftScale > rightScale ? -1 : 1;
        leftScale += dirValue * value;
        rightScale += (-dirValue) * value;

        left.SetScale(leftScale);
        right.SetScale(rightScale);
        CheckResult();
    }

    private void CheckResult()
    {
        if (Mathf.Abs(leftScale - rightScale) < 0.04)
        {
            left.MergeToTarget(MergeCallback);
            right.MergeToTarget(MergeCallback);
            resources.UpdateSticks(1);
            return;
        }
        if (currentTry >= tries)
        {        
            careButton.gameObject.SetActive(false);
            confButton.gameObject.SetActive(false);
            hintText.SetActive(false);
            eventManager.PlayVibro();
            startButton.gameObject.SetActive(true);
        }        
    }

    private void MergeCallback()
    {
        eventManager.PlayStick();
        winPanel.SetActive(true);
        hintText.SetActive(false);
        careButton.gameObject.SetActive(false);
        confButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }
}
