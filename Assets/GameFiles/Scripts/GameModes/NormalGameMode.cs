using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class NormalGameMode : MonoBehaviour
{
    [SerializeField] private bool advanced;
    [SerializeField] private int advancedFee;

    [SerializeField] private Button recipeCheck;
    [SerializeField] private Button spice;
    [SerializeField] private Button recipePick;

    [SerializeField] private Text picksT;
    //[SerializeField] private Text roundsT;

    [SerializeField] private IngredientSystem ingredientSystem;
    [SerializeField] private HolderSystem holderSystem;
    [SerializeField] private RecipeSystem recipeSystem;

    [SerializeField] private Timer timer;
    [SerializeField] private GameInfo messages;

    [SerializeField] private int setSize;
    [SerializeField] private int lockNumber;

    [SerializeField] private int picks;
    [SerializeField] private int rounds;

    private int roundNumber;
    private int pickNumber;

    [Inject] private readonly EventHandler events;
    [Inject] private readonly GameData gameData;
    [Inject] private readonly ValueProvider rand;

    private void Awake()
    {
        holderSystem.InitHolders(PickCallback);

        if (advanced)
        {
            timer.SetCallback(HandleTimeout);
        }
    }

    private void OnEnable()
    {
        recipePick.onClick.AddListener(FinishRound);

        recipeCheck.onClick.AddListener(CheckUnique);
        spice.onClick.AddListener(ForceUnique);     

        recipePick.interactable = false;
        recipeCheck.interactable = false;

        recipeSystem.ResetAll();

        if (advanced)
        {
            spice.interactable = false;

            timer.SwitchVisibility(true);
            timer.Refresh();            
        }
        else
        {
            timer.SwitchVisibility(false);
        }

        events.GameModeEvent += Activate;

        gameData.RefreshResData();
    }

    private void OnDisable()
    {
        if (advanced)
        {
            timer.Deactivate();
        }

        recipePick.onClick.RemoveListener(FinishRound);
        recipeCheck.onClick.RemoveListener(CheckUnique);
        spice.onClick.RemoveListener(ForceUnique);

        events.GameModeEvent -= Activate;

        DOTween.Kill("ingredient");
        DOTween.Kill("game_info");
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            
            recipePick.interactable = false;

            roundNumber = 1;
            pickNumber = 0;
            //roundsT.text = roundNumber + "/" + rounds;
            picksT.text = pickNumber + "/" + picks;

            if (advanced)
            {
                timer.Refresh();

                if(gameData.GameScore < advancedFee)
                {
                    messages.ShowInfo(MessageType.AdvanceDisabled);
                    return;
                }
                else
                {
                    messages.ShowInfo(MessageType.AdvanceEnabled);
                    gameData.UpdateResData(ResData.Points, -advancedFee);
                }
                timer.Activate(2f);
            }
            else
            {
                recipeCheck.interactable = false;
                spice.interactable = true;
            }

            holderSystem.SetHolders(ingredientSystem.GenerateNew(setSize));
            holderSystem.LockHolders(lockNumber);
        }
    }

    private void ForceUnique()
    {
        if (gameData.Spices > 0)
        {
            recipeSystem.Force();
            gameData.UpdateResData(ResData.Spices, -1);
            spice.interactable = false;

            messages.ShowInfo(MessageType.ForceUnique);
            events.PlaySound(GameSound.Spice);
        }        
    }

    private void CheckUnique()
    {
        if (gameData.Checks > 0)
        {
            bool result = recipeSystem.CheckRecipe(false);
            messages.ShowInfo(result ? MessageType.CheckUnique : MessageType.CheckEqual);
            gameData.UpdateResData(ResData.Checks, -1);
            events.PlaySound(GameSound.Check);
        }        
    }

    private void PickCallback(ActionType type, int holderId, int ingrId)
    {
        //Debug.Log("Pick:"+type + " ingr:"+ingrId);
        switch (type)
        {
            case ActionType.Unlock:
                if(gameData.Locks > 0)
                {
                    holderSystem.UpdateHolders(holderId, type);
                    gameData.UpdateResData(ResData.Locks, -1);
                    events.PlaySound(GameSound.Unlock);
                }
                break;
            case ActionType.Pick:
                if (pickNumber >= picks)
                {
                    return;
                }
                pickNumber++;
                picksT.text = pickNumber + "/" + picks;
                recipeSystem.Pick(ingrId);
                holderSystem.UpdateHolders(holderId, type);
                break;
            case ActionType.Unpick:
                pickNumber--;
                picksT.text = pickNumber + "/" + picks;
                recipeSystem.Unpick(ingrId);
                holderSystem.UpdateHolders(holderId, type);
                break;
            default: throw new System.NotSupportedException();
        }

        if (!advanced)
        {
            recipeCheck.interactable = pickNumber >= picks;
        }
        recipePick.interactable = pickNumber >= picks;
    }

    private void FinishRound()//pick recipe button
    {
        if (advanced)
        {
            timer.Deactivate();
        }

        bool uniqueRecipe = recipeSystem.CheckRecipe(false);
        if (uniqueRecipe)
        {
            recipeSystem.AddRecipe();
            events.PlaySound(GameSound.Check);
        }
        else
        {
            FinishGame(false);
            return;
        }

        roundNumber++;
        //roundsT.text = roundNumber + "/" + rounds;
        if(roundNumber >= rounds)
        {
            FinishGame(true);
        }
        else
        {
            pickNumber = 0;
            picksT.text = pickNumber + "/" + picks;
            holderSystem.SetHolders(ingredientSystem.GenerateNew(setSize));
            holderSystem.LockHolders(lockNumber);

            recipeSystem.ResetCurrent();            

            if (advanced)
            {
                timer.Activate();
            }
            else
            {
                spice.interactable = true;
                recipeCheck.interactable = false;
            }
            recipePick.interactable = false;
        }
    }

    public void HandleTimeout()
    {
        FinishGame(false);
    }

    private void FinishGame(bool win)
    {
        if (advanced)
        {
            timer.Deactivate();
        }

        recipeSystem.ResetAll();

        if (win)
        {
            if (advanced)
            {
                gameData.UpdateResData(ResData.Spices, 1);
                gameData.UpdateResData(ResData.Points, 20);
                events.PlayEndGame(EndGameState.AdvancedWin, roundNumber + 5);
            }
            else
            {
                bool pickFirst = rand.GenerateInt(0, 2) < 1;
                gameData.UpdateResData(pickFirst ? ResData.Checks : ResData.Locks, 3);
                gameData.UpdateResData(ResData.Points, roundNumber);
                events.PlayEndGame(pickFirst ? EndGameState.NormalWin1 : EndGameState.NormalWin2, roundNumber);
            }
        }
        else
        {
            int reward;
            if (advanced)
            {
                reward = 0;              
            }
            else
            {
                reward = roundNumber;
            }
            gameData.UpdateResData(ResData.Points, reward);
            events.PlayEndGame(EndGameState.CommonLose, reward);
        }
        events.PlaySound(win ? GameSound.Win : GameSound.Lose);
        events.SwitchGameMode(false);
    }
}
