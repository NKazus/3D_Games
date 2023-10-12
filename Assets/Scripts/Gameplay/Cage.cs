using UnityEngine;

public enum CageAction
{
    MoveUp,
    MoveDown,
    RotateFast,
    RotateSlow
}
public class Cage : MonoBehaviour
{
    [SerializeField] private CageHighlight highlight;
    [SerializeField] private Rotator rotator;
    [SerializeField] private Mover mover;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private MeshRenderer glass;

    private System.Action activePhaseCallback;
    private System.Action switchRotationCallback;

    public void SetPhaseCallback(System.Action callback)
    {
        activePhaseCallback = callback;
    }

    public void SetSwitchCallback(System.Action callback)
    {
        switchRotationCallback = callback;
    }

    public void SetRotatorCallback(System.Action<int> callback)
    {
        rotator.SetCallback(callback);
    }

    public void GenerateWinStage(Randomizer random, out int mStage, out int rStage)
    {
        mStage = random.GetInt(0, mover.GetMovementStagesMax());
        rStage = random.GetInt(1, rotator.GetRotationStagesMax());
    }

    public int GetCurrentStage()
    {
        return mover.GetMovementStageCurrent();
    }

    public void SwitchCage(CageAction action)
    {
        switch (action)
        {
            case CageAction.MoveUp:
                mover.MoveUp();
                break;
            case CageAction.MoveDown:
                mover.MoveDown();
                break;
            case CageAction.RotateFast:
                switchRotationCallback();
                if(rotator.GetRotationStagesCurrent() == 0)
                {
                    activePhaseCallback();
                    highlight.SetHighlight(Highlight.Active);
                }
                rotator.RotateFaster();
                break;
            case CageAction.RotateSlow:
                rotator.RotateSlower();
                break;
            default: throw new System.NotSupportedException();
        }
    }

    public void ResetCage(bool hard = false)
    {
        highlight.SetHighlight(Highlight.Normal);
        rotator.ResetRotator(hard);
        if (hard)
        {
            glass.enabled = true;
            mover.ResetMover(hard);
        }
    }

    public void Break()
    {
        highlight.SetHighlight(Highlight.Normal);
        glass.enabled = false;
        explosion.Play();
    }

    public void Fail()
    {
        highlight.SetHighlight(Highlight.Failed);
    }
}
