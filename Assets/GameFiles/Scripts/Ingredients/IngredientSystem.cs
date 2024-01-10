using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class IngredientSystem : MonoBehaviour //manages queue and provides new combinations
{
    [SerializeField] private Ingredient[] ingredients;

    [Inject] private readonly ValueProvider randProvider;

    public List<Ingredient> GenerateNew(int number)
    {
        if(number > ingredients.Length)
        {
            throw new System.NotSupportedException();
        }

        List<int> ids = new List<int>();
        List<Ingredient> dataSet = new List<Ingredient>();

        int tempId;
        Ingredient tempIngr;

        for(int i = 0; i < number; i++)
        {
            do
            {
                tempIngr = ingredients[randProvider.GenerateInt(0, ingredients.Length)];
                tempId = tempIngr.GetId();
            }
            while (ids.Contains(tempId));
            ids.Add(tempId);
            dataSet.Add(tempIngr);
        }

        return dataSet;
    }
}
