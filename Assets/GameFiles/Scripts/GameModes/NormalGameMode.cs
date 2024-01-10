using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class NormalGameMode : MonoBehaviour
{
    [SerializeField] private Button recipeCheck;
    [SerializeField] private Button unlockIngredient;
    [SerializeField] private Button spice;
    [SerializeField] private Button recipePick;

    [SerializeField] private IngredientSystem ingredientSystem;
    [SerializeField] private HolderSystem holderSystem;
    [SerializeField] private RecipeSystem recipeSystem;

    [SerializeField] private int setSize;
    [SerializeField] private int lockNumber;

    [SerializeField] private int picks;//config
    [SerializeField] private int rounds;

    private int roundNumber;
    private int pickNumber;

    [Inject] private readonly EventHandler events;
    [Inject] private readonly GameData gameData;

    private void Awake()
    {
        holderSystem.InitHolders(PickCallback);
    }

    private void OnEnable()
    {
        recipePick.onClick.AddListener(FinishRound);
        recipeCheck.onClick.AddListener(CheckUnique);
        spice.onClick.AddListener(ForceUnique);

        recipePick.interactable = false;
        recipeCheck.interactable = false;

        recipeSystem.ResetAll();

        events.GameModeEvent += Activate;
    }

    private void OnDisable()
    {
        recipePick.onClick.RemoveListener(FinishRound);
        recipeCheck.onClick.RemoveListener(CheckUnique);
        spice.onClick.RemoveListener(ForceUnique);

        events.GameModeEvent -= Activate;

        DOTween.Kill("ingredient");
    }

    private void Activate(bool activate)
    {
        if (activate)
        {
            recipePick.interactable = false;
            recipeCheck.interactable = false;
            spice.interactable = true;
            //reset ingr

            roundNumber = 1;
            pickNumber = 0;

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
        }        
    }

    private void CheckUnique()
    {
        if (gameData.Checks > 0)
        {
            recipeSystem.CheckRecipe(false);
            gameData.UpdateResData(ResData.Checks, -1);
        }        
    }

    private void PickCallback(ActionType type, int holderId, int ingrId)
    {
        switch (type)
        {
            case ActionType.Unlock:
                if(gameData.Locks > 0)
                {
                    holderSystem.UpdateHolders(holderId, type);
                    gameData.UpdateResData(ResData.Locks, -1);
                }
                break;
            case ActionType.Pick:
                pickNumber++;
                recipeSystem.Pick(ingrId);
                break;
            case ActionType.Unpick:
                pickNumber--;
                recipeSystem.Unpick(ingrId);
                break;
            default: throw new System.NotSupportedException();
        }

        recipePick.interactable = pickNumber >= picks;
        recipeCheck.interactable = pickNumber >= picks;
    }

    private void FinishRound()//pick recipe button
    {
        bool uniqueRecipe = recipeSystem.CheckRecipe(false);
        if (uniqueRecipe)
        {
            recipeSystem.AddRecipe();
        }
        else
        {
            FinishGame(false);
        }

        roundNumber++;
        if(roundNumber >= rounds)
        {
            FinishGame(true);
        }
        else
        {
            pickNumber = 0;
            holderSystem.SetHolders(ingredientSystem.GenerateNew(setSize));
            holderSystem.LockHolders(lockNumber);

            recipeSystem.ResetCurrent();

            spice.interactable = true;
        }
    }



    private void FinishGame(bool win)
    {
        recipeSystem.ResetAll();
        Debug.Log("wim:"+win);
        events.SwitchGameMode(false);
    }
}
