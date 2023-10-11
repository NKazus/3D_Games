using UnityEngine;
using UnityEngine.UI;


public class Cage : MonoBehaviour
{
    [SerializeField] private CageHighlight highlight;
    [SerializeField] private Rotator rotator;
    [SerializeField] private Mover mover;

    [SerializeField] private Button upControlM;
    [SerializeField] private Button downControlM;
    [SerializeField] private Button upControlR;
    [SerializeField] private Button downControlR;

    private void OnEnable()
    {
        upControlM.onClick.AddListener(() => mover.MoveUp());
        downControlM.onClick.AddListener(() => mover.MoveDown());
        upControlR.onClick.AddListener(() => rotator.RotateFaster());
        downControlR.onClick.AddListener(() => rotator.RotateSlower());
    }

    private void OnDisable()
    {
        upControlM.onClick.RemoveAllListeners();
        downControlM.onClick.RemoveAllListeners();
        upControlR.onClick.RemoveAllListeners();
        downControlR.onClick.RemoveAllListeners();
    }

    public void SetControls(bool active)
    {
        upControlM.interactable = downControlM.interactable = upControlR.interactable = downControlR.interactable = active;
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

    public void ActivateCage()
    {

    }

    public void ResetCage()
    {
        highlight.SetHighlight(Highlight.Normal);
        rotator.ResetRotator();
        mover.ResetMover();
    }
}
