using System;

public class Randomizer
{
    private Random random = new Random();

    public float GenerateFloat(float minValue, float maxValue)
    {
        return minValue + (maxValue - minValue) * (float)random.NextDouble();
    }

    public void RandomizeArray<T>(T[] array)
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

}
