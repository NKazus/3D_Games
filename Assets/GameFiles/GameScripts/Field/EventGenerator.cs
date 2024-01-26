public class EventGenerator
{
    public UnitType GenerateAction(Slot target)
    {
        if (!target.IsEmpty())
        {
            return UnitType.None;
        }

        return UnitType.Park;
    }
}
