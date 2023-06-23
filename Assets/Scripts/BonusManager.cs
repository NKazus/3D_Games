using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private Transform targetBoard;
    [SerializeField] private Transform pickBoard;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private BonusCrystal targetCrystal;
    [SerializeField] private BonusCrystal[] crystals;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject pickText;
    [SerializeField] private GameObject winPanel;

    private CrystalColor[] crystalColors;

    private void Awake()
    {
        crystalColors = new CrystalColor[crystals.Length];
    }

    private void OnEnable()
    {
        winPanel.SetActive(false);
        startButton.onClick.AddListener(Activate);
        startButton.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        DOTween.KillAll();
        StopAllCoroutines();
        ResetBoards();
        startButton.onClick.RemoveListener(Activate);
        pickText.SetActive(false);
    }

    private void Activate()
    {
        startButton.gameObject.SetActive(false);
        winPanel.SetActive(false);
        ResetBoards();
        CalculateColors();
        targetBoard.DOMove(targetPosition, 2f)
            .SetId(this)
            .OnComplete(() => { StartCoroutine(Wait()); });
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        targetBoard.DOMove(startPosition, 2f)
            .SetId(this)
            .OnComplete(() => { MoveCrystals(); });
    }

    private void MoveCrystals()
    {
        pickBoard.DOMove(targetPosition, 2f)
            .SetId(this)
            .OnComplete(() => {
                for (int i = 0; i < crystals.Length; i++)
                {
                    crystals[i].Activate(CheckResult);
                }
                pickText.SetActive(true);
            });
    }

    private void CalculateColors()
    {
        Color targetColor = targetCrystal.SetRandom();
        Color hue;
        float axisDir;
        crystalColors[0].color = targetColor;
        crystalColors[0].id = 1;
        for(int i = 1; i < crystalColors.Length; i++)
        {
            axisDir = RandomGenerator.GenerateInt(0, 5) > 5 ? 1f : -1f;
            hue = new Color(
                Mathf.Clamp01(targetColor.r + axisDir * RandomGenerator.GenerateFloat(0.15f, 0.5f)),
                Mathf.Clamp01(targetColor.g + axisDir * RandomGenerator.GenerateFloat(0.15f, 0.5f)),
                Mathf.Clamp01(targetColor.b + axisDir * RandomGenerator.GenerateFloat(0.15f, 0.5f)),
                targetColor.a);
            crystalColors[i].color = hue;
            crystalColors[i].id = -1;
        }
        RandomGenerator.RandomizeArray(crystalColors, true);
        for(int i = 0; i < crystals.Length; i++)
        {
            crystals[i].SetHue(crystalColors[i]);
        }
    }


    private void CheckResult(int id)
    {
        pickText.SetActive(false);
        for (int i = 0; i < crystals.Length; i++)
        {
            crystals[i].Deactivate();
        }
        if (id > 0)
        {
            GlobalEventManager.PlayBonus();
            dataHandler.SetBonusTime();
            winPanel.SetActive(true);
        }
        else
        {
            GlobalEventManager.PlayVibro();
        }

        startButton.gameObject.SetActive(true);
    }

    private void ResetBoards()
    {
        targetBoard.position = startPosition;
        pickBoard.position = new Vector3(startPosition.x, startPosition.y, -startPosition.z);
    }
}
