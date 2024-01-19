using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeSystem : MonoBehaviour
{
    private List<List<int>> recipes = new List<List<int>>();
    private List<int> currentRecipe = new List<int>();

    private bool forcedUnique;
    private bool isEqualFound;

    public void Pick(int id)
    {
        currentRecipe.Add(id);
    }

    public void Unpick(int id)
    {
        currentRecipe.Remove(id);
    }

    public bool CheckRecipe(bool inOrder)
    {
        if (!inOrder)
        {
            currentRecipe.Sort();
        }

        if(recipes.Count == 0)
        {
            return true;
        }

        isEqualFound = false;

        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i].SequenceEqual(currentRecipe))
            {
                isEqualFound = true;
            }
        }
        
        return forcedUnique ? true : (!isEqualFound);
    }

    public void AddRecipe()
    {
        if (forcedUnique && isEqualFound)
        {
            return;
        }

        List<int> newRecipe = new List<int>();

        for (int i = 0; i < currentRecipe.Count; i++)
        {
            newRecipe.Add(currentRecipe[i]);
        }

        recipes.Add(newRecipe);
    }

    public void Force()
    {
        forcedUnique = true;
    }

    public void ResetCurrent()
    {
        forcedUnique = false;
        currentRecipe.Clear();
    }

    public void ResetAll()
    {
        currentRecipe.Clear();
        recipes.Clear();
    }
}
