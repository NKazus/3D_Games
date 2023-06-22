using System;

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
        while (n > 2)
        {
            k = random.Next(1, n--);
            temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
