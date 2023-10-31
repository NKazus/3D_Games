using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button tryButton;

    [SerializeField] private Stick stick;

    [SerializeField] private Gear gear;
    [SerializeField] private Pin[] pins;
    [SerializeField] private HighlightController highlight;

    private int stage;
    private int[] colors;

    private bool move;
    private bool win;

    [Inject] private readonly GameResourceHandler resources;
    [Inject] private readonly GameGlobalEvents eventManager;
    [Inject] private readonly RandomProvider random;

    private void Awake()
    {
        colors = new int[4];
        for(int i = 0; i < 4; i++)
        {
            colors[i] = i;
        }
    }

    private void OnEnable()
    {
        resources.UpdateTreasure(0);
        resources.UpdateSticks(0);

        tryButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(Initialize);
    }

    private void OnDisable()
    {
        highlight.ResetHighlight();
        eventManager.GameStateEvent -= Activate;

        startButton.onClick.RemoveAllListeners();
        tryButton.onClick.RemoveAllListeners();
    }

    private void Initialize()
    {
        startButton.onClick.RemoveListener(Initialize);
        startButton.gameObject.SetActive(false);
        Activate(true);
        eventManager.GameStateEvent += Activate;        
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            resources.UpdateSticks(0);

            move = false;
            win = false;
            gear.ActivateGear(activate);
            for (int i = 0; i < pins.Length; i++)
            {
                pins[i].ResetPin();
            }

            random.RandomizeArray(colors);
            stage = 0;
            highlight.UpdateHighlight(colors[stage]);
            stick.SetStick(stage, StickCallback);

            eventManager.ButtonPressEvent += CheckStage;
            eventManager.StickEvent += CheckSticks;
        }
        else
        {
            for (int i = 0; i < pins.Length; i++)
            {
                pins[i].DeactivatePin();
            }

            tryButton.onClick.RemoveAllListeners();
            tryButton.gameObject.SetActive(false);
            
            eventManager.ButtonPressEvent -= CheckStage;
            eventManager.StickEvent -= CheckSticks;
        }
    }

    private void StickCallback()
    {
        pins[stage].ActivatePin();

        tryButton.image.color = Color.white;
        tryButton.gameObject.SetActive(true);
        tryButton.onClick.AddListener(MoveStick);        
    }


    private void MoveStick()
    {
        tryButton.image.color = Color.gray;
        tryButton.onClick.RemoveListener(MoveStick);
        stick.MoveStick(MoveCallback);
    }

    private void MoveCallback()
    {
        if (win)
        {
            return;
        }
        if (move)
        {
            highlight.UpdateHighlight(colors[stage]);
            stick.SetStick(stage, StickCallback);
            move = false;
        }
        else
        {
            tryButton.image.color = Color.white;
            tryButton.onClick.AddListener(MoveStick);
        }
    }

    private void CheckStage(int id)
    {
        if(colors[stage] == id)
        {
            pins[stage].DeactivatePin();
            tryButton.image.color = Color.gray;
            tryButton.onClick.RemoveListener(MoveStick);
            if(++stage >= 4)
            {
                gear.ActivateGear(false);
                highlight.ResetHighlight();
                win = true;
                eventManager.DoWin();
                eventManager.PlayReward();
                resources.UpdateTreasure(1);
                eventManager.SwitchGame(false);
            }
            else
            {
                move = true;
            }
        }
    }

    private void CheckSticks()
    {
        eventManager.PlayBreak();
        resources.UpdateSticks(-1);
        int sticks = resources.Sticks;

        if (sticks <= 0)
        {
            eventManager.SwitchGame(false);
            return;
        }

        stick.SetStick(stage);
        tryButton.image.color = Color.white;
        tryButton.gameObject.SetActive(true);
        tryButton.onClick.AddListener(MoveStick);
    }
}
