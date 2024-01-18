using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HolderSystem : MonoBehaviour //places models into positions and provides interactions
{
    [SerializeField] private IngredientHolder[] holders;
    [SerializeField] private Sprite emptyIcon;
    [SerializeField] private Sprite pickedIcon;
    [SerializeField] private Sprite lockedIcon;

    [Inject] private readonly ValueProvider randProvider;

    private void OnEnable()
    {
        for (int i = 0; i < holders.Length; i++)
        {
            holders[i].UpdateHolder(emptyIcon);
        }
    }

    public void InitHolders(System.Action<ActionType, int, int> callback)
    {
        for (int i = 0; i < holders.Length; i++)
        {
            holders[i].InitHolder(i);
            holders[i].SetHolderCallback(callback);
        }
    }

    public void SetHolders(List<Ingredient> ingredients)
    {
        for (int i = 0; i < holders.Length; i++)
        {
            holders[i].SetHolder(ingredients[i]);
            holders[i].SwitchHolderLock(false);
            holders[i].UpdateHolder(emptyIcon);
        }
    }

    public void LockHolders(int number)
    {
        List<int> holderIds = new List<int>();

        int tempId;

        for (int i = 0; i < number; i++)
        {
            do
            {
                tempId = randProvider.GenerateInt(0, holders.Length);
            }
            while (holderIds.Contains(tempId));
            holderIds.Add(tempId);
            holders[tempId].SwitchHolderLock(true);
            holders[tempId].UpdateHolder(lockedIcon);
        }
    }

    public void UpdateHolders(int holderId, ActionType type)
    {
        switch (type)
        {
            case ActionType.Unlock:
                holders[holderId].SwitchHolderLock(false);
                holders[holderId].UpdateHolder(emptyIcon);
                break;
            case ActionType.Pick:
                holders[holderId].SwitchHolderPick();
                holders[holderId].UpdateHolder(pickedIcon);
                break;
            case ActionType.Unpick:
                holders[holderId].UpdateHolder(emptyIcon);
                break;
            default: throw new System.NotSupportedException();
        }
    }
}
