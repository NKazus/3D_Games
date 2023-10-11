using System;

public class Randomizer
{
    private Random random = new Random();

    public int GetInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public float GetFloat(float minValue, float maxValue)
    {
        return minValue + (float) random.NextDouble() * (maxValue - minValue);
    }
}
