using System;

public class RandomValueGenerator
{
    private Random random = new Random();

    public int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
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
