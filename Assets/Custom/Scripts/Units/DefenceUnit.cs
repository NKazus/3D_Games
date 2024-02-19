using System.Collections.Generic;

namespace CMGame.Gameplay
{
    public class DefenceUnit : Unit
    {
        public override void Act(List<Unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].GetUnitCategory() == category)
                {
                    targets[i].UpdateHp();
                }
            }

            FinishAction();
        }
    }
}
