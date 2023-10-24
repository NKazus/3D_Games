using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CageManager : MonoBehaviour
{
    [SerializeField] private Button startButton;

    [SerializeField] private GameObject controlPanel;
    [SerializeField] private Button upControlM;
    [SerializeField] private Button downControlM;
    [SerializeField] private Button upControlR;
    [SerializeField] private Button downControlR;

    [SerializeField] private Cage cage;
    [SerializeField] private Timer timer;
    [SerializeField] private int breakAttempts;

    [SerializeField] private GameObject scanPanel;
    [SerializeField] private Scanner scanner;
    [SerializeField] private Button scanButton;

    private int winMStage;
    private int winRStage;

    private int currentAttempt;
    private bool activePhase;

    private bool useExtraCharges;

    [Inject] private readonly ResourceController resources;
    [Inject] private readonly InGameEvents globalEvents;
    [Inject] private readonly Randomizer random;

    private void OnEnable()
    {
        resources.UpdateProgress(0);
        resources.UpdateScanExtra(0);

        startButton.gameObject.SetActive(true);
        SetControls(false, false, true);
        startButton.onClick.AddListener(Initialize);

        scanPanel.SetActive(false);
        scanButton.onClick.AddListener(Scan);

        SwitchControlsListeners(true);
    }

    private void OnDisable()
    {
        timer.Deactivate();
        SwitchControlsListeners(false);

        globalEvents.GameStateEvent -= Activate;

        startButton.onClick.RemoveListener(Initialize);
        scanButton.onClick.RemoveListener(Scan);
    }

    private void Initialize()
    {
        startButton.gameObject.SetActive(false);
        SetControls(true, true, true);

        timer.SetCallback(TimerCallback);
        cage.SetPhaseCallback(ActivePhaseCallback);
        cage.SetRotatorCallback(RotationCallback);
        cage.SetSwitchCallback(BeginRotationCallback);

        Activate(true);
        globalEvents.GameStateEvent += Activate;        
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            scanPanel.SetActive(true);
            scanButton.interactable = true;
            scanner.ResetScanner(resources.ScanActive, true);
            useExtraCharges = false;

            cage.GenerateWinStage(random, out winMStage, out winRStage);
            cage.ResetCage(true);
            SetControls(true, true);
            currentAttempt = 0;            
        }
        else
        {
            SetControls(false, false);
            scanPanel.SetActive(false);
        }
    }

    private void Scan()
    {
        if (useExtraCharges)
        {
            resources.UpdateScanExtra(-1);
        }
        globalEvents.PlayScanner();
        int value = cage.GetCurrentStage();
        bool scannerEnabled = scanner.Scan(random, value == winMStage);

        if (!scannerEnabled)
        {
            if(resources.ExtraScanCharges > 0)
            {
                useExtraCharges = true;
            }
            else
            {
                scanButton.interactable = false;
                resources.SetScanStatus(false);
            }            
        }
    }

    private void RotationCallback(int rotationStage)
    {
        if (!activePhase)
        {
            return;
        }
        if((cage.GetCurrentStage() == winMStage) && (rotationStage == winRStage))
        {
            activePhase = false;
            timer.Deactivate();
            cage.Break();
            globalEvents.PlayBreak();
            resources.UpdateProgress(2);
            resources.SetScanStatus(false);
            globalEvents.DoWin();
            globalEvents.SwitchGameState(false);
        }
        else
        {
            SetControls(false, true);
        }
    }

    private void BeginRotationCallback()
    {
        SetControls(false, false);
    }

    private void TimerCallback()
    {
        globalEvents.PlayVibro();
        activePhase = false;
        cage.ResetCage();
        currentAttempt++;
        if(currentAttempt >= breakAttempts)
        {
            cage.Fail();
            SetControls(false, false);
            globalEvents.PlayAlert();
            globalEvents.SwitchGameState(false);
        }
        else
        {
            SetControls(true, true);
        }
    }

    private void ActivePhaseCallback()
    {
        timer.Activate();
        globalEvents.PlayActive();
        activePhase = true;
    }

    private void SwitchControlsListeners(bool active)
    {
        if (active)
        {
            upControlM.onClick.AddListener(() => cage.SwitchCage(CageAction.MoveUp));
            downControlM.onClick.AddListener(() => cage.SwitchCage(CageAction.MoveDown));
            upControlR.onClick.AddListener(() => cage.SwitchCage(CageAction.RotateFast));
            downControlR.onClick.AddListener(() => cage.SwitchCage(CageAction.RotateSlow));
        }
        else
        {
            upControlM.onClick.RemoveAllListeners();
            downControlM.onClick.RemoveAllListeners();
            upControlR.onClick.RemoveAllListeners();
            downControlR.onClick.RemoveAllListeners();
        }
    }

    private void SetControls(bool mActive, bool rActive, bool visuals = false)
    {
        if (visuals)
        {
            controlPanel.SetActive(mActive);
        }
        else
        {
            upControlM.interactable = downControlM.interactable = mActive;
            upControlR.interactable = downControlR.interactable = rActive;
        }        
    }
}
