using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class GravityMode : MonoBehaviour
{
    [SerializeField] private BlobController blobController;
    [SerializeField] private WallController wallController;
    [SerializeField] private Shield shield;

    [Header("Direction buttons (see tooltip)")]
    [Tooltip("Order: Top, Right, Bottom, Left")]
    [SerializeField] private Button[] controls;
    [SerializeField] private int turns;
    [SerializeField] private Button staticWallButton;
    [SerializeField] private Button turnButton;
    [SerializeField] private float shiftStep;

    [SerializeField] private WallMode wallMode;

    private int currentTurn;
    private int directionsNumber;

    private bool staticControlActive;

    [Inject] private readonly GlobalEvents events;
    [Inject] private readonly RandomProvider random;
    [Inject] private readonly GameData data;

    private void Awake()
    {
        directionsNumber = Enum.GetNames(typeof(WallDirection)).Length;
    }

    private void OnEnable()
    {
        events.GameEvent += ActivateGameplay;
        data.UpdateResourceValue(GameResource.Score, 0);
        data.UpdateResourceValue(GameResource.Walls, 0);
        data.UpdateRoundsValue(0);
    }

    private void OnDisable()
    {
        events.GameEvent -= ActivateGameplay;
        staticWallButton.onClick.RemoveListener(SwitchStaticControls);
        SwitchListeners(false);

        if (IsInvoking())
        {
            CancelInvoke();
        }
        DOTween.Kill("mat_instance");
        DOTween.Kill("blob");
    }

    private void ActivateGameplay(bool activate)
    {
        if (activate)
        {
            currentTurn = 0;
            data.UpdateRoundsValue(currentTurn);
            wallController.ResetAll();
            wallController.ResetCurrent();
            blobController.ResetBlob();
            shield.ShiftColor(false);

            SwitchListeners(true);
            staticControlActive = false;
            wallMode.SwitchMode(false);
            if (data.WallNumber > 0)
            {
                staticWallButton.onClick.AddListener(SwitchStaticControls);
                staticWallButton.image.DOFade(1f, 0.4f);
            }
            else
            {
                staticWallButton.image.DOFade(0.5f, 0.4f);
            }
            turnButton.onClick.AddListener(ApplyGravity);
        }
        else
        {
            staticWallButton.onClick.RemoveListener(SwitchStaticControls);
            turnButton.onClick.RemoveListener(ApplyGravity);
            SwitchListeners(false);
        }
    }

    private void SwitchStaticControls()
    {
        SwitchListeners(false);
        if (staticControlActive)
        {
            SwitchListeners(true);
        }
        else
        {
            for (int i = 0; i < directionsNumber; i++)
            {
                int dir = i;
                controls[i].onClick.AddListener(() => SetStaticDirection((WallDirection)dir));
            }
        }
        staticControlActive = !staticControlActive;
        wallMode.SwitchMode(staticControlActive);
    }

    private void SwitchListeners(bool add)
    {
        if (add)
        {
            for (int i = 0; i < directionsNumber; i++)
            {
                int dir = i;
                controls[i].onClick.AddListener(() => SetDirection((WallDirection)dir));
            }
        }
        else
        {
            for (int i = 0; i < directionsNumber; i++)
            {
                controls[i].onClick.RemoveAllListeners();
            }
        }
    }

    private void SetDirection(WallDirection dir)
    {
        wallController.MoveWall(dir);
        events.PlayWall(false);
    }

    private void SetStaticDirection(WallDirection dir)
    {
        wallController.SetStatic(dir);
        events.PlayWall(true);
        data.UpdateResourceValue(GameResource.Walls, -1);
        SwitchStaticControls();
        staticWallButton.onClick.RemoveListener(SwitchStaticControls);
        staticWallButton.image.DOFade(0.5f, 0.4f);
    }

    private WallDirection GenerateGravity()
    {
        return (WallDirection) random.GenerateInt(0, directionsNumber);
    }

    private void ApplyGravity()
    {
        turnButton.onClick.RemoveListener(ApplyGravity);
        SwitchListeners(false);
        WallDirection newDirection = GenerateGravity();
        bool blockedGravity = wallController.ApplyGravity(newDirection);
        if (!blockedGravity)
        {
            Vector3 shiftDir = newDirection switch
            {
                WallDirection.Top => new Vector3(0, 0, shiftStep),
                WallDirection.Bottom => new Vector3(0, 0, -shiftStep),
                WallDirection.Left => new Vector3(-shiftStep, 0, 0),
                WallDirection.Right => new Vector3(shiftStep, 0, 0),
                _ => throw new NotSupportedException()
            };
            blobController.MoveBlob(shiftDir, CalculateTurn);
            events.PlayGravity();
        }
        else
        {
            Invoke("SkipTurn", 1f);
        }
    }

    private void SkipTurn()
    {
        CalculateTurn(true);
    }

    private void CalculateTurn(bool isBlobInside)
    {
        if (!isBlobInside)
        {
            data.UpdateResourceValue(GameResource.Score, -5);
            events.PlayVibro();
            shield.ShiftColor(true);
            Invoke("Finish", 1f);
            return;
        }
        currentTurn++;
        data.UpdateRoundsValue(currentTurn);

        if (currentTurn >= turns)
        {
            events.DoWin(10);
            data.UpdateResourceValue(GameResource.Score, 10);
            events.PlayReward();
            Invoke("Finish", 1f);
        }
        else
        {
            turnButton.onClick.AddListener(ApplyGravity);
            SwitchListeners(true);
        }
    }

    private void Finish()
    {
        events.SwitchGravityMode(false);
    }
}
