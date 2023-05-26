using System;

public static class RandomGenerator
{
    private static Random random = new Random();

    public static int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public static void RandomizeArray(int[] array)
    {
        int n = array.Length;
        int k;
        int temp;
        while (n > 1)
        {
            k = random.Next(n--);
            temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
