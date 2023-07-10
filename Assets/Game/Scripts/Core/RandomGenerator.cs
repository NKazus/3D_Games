using System;
using System.Collections.Generic;

public static class RandomGenerator
{
    private static Random random = new Random();

    public static int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public static void RandomizeArray<T>(T[] array)
    {
        int n = array.Length;
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

    public static void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        int k;
        T temp;
        while(n > 1)
        {
            k = random.Next(n--);
            temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }
}
