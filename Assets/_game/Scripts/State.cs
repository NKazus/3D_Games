using System;

public enum GardenState
{
    Normal = 0,
    Rain = 1,
    Heat = 2
}
public class State
{
    private Random rand;
    private int stateNumber;

    public State()
    {
        rand = new Random();
        stateNumber = Enum.GetNames(typeof(GardenState)).Length;
    }

    private GardenState UpdateNormal()
    {
        return (GardenState)rand.Next(0, stateNumber);
    }

    private GardenState UpdateRain()
    {
        int val = rand.Next(0,10);
        return (val > 4) ? GardenState.Heat : GardenState.Normal;
    }

    private GardenState UpdateHeat()
    {
        return (GardenState)rand.Next(0, stateNumber - 1);
    }

    public GardenState UpdateState(GardenState currentState)
    {
        GardenState newState;
        switch (currentState)
        {
            case GardenState.Normal: newState = UpdateNormal(); break;
            case GardenState.Rain: newState = UpdateRain(); break;
            case GardenState.Heat: newState = UpdateHeat(); break;
            default: throw new NotSupportedException();
        }
        return newState;
    }
}
