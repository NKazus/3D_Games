using System;

public enum GardenState
{
    Normal,
    Rain,
    Heat
}
public class State
{
    private GardenState currentState;

    private void UpdateNormal()
    {

    }

    private void UpdateRain()
    {

    }

    private void UpdateHeat()
    {

    }

    public void SetState(GardenState target)
    {
        currentState = target;
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case GardenState.Normal: UpdateNormal(); break;
            case GardenState.Rain: UpdateRain(); break;
            case GardenState.Heat: UpdateHeat(); break;
            default: throw new NotSupportedException();
        }
    }
}
