using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CageManager : MonoBehaviour
{
    [SerializeField] private Button startButton;

    [SerializeField] private Cage cage;

    [SerializeField] private GameObject scanPanel;
    [SerializeField] private Scanner scanner;
    [SerializeField] private Button scanButton;

    private int winMStage;
    private int winRStage;

    private bool move;
    private bool win;

    [Inject] private readonly ResourceController resources;
    [Inject] private readonly InGameEvents eventManager;
    [Inject] private readonly Randomizer random;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        resources.UpdateProgress(0);

        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(Initialize);

        scanButton.onClick.AddListener(Scan);
    }

    private void OnDisable()
    {
        eventManager.GameStateEvent -= Activate;

        startButton.onClick.RemoveListener(Initialize);
        scanButton.onClick.RemoveListener(Scan);
    }

    private void Initialize()
    {
        startButton.gameObject.SetActive(false);
        Activate(true);
        eventManager.GameStateEvent += Activate;        
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            scanPanel.SetActive(true);
            scanButton.interactable = true;
            scanner.ResetScanner(resources.ScanActive);

            cage.GenerateWinStage(random, out winMStage, out winRStage);
            cage.ResetCage();
            cage.SetControls(true);

            
        }
        else
        {
            cage.SetControls(false);
            scanPanel.SetActive(false);
        }
    }

    private void Scan()
    {
        int value = cage.GetCurrentStage();
        bool scannerEnabled = scanner.Scan(random, value == winMStage);
        scanButton.interactable = scannerEnabled;
    }
    
}
