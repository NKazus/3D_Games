using System;

public class ValueProvider
{
    private Random random = new Random();

    public int GenerateInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }
}
