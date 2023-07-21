using System;

public class RandomGenerator
{
    private Random random = new Random();

    public int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public float GenerateFloat(float minValue, float maxValue)
    {
        return minValue + (float) random.NextDouble() * (maxValue - minValue);
    }

    public void RandomizeArray<T>(T[] array, bool all = false)
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
