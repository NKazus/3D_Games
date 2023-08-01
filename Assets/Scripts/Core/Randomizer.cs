using System;
using System.Collections.Generic;

public class Randomizer
{
    private Random random = new Random();

    public int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public void RandomizeList<T>(List<T> array)
    {
        int n = array.Count;
        int k;
        T temp;
        while (n > 1)
        {
            k = random.Next(n--);
            temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
