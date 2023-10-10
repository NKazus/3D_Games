using System;

public class Randomizer
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

    public void RandomizeArray<T>(T[] array)
    {
        int n = array.Length;
        int k;
        T temp;
        while (n > 1)
        {
            k = random.Next(0, n--);
            temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
