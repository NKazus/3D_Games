using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScannerSetup : MonoBehaviour
{
    [SerializeField] private ScannerElement target;

    [SerializeField] private Button startButton;
    [SerializeField] private Button strongButton;
    [SerializeField] private Button weakButton;

    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject setupText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject triesPanel;

    [SerializeField] private int tries;
    [SerializeField] private float strongValue;
    [SerializeField] private float weakValue;

    private int currentTry;
    private float positionShiftL;
    private float positionShiftU;

    private const float PATH_VALUE = 1f;

    [Inject] private readonly ResourceController resources;
    [Inject] private readonly InGameEvents eventManager;
    [Inject] private readonly Randomizer randomGenerator;

    private void OnEnable()
    {
        resources.UpdateProgress(0);

        winPanel.SetActive(false);
        hintText.SetActive(false);
        setupText.SetActive(false);

        triesPanel.SetActive(false);

        if (!resources.ScanActive)
        {
            EnableSetup();
        }
        else
        {
            setupText.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Finish");
        }

        startButton.onClick.RemoveListener(Activate);
        weakButton.onClick.RemoveAllListeners();
        strongButton.onClick.RemoveAllListeners();
        weakButton.gameObject.SetActive(false);
        strongButton.gameObject.SetActive(false);

        hintText.SetActive(false);
    }

    private void EnableSetup()
    {
        currentTry = 0;
        resources.UpdateTries(currentTry);

        startButton.onClick.AddListener(Activate);
        startButton.gameObject.SetActive(true);

        weakButton.onClick.AddListener(() => CalculateScale(weakValue));
        strongButton.onClick.AddListener(() => CalculateScale(strongValue));
    }

    private void Activate()
    {
        currentTry = 0;
        resources.UpdateTries(currentTry);

        startButton.gameObject.SetActive(false);
        winPanel.SetActive(false);
        hintText.SetActive(true);
        InitializeComponents();

        weakButton.gameObject.SetActive(true);
        strongButton.gameObject.SetActive(true);
        triesPanel.SetActive(true);
    }

    private void InitializeComponents()
    {
        float initCoeff = randomGenerator.GetInt(2, 4) / 10f;
        initCoeff *= randomGenerator.GetInt(0, 10) > 5 ? 1 : -1;

        positionShiftL = PATH_VALUE / 2f + initCoeff;
        Debug.Log("init l shift:" + positionShiftL);
        positionShiftU = PATH_VALUE - positionShiftL;

        target.SetPosition(positionShiftL);
    }

    private void CalculateScale(float value)
    {
        currentTry++;
        resources.UpdateTries(currentTry);

        float dirValue = positionShiftL > positionShiftU ? -1 : 1;
        positionShiftL += dirValue * value;
        Debug.Log("l shift:" + positionShiftL);
        positionShiftU = PATH_VALUE - positionShiftL;

        target.SetPosition(positionShiftL);
        CheckResult();
    }

    private void CheckResult()
    {
        if (Mathf.Abs(positionShiftL - positionShiftU) < 0.02)
        {
            resources.SetScanStatus(true);
            resources.UpdateProgress(-1);
            Invoke("Finish", 1f);            
            return;
        }
        if (currentTry >= tries)
        {        
            weakButton.gameObject.SetActive(false);
            strongButton.gameObject.SetActive(false);
            hintText.SetActive(false);
            eventManager.PlayVibro();
            startButton.gameObject.SetActive(true);
        }        
    }

    private void Finish()
    {
        eventManager.PlayStick();
        winPanel.SetActive(true);
        hintText.SetActive(false);
        weakButton.gameObject.SetActive(false);
        strongButton.gameObject.SetActive(false);
        triesPanel.SetActive(false);
    }
}
