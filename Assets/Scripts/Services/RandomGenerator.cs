using System;

public class RandomProvider
{
    private Random random = new Random();

    public int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public float GenerateFloat(float minValue, float maxValue)
    {
        return minValue + (float)random.NextDouble() * (maxValue - minValue);
    }
}
