using System;

public static class RandomGenerator
{
    private static Random random = new Random();

    public static int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public static float GenerateFloat(float minValue, float maxValue)
    {
        return minValue + (float) random.NextDouble() * (maxValue - minValue);
    }

    public static void RandomizeArray<T>(T[] array, bool all = false)
    {
        int extra = all ? 0 : 1;
        int n = array.Length;
        int k;
        T temp;
        while (n > 1 + extra)
        {
            k = random.Next(extra, n--);
            temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
