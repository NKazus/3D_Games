using System.Collections.Generic;

public class AttackUnit : Unit
{
    public override void Act(List<Unit> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].GetUnitCategory() != category)
            {
                targets[i].DamageUnit(damage);
            }
        }

        actions--;
        CheckActions();
    }
}
